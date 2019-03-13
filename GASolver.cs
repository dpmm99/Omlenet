#define PARALLEL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Omlenet
{
    public class GASolver
    {
        private Thread worker = null;
        private int populationSize;
        private int generation;
        private int targetGenerations;
        private Chromosome winner = null;
        private string winnerText = null;
        private List<ResultListItem> winnerFoods = null;
        private bool winnerChanged = true; //To save a tiny amount of time when the UI requests the winner's info again

        private List<FoodNutrient>[] nutrientsByChromosomeIndex;
        private Dictionary<int, List<FoodNutrient>> nutrientsByFoodId;
        private List<FoodDescription> foodDescs;
        private List<Nutrient> nutrients;
        private List<NutrientTarget> targets;
        private int targetFoodUnits;

        private Dictionary<int, int> lockedFoodCounts = new Dictionary<int, int>();
        private float[] lockedFoodNutrients;
        private List<FoodDescription> lockedFoodDescs = new List<FoodDescription>(); //so we can remove them from the other list

        public bool executed;
        public bool HasWinner { get { return winner != null; } }

        public GASolver(List<FoodDescription> foodDescs, List<NutrientTarget> targets, List<Nutrient> nutrients, Dictionary<int, List<FoodNutrient>> foodNutrientsDict, HashSet<int> lockedFoods, int targetFoodUnits, int targetGenerations = 75000, int populationSize = 48)
        {
            this.nutrientsByFoodId = foodNutrientsDict;
            this.foodDescs = foodDescs.ToList();
            this.nutrients = nutrients.ToList();
            this.targetFoodUnits = targetFoodUnits;
            this.targetGenerations = targetGenerations;
            this.populationSize = populationSize;
            this.UpdateTargets(targets);
            this.UpdateLockedFoods(lockedFoods);
        }

        /// <summary>
        /// Get the foods in the winner as a dictionary from ID to unit count
        /// </summary>
        public Dictionary<int, int> GetWinningFoods()
        {
            var ret = new Dictionary<int, int>();
            for (var x = 0; x < winner.foods.Length; x++)
            {
                if (winner.foods[x] != 0) ret.Add(foodDescs[x].id, winner.foods[x]);
            }
            foreach (var kv in lockedFoodCounts) ret.Add(kv.Key, kv.Value); //Include locked foods
            return ret;
        }

        public void Start()
        {
            Stop();

            generation = 0;
            executed = true;
            if (HasWinner) winner.score = 0; //Reset score in case the goals were changed
            worker = new Thread(GeneticAlgorithm);
            worker.Start(); //Don't do the work on the UI thread, so that the UI remains usable
        }

        /// <summary>
        /// Calculate that nutrient's effect on the chromosome's cost (assuming this is the last nutrient added and there is a winning chromosome)
        /// </summary>
        public float CalculateNutrientCostDifference(ushort nutrientId, float nutrientAmount)
        {
            if (!HasWinner) return 0;

            var scoreSpace = new float[lockedFoodNutrients.Length];
            var tempFoodNutrients = new float[lockedFoodNutrients.Length];
            Array.Copy(lockedFoodNutrients, tempFoodNutrients, lockedFoodNutrients.Length);

            var scoreWith = scoreChromosome(winner, ref scoreSpace, lockedFoodNutrients);
            tempFoodNutrients[nutrientId] -= nutrientAmount;
            var scoreWithout = scoreChromosome(winner, ref scoreSpace, tempFoodNutrients);

            return scoreWith - scoreWithout;
        }

        public void Stop()
        {
            if (worker != null)
            {
                generation = targetGenerations;
                worker.Join();
                worker = null;
            }
        }

        /// <returns>Completion progress as a percentage (floored, so 100 is actually completely done)</returns>
        public int GetProgress()
        {
            var progress = (generation * 100 / targetGenerations);
            if (progress == 100 || progress < 0)
            {
                try
                {
                    worker.Join();
                } catch { } //Could already be null
                return 100;
            }
            return progress;
        }

        public void UpdateFoodMass(int targetFoodUnits)
        {
            //Pick some foods to add to/remove from the winner (if any) to meet the target food unit requirement
            this.targetFoodUnits = targetFoodUnits;
            if (HasWinner)
            {
                //Instead of assuming the old targetFoodUnits was correct, count how many foods the old winner had--it's not hard.
                AssignFoodsGreedily(winner, targetFoodUnits - winner.foods.Sum(p => p));
                winner.score = 0;
                winnerChanged = true;
            }
        }

        private void RemoveArrayElement(ref int[] array, int idx)
        {
            var newArray = new int[array.Length - 1];
            if (idx > 0) Array.Copy(array, newArray, idx);
            if (idx < array.Length - 1) Array.Copy(array, idx + 1, newArray, idx, newArray.Length - idx);
            array = newArray;
        }

        public void UpdateLockedFoods(HashSet<int> foodLocked)
        {
            foodLocked.RemoveWhere(p => !lockedFoodCounts.ContainsKey(p) && !foodDescs.Any(q => q.id == p));

            //lockedFoodCounts -- if an item is already present in there, leave it alone. If one is present and needs removed, move its count into winner
            if (!HasWinner) winner = new Chromosome(foodDescs.Count, targetFoodUnits); //Because if any food is locked with a count, we need a place to put it when unlocking
            var keys = lockedFoodCounts.Select(p => p.Key).ToList();
            foreach (var foodId in keys)
            {
                //If it was locked and is no longer supposed to be locked, unlock it
                if (!foodLocked.Contains(foodId))
                {
                    //Update lockedFoodDescs and foodDescs
                    var idx = lockedFoodDescs.FindIndex(p => p.id == foodId);
                    foodDescs.Add(lockedFoodDescs[idx]);
                    lockedFoodDescs.RemoveAt(idx);

                    //Update winner chromosome
                    var newFoodsCount = new int[foodDescs.Count];
                    Array.Copy(winner.foods, newFoodsCount, winner.foods.Length);
                    newFoodsCount[foodDescs.Count - 1] = lockedFoodCounts[foodId];
                    winner.foods = newFoodsCount;

                    //Update lockedFoodCounts
                    lockedFoodCounts.Remove(foodId);
                }
            }

            foreach (var foodId in foodLocked)
            {
                //If it was unlocked and is now supposed to be locked, lock it
                if (!lockedFoodCounts.ContainsKey(foodId))
                {
                    var idx = foodDescs.FindIndex(p => p.id == foodId);
                    //Update winner chromosome
                    var count = winner.foods[idx];
                    RemoveArrayElement(ref winner.foods, idx);
                    //Update lockedFoodCounts
                    lockedFoodCounts.Add(foodId, count);

                    //Update lockedFoodDescs and foodDescs
                    lockedFoodDescs.Add(foodDescs[idx]);
                    foodDescs.RemoveAt(idx);
                }
            }

            UpdateLockedNutrientAmounts();

            //Prepare foods as a pseudo-dictionary array
            nutrientsByChromosomeIndex = foodDescs.Select(p => nutrientsByFoodId[p.id]).ToArray();
        }

        private List<ResultListItem> GenerateChromosomeFoodList(Chromosome c)
        {
            var ret = new List<ResultListItem>();

            for (var x = 0; x < c.foods.Length; x++)
            {
                if (c.foods[x] != 0)
                {
                    var item = foodDescs.First(p => p.id == nutrientsByChromosomeIndex[x][0].foodId);
                    ret.Add(new ResultListItem { Id = item.id, Name = item.longDesc, Mass = c.foods[x] * 100 });
                }
            }

            //Locked foods
            foreach (var item in lockedFoodDescs)
            {
                var count = lockedFoodCounts[item.id];
                ret.Add(new ResultListItem { Id = item.id, Name = item.longDesc, Mass = count * 100 });
            }

            return ret;
        }

        private void GenerateFoodText(FoodDescription food, int count, StringBuilder sb, List<Tuple<ushort, float, string>> nutrientTotals)
        {
            sb.AppendLine("(" + nutrientsByFoodId[food.id][0].foodId + ") " + food.longDesc);
            sb.AppendLine("    " + (count * 100) + "g");
            for (var y = 0; y < nutrientsByFoodId[food.id].Count; y++)
            {
                var nutrient = nutrients.First(p => p.id == nutrientsByFoodId[food.id][y].nutrientId);
                var range = targets.FirstOrDefault(p => p.nutrientId == nutrient.id);
                var trueNutrientAmount = count * nutrientsByFoodId[food.id][y].nutrientAmount;
                var percent = range != null && range.target != 0 ? " (" + Math.Round(trueNutrientAmount * 700 / range.target, 1) + "% DV)" : "";
                sb.AppendLine("    " + nutrient.name + ": " + trueNutrientAmount + nutrient.unitOfMeasure + percent);

                var totalIdx = nutrientTotals.FindIndex(p => p.Item1 == nutrient.id);
                if (totalIdx < 0)
                {
                    nutrientTotals.Add(new Tuple<ushort, float, string>(nutrient.id, trueNutrientAmount, nutrient.name));
                }
                else
                {
                    nutrientTotals[totalIdx] = new Tuple<ushort, float, string>(nutrient.id, nutrientTotals[totalIdx].Item2 + trueNutrientAmount, nutrient.name);
                }
            }
            sb.AppendLine();
        }

        private string GenerateChromosomeText(Chromosome c)
        {
            if (c == null) return "";

            //Look up data for displaying
            var testOutput = new StringBuilder();
            //testOutput.AppendLine("Score: " + Math.Round(100000 / (1000 + c.score), 1) + " / 100");
            var scoringSpace = (float[])null;
            if (c.score == 0) c.score = scoreChromosome(c, ref scoringSpace, lockedFoodNutrients); //We don't want to display 0 pointlessly
            testOutput.AppendLine("Cost: " + Math.Round(c.score));

            var nutrientTotals = new List<Tuple<ushort, float, string>>();
            for (var x = 0; x < c.foods.Length; x++)
            {
                if (c.foods[x] != 0)
                {
                    var foodItem = foodDescs.First(p => p.id == nutrientsByChromosomeIndex[x][0].foodId);
                    GenerateFoodText(foodItem, c.foods[x], testOutput, nutrientTotals);
                }
            }
            foreach (var food in lockedFoodCounts)
            {
                var foodItem = lockedFoodDescs.First(p => p.id == food.Key);
                GenerateFoodText(foodItem, food.Value, testOutput, nutrientTotals);
            }

            //Nutrient totals
            testOutput.AppendLine();
            testOutput.AppendLine("Nutrient Totals:");
            for (var x = 0; x < nutrientTotals.Count; x++)
            {
                var range = targets.FirstOrDefault(p => p.nutrientId == nutrientTotals[x].Item1);
                var percent = range != null && range.target != 0 ? " (" + Math.Round(nutrientTotals[x].Item2 * 100 / range.target, 1) + "%) " : " ";
                testOutput.AppendLine(nutrientTotals[x].Item2 + percent + nutrientTotals[x].Item3);
            }
            return testOutput.ToString();
        }

        private void UpdateLockedNutrientAmounts() //TODO: It's sufficient for this to happen only before running the GA and before updating results, but is it slow enough to matter? (being called in SetFood)
        {
            //Generate lockedFoodNutrients from lockedFoodCounts
            lockedFoodNutrients = new float[nutrients.Max(p => p.id) + 1]; //Hopefully no nutrient has a very big ID
            foreach (var kv in lockedFoodCounts.Where(p => p.Value != 0))
            {
                foreach (var nutrient in nutrientsByFoodId[kv.Key])
                {
                    lockedFoodNutrients[nutrient.nutrientId] += nutrient.nutrientAmount * kv.Value;
                }
            }
        }

        //For use when loading a file
        public void SetFoods(Dictionary<int, int> foodCountsById)
        {
            var foodUnitsBeingSet = foodCountsById.Sum(p => p.Value);
            //Make a random chromosome (the count may not be correct here since some are being overwritten by the passed-in amounts)
            winner = new Chromosome(nutrientsByChromosomeIndex.Length, targetFoodUnits - foodUnitsBeingSet);

            var atLeastOneLockedFood = false;
            foreach (var food in foodCountsById)
            {
                if (lockedFoodCounts.ContainsKey(food.Key))
                {
                    lockedFoodCounts[food.Key] = food.Value;
                }
                else
                {
                    var index = foodDescs.FindIndex(p => p.id == food.Key);
                    winner.foods[index] = food.Value;
                }
            }

            if (atLeastOneLockedFood) UpdateLockedNutrientAmounts();
        }

        //For use in immediate response to user input (which therefore has to be locked while the GA is running)
        public void SetFood(int id, int count)
        {
            if (winner == null) winner = new Chromosome(nutrientsByChromosomeIndex.Length, targetFoodUnits);

            if (lockedFoodCounts.ContainsKey(id))
            {
                lockedFoodCounts[id] = count;
                UpdateLockedNutrientAmounts();
            }
            else
            {
                //TODO: It might not be in the foodDescs list if it wasn't enabled last time you were executing
                //TODO: In that case, immediately update foodDescs (but you have to do so before calling this method)
                var index = foodDescs.FindIndex(p => p.id == id);
                if (index == -1) return;
                winner.foods[index] = count;
            }
        }

        public Tuple<string, List<ResultListItem>> GetWinner()
        {
            lock (this)
            {
                if (winnerChanged)
                {
                    winnerText = GenerateChromosomeText(winner);
                    winnerFoods = GenerateChromosomeFoodList(winner);
                    winnerChanged = false;
                }

                return new Tuple<string, List<ResultListItem>>(winnerText, winnerFoods);
            }
        }

        private void GeneticAlgorithm()
        {
            //Temporarily remove the locked foods from the within-chromosome total food mass
            targetFoodUnits -= lockedFoodCounts.Sum(p => p.Value);

            var population = GeneratePopulation(populationSize, nutrientsByChromosomeIndex.Length, targetFoodUnits);
            if (winner != null) //If continuing from an existing result, include the old winner
            {
                population.RemoveAt(population.Count - 1);
                population.Add(winner);

                //Make sure winner has the right total food mass (in case the user changed food amounts with SetFood)
                var diff = targetFoodUnits - winner.foods.Sum(p => p);
                AssignFoodsGreedily(winner, diff);
            }

            for (; generation < targetGenerations; generation++)
            {
#if PARALLEL
                Parallel.For(0, population.Count, () => (float[])null, (x, state, scoringSpace) =>
#else
                var scoringSpace = (float[])null;
                for (var x = 0; x < population.Count; x++)
#endif
                {
                    if (population[x].score == 0) //Don't rescore needlessly (unless maybe there's an absolutely perfect chromosome somehow)
                        population[x].score = scoreChromosome(population[x], ref scoringSpace, lockedFoodNutrients);
#if PARALLEL
                    return null;
                }
                , p => { }
                );
#else
                }
#endif
                population = population.OrderBy(p => p.score).ToList();

                lock (this)
                {
                    if (winner == null || winner.score != population[0].score)
                    winner = population[0];
                    winnerChanged = true;
                }

                if (generation < targetGenerations - 1) population = BreedNewPopulation(population, 8, 100, 1, targetFoodUnits);
            }

            //Restore mass of locked foods
            targetFoodUnits += lockedFoodCounts.Sum(p => p.Value);

            //TODO: remap the data in a way that is optimized for getting a food from a nutrient (when one is lacking)
            //Maybe a dictionary of lists, where the key is the nutrient ID and the list is sorted by amount of that nutrient in the food.
            //Then you can binary search the list for the exact amount if needed. This might be useful when the week's plan is almost full.
            //It might also be useful if certain nutrients are found to be very difficult to obtain.
        }

        //I experimented with a greedy approach just once, but it took 36 seconds and did not give an even remotely good result.
        //It might be okay for filling in the final slot, though!
        //In the end, I decided to use this as a rare mutation or when the user increases/decreases the targetFoodUnits.
        private Chromosome GenerateGreedyChromosome(int foodCount, int targetFoodUnits)
        {
            var c = new Chromosome(foodCount, targetFoodUnits);
            AssignFoodsGreedily(c, targetFoodUnits);
            return c;
        }

        /// <summary>
        /// Add or remove the specified number of units of food.
        /// This is very expensive, so it runs in full parallel.
        /// </summary>
        private Chromosome AssignFoodsGreedily(Chromosome c, int targetFoodUnits)
        {
            var direction = (targetFoodUnits > 0 ? 1 : -1);
            while (targetFoodUnits != 0)
            {
                //Score every possible food at this step and select the one that makes it the best
                var bestIndex = 0;
                float bestScore = 1000000;

#if PARALLEL
                //scoringSpace is a partition-local (one or more per thread, but not shared between threads) array used for summing nutrient amounts
                Parallel.For(0, c.foods.Length, () => (float[])null, (x, state, scoringSpace) =>
#else
                var scoringSpace = (float[])null;
                for (int x = 0; x < c.foods.Length; x++)
#endif
                {
                    if (direction < 0 && c.foods[x] == 0) //Can't subtract from 0 units
#if PARALLEL
                        return null;
#else
                        continue;
#endif
                    c.foods[x] += direction;
                    var tempScore = scoreChromosome(c, ref scoringSpace, lockedFoodNutrients);
                    if (tempScore < bestScore)
                    {
                        bestScore = tempScore;
                        bestIndex = x;
                    }
                    c.foods[x] -= direction;

#if PARALLEL
                    return null;
                }
                , p => { }
                );
#else
                }
#endif
                c.foods[bestIndex] += direction;
                targetFoodUnits -= direction;
            }
            return c;
        }

        private List<Chromosome> GeneratePopulation(int size, int foodCount, int targetFoodUnits)
        {
            var ret = new List<Chromosome>();

            while (size-- > 0)
            {
                ret.Add(new Chromosome(foodCount, targetFoodUnits));
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldPopulation"></param>
        /// <param name="survivalRate">Number of top chromosomes to use for breeding and mutation</param>
        /// <param name="mutationChance">Tenths of a percent chance for mutation</param>
        /// <param name="greedyChance">Tenths of a percent chance for dropping the worst and then adding the best food greedily (an expensive operation)</param>
        /// <returns></returns>
        private List<Chromosome> BreedNewPopulation(List<Chromosome> oldPopulation, int survivalRate, int mutationChance, int greedyChance, int targetFoodUnits)
        {
            var ret = new List<Chromosome>();
            var rnd = new Random();

            greedyChance += mutationChance; //Stack probabilities on top of each other for easier randomization

            var nextIndex = 0;
            ret.Add(oldPopulation[0].Clone()); //The fittest one shall always survive
            while (ret.Count < oldPopulation.Count)
            {
                //Reuse the top <survivalRate> chromosomes for breeding and mutating
                var val = rnd.Next(1000);
                if (val < mutationChance) ret.Add(new Chromosome(oldPopulation[nextIndex]));
                else if (val < greedyChance)
                {
                    var c = oldPopulation[nextIndex].Clone();
                    AssignFoodsGreedily(c, -1);
                    AssignFoodsGreedily(c, 1);
                    ret.Add(c);
                }
                else
                {
                    var alterIndex = rnd.Next(survivalRate);
                    ret.Add(new Chromosome(oldPopulation[nextIndex], oldPopulation[alterIndex], targetFoodUnits));
                }

                nextIndex = (nextIndex + 1) % survivalRate;
            }

            return ret;
        }

        public void UpdateFoodList(List<FoodDescription> updatedFoodDescs)
        {
            //Drop locked foods that aren't in the passed-in list, because they can't be locked if they're not enabled
            var foodsThatCanBeLocked = updatedFoodDescs.Where(p => lockedFoodCounts.ContainsKey(p.id)).ToLookup(p => p.id);
            for (var x = 0; x < lockedFoodDescs.Count; x++)
            {
                if (!foodsThatCanBeLocked[lockedFoodDescs[x].id].Any())
                {
                    lockedFoodCounts.Remove(lockedFoodDescs[x].id);
                    //lockedFoodNutrients will update later
                    lockedFoodDescs.RemoveAt(x);
                    x--;
                }
            }

            //Exclude foods that were already locked
            updatedFoodDescs = updatedFoodDescs.Where(p => !lockedFoodCounts.ContainsKey(p.id)).ToList();

            //This takes a long time, so first, let's just check if the list actually needs updated.
            if (updatedFoodDescs.Count == foodDescs.Count)
            {
                var needsUpdate = false;
                for (var x = 0; x < foodDescs.Count; x++)
                {
                    if (foodDescs[x].id != updatedFoodDescs[x].id)
                    {
                        needsUpdate = true;
                        break;
                    }
                }
                if (!needsUpdate) return;
            }

            //Have to go through the old and new lists simultaneously and see if any foods were added/removed
            var foodIdToNewIndexMapping = nutrientsByChromosomeIndex.ToDictionary(p => p[0].foodId, p => updatedFoodDescs.FindIndex(q => q.id == p[0].foodId));
            var oldFoods = (int[])winner.foods.Clone();
            winner.foods = new int[updatedFoodDescs.Count]; //Resize the array
            Random rnd = new Random();
            for (var x = 0; x < oldFoods.Length; x++)
            {
                //If the old food still exists in the new list, correct its index
                var newIndex = foodIdToNewIndexMapping[nutrientsByChromosomeIndex[x][0].foodId];
                if (newIndex != -1)
                {
                    winner.foods[newIndex] = oldFoods[x];
                }
                else if (oldFoods[x] != 0)
                {
                    //Randomly allocate those mass points to another, still-available food
                    winner.foods[rnd.Next(winner.foods.Length)] += oldFoods[x];
                }
            }
            winner.score = 0;
            winnerChanged = true;

            foodDescs = updatedFoodDescs;
            nutrientsByChromosomeIndex = foodDescs.Select(p => nutrientsByFoodId[p.id]).ToArray();
        }

        public void UpdateTargets(List<NutrientTarget> targets)
        {
            this.targets = targets.Select(p => p.Clone()).ToList();

            //Targets are saved as daily values, so we'll multiply by 7 here to get weekly values
            foreach (var range in this.targets)
            {
                range.min *= 7;
                range.max *= 7;
                range.target *= 7;
                range.costOver /= 7;
                range.costUnder /= 7;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="nutrientAmounts">Per-thread preallocated scoring space, to avoid the cost of allocating and garbage collecting for every scoring</param>
        /// <returns></returns>
        private float scoreChromosome(Chromosome c, ref float[] nutrientAmounts, float[] extraNutrientAmounts)
        {
            if (nutrientAmounts == null) //Array.Clear(nutrientAmounts, 0, nutrientAmounts.Length);
            //else
                nutrientAmounts = new float[nutrients.Max(p => p.id) + 1]; //Hopefully no nutrient has a very big ID
            Array.Copy(extraNutrientAmounts, nutrientAmounts, extraNutrientAmounts.Length);

            for (int x = 0; x < c.foods.Length; x++)
            {
                if (c.foods[x] != 0)
                {
                    for (int y = 0; y < nutrientsByChromosomeIndex[x].Count; y++)
                    {
                        nutrientAmounts[nutrientsByChromosomeIndex[x][y].nutrientId] += nutrientsByChromosomeIndex[x][y].nutrientAmount * c.foods[x];
                    }
                }
            }
            return score(nutrientAmounts);
        }

        /// <summary>
        /// Calculate an inverse score based on the given ranges (should already be filtered by body type). (Higher is worse.)
        /// </summary>
        /// <param name="foodNutrients"></param>
        /// <param name="ranges"></param>
        /// <returns></returns>
        private float score(float[] foodNutrients)
        {
            float sum = 0;
            foreach (var target in targets)
            {
                var total = foodNutrients[target.nutrientId];
                if (total < target.min || total > target.max) sum += 1000; //Exceeding the limits is a huge cost
                if (total < target.target) sum += (target.target - total) * target.costUnder; //Cost per unit differs for over or under target
                else if (total > target.target) sum += (total - target.target) * target.costOver;
            }
            return sum;
        }
    }
}
