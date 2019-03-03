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
        private bool winnerChanged = true; //To save a tiny amount of time when the UI requests the winner's info again

        private List<FoodNutrient>[] foodDict;
        private List<FoodNutrient> foodNutrients;
        private List<FoodDescription> foodDescs;
        private List<Nutrient> nutrients;
        private List<NutrientTarget> targets;
        private int targetFoodUnits;

        public bool executed;
        public bool HasWinner { get { return winner != null; } }

        public GASolver(List<FoodDescription> foodDescs, List<NutrientTarget> targets, List<Nutrient> nutrients, List<FoodNutrient> foodNutrients, int targetFoodUnits, int targetGenerations = 75000, int populationSize = 48)
        {
            this.foodDescs = foodDescs.ToList();
            this.nutrients = nutrients.ToList();
            this.foodNutrients = foodNutrients.ToList();
            this.targetFoodUnits = targetFoodUnits;
            this.targetGenerations = targetGenerations;
            this.populationSize = populationSize;
            this.UpdateTargets(targets);

            //Prepare foods as a pseudo-dictionary array
            //TODO: Do this the other direction. It'll probably be a lot faster if you linearly go through the foodNutrients linearly and add them to a food lookup and convert it to an array at the end.
            foodDict = foodDescs.Select(p => foodNutrients.BinarySearch(q => q.foodId, p.id)).ToArray(); //foodNutrients are sorted by foodId.
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

        private string GenerateChromosomeText(Chromosome c)
        {
            if (c == null) return "";

            //Look up data for displaying
            var testOutput = new StringBuilder();
            //testOutput.AppendLine("Score: " + Math.Round(100000 / (1000 + c.score), 1) + " / 100");
            if (c.score == 0) c.score = scoreChromosome(c); //We don't want to display 0 pointlessly
            testOutput.AppendLine("Cost: " + Math.Round(c.score));

            var nutrientTotals = new List<Tuple<ushort, float, string>>();
            for (var x = 0; x < c.foods.Length; x++)
            {
                if (c.foods[x] != 0)
                {
                    var foodItem = foodDescs.First(p => p.id == foodDict[x][0].foodId);
                    testOutput.AppendLine("(" + foodDict[x][0].foodId + ") " + foodItem.longDesc);
                    testOutput.AppendLine("    " + (c.foods[x] * 100) + "g");
                    for (var y = 0; y < foodDict[x].Count; y++)
                    {
                        var nutrient = nutrients.First(p => p.id == foodDict[x][y].nutrientId);
                        var range = targets.FirstOrDefault(p => p.nutrientId == nutrient.id);
                        var trueNutrientAmount = c.foods[x] * foodDict[x][y].nutrientAmount;
                        var percent = range != null && range.target != 0 ? " (" + Math.Round(trueNutrientAmount * 700 / range.target, 1) + "% DV)" : "";
                        testOutput.AppendLine("    " + nutrient.name + ": " + trueNutrientAmount + nutrient.unitOfMeasure + percent);

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
                    testOutput.AppendLine();
                }
            }

            //TODO: maybe sort nutrient totals? Or maybe not...
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

        public void SetFood(int id, int count)
        {
            if (winner == null) winner = new Chromosome(foodDict.Length, 0);

            var index = foodDescs.FindIndex(p => p.id == id);
            winner.foods[index] = count;
        }

        public string GetWinnerText()
        {
            lock (this)
            {
                if (winnerChanged)
                {
                    winnerText = GenerateChromosomeText(winner);
                    winnerChanged = false;
                }

                return winnerText;
            }
        }

        private void GeneticAlgorithm()
        {
            var population = GeneratePopulation(populationSize, foodDict.Length, targetFoodUnits);
            if (winner != null) //If continuing from an existing result, include the old winner
            {
                population.RemoveAt(population.Count - 1);
                population.Add(winner);
            }

            for (; generation < targetGenerations; generation++)
            {
#if PARALLEL
                Parallel.For(0, population.Count, x =>
#else
                for (var x = 0; x < population.Count; x++)
#endif
                {
                    if (population[x].score == 0) //Don't rescore needlessly (unless maybe there's an absolutely perfect chromosome somehow)
                        population[x].score = scoreChromosome(population[x]);
                }
#if PARALLEL
                );
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

            //TODO: remap the data in a way that is optimized for getting a food from a nutrient (when one is lacking)
            //Maybe a dictionary of lists, where the key is the nutrient ID and the list is sorted by amount of that nutrient in the food.
            //Then you can binary search the list for the exact amount if needed. This might be useful when the week's plan is almost full.
            //It might also be useful if certain nutrients are found to be very difficult to obtain.
            //TODO: The remapped data should also exclude anything the user specifically said they want to avoid (such as a food group)
            //TODO: Allow user to override Range objects
        }

        //I experimented with a greedy approach just once, but it took 36 seconds and did not give an even remotely good result.
        //It might be okay for filling in the final slot, though! //TODO: Try using it for the last food occasionally when crossing over or mutating
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
                Parallel.For(0, c.foods.Length, x =>
#else
                for (int x = 0; x < c.foods.Length; x++)
#endif
                {
                    if (direction < 0 && c.foods[x] == 0) //Can't subtract from 0 units
#if PARALLEL
                        return;
#else
                        continue;
#endif
                    c.foods[x] += direction;
                    var tempScore = scoreChromosome(c);
                    if (tempScore < bestScore)
                    {
                        bestScore = tempScore;
                        bestIndex = x;
                    }
                    c.foods[x] -= direction;
                }
#if PARALLEL
                );
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
            var foodIdToNewIndexMapping = foodDict.ToDictionary(p => p[0].foodId, p => updatedFoodDescs.FindIndex(q => q.id == p[0].foodId));
            var oldFoods = (int[])winner.foods.Clone();
            winner.foods = new int[updatedFoodDescs.Count]; //Resize the array
            Random rnd = new Random();
            for (var x = 0; x < oldFoods.Length; x++)
            {
                //If the old food still exists in the new list, correct its index
                var newIndex = foodIdToNewIndexMapping[foodDict[x][0].foodId];
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
            foodDict = foodDescs.Select(p => foodNutrients.BinarySearch(q => q.foodId, p.id)).ToArray(); //foodNutrients are sorted by foodId.
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
        /// <param name="foods">A list of food nutrients per food (array element)</param>
        /// <param name="ranges"></param>
        /// <returns></returns>
        private float scoreChromosome(Chromosome c)
        {
            //var foodNutrientsIncluded = new List<FoodNutrient>();
            //for (int x = 0; x < c.foods.Length; x++)
            //{
            //    for (int y = 0; y < c.foods[x]; y++) //TODO: Be efficient. Why build a list with repeat entries when you can just sum the nutrients?
            //    {
            //        foodNutrientsIncluded.AddRange(foodDict[x]);
            //    }
            //}
            //return score(foodNutrientsIncluded);

            //This one was about 15% faster until I added the "if c.foods[x] != 0," and now it takes about 1/30 as long
            var nutrientAmounts = new float[nutrients.Max(p => p.id) + 1]; //Hopefully no nutrient has a very big ID
            for (int x = 0; x < c.foods.Length; x++)
            {
                if (c.foods[x] != 0)
                {
                    for (int y = 0; y < foodDict[x].Count; y++)
                    {
                        nutrientAmounts[foodDict[x][y].nutrientId] += foodDict[x][y].nutrientAmount * c.foods[x];
                    }
                }
            }
            return score(nutrientAmounts);

            //Alternate method! This actually took 3x as long. :|
            var nutrientAmountById = new Dictionary<ushort, float>();
            for (int x = 0; x < c.foods.Length; x++)
            {
                for (int y = 0; y < foodDict[x].Count; y++)
                {
                    if (nutrientAmountById.ContainsKey(foodDict[x][y].nutrientId))
                    {
                        nutrientAmountById[foodDict[x][y].nutrientId] += foodDict[x][y].nutrientAmount * c.foods[x];
                    }
                    else nutrientAmountById.Add(foodDict[x][y].nutrientId, foodDict[x][y].nutrientAmount * c.foods[x]);
                }
            }

            return score(nutrientAmountById);
        }

        /// <summary>
        /// Calculate an inverse score based on the given ranges (should already be filtered by body type). (Higher is worse.)
        /// </summary>
        /// <param name="foodNutrients"></param>
        /// <param name="ranges"></param>
        /// <returns></returns>
        private float score(List<FoodNutrient> foodNutrients)
        {
            float sum = 0;
            foreach (var target in targets)
            {
                var total = foodNutrients.Where(p => p.nutrientId == target.nutrientId).Sum(p => p.nutrientAmount);
                if (total < target.min || total > target.max) sum += 1000; //Exceeding the limits is a huge cost
                if (total < target.target) sum += (target.target - total) * target.costUnder; //Cost per unit differs for over or under target
                else if (total > target.target) sum += (total - target.target) * target.costOver;
            }
            return sum;
        }

        /// <summary>
        /// Calculate an inverse score based on the given ranges (should already be filtered by body type). (Higher is worse.)
        /// </summary>
        /// <param name="foodNutrients"></param>
        /// <param name="ranges"></param>
        /// <returns></returns>
        private float score(Dictionary<ushort, float> foodNutrients)
        {
            float sum = 0;
            foreach (var target in targets)
            {
                if (!foodNutrients.TryGetValue(target.nutrientId, out var total)) total = 0;
                if (total < target.min || total > target.max) sum += 1000; //Exceeding the limits is a huge cost
                if (total < target.target) sum += (target.target - total) * target.costUnder; //Cost per unit differs for over or under target
                else if (total > target.target) sum += (total - target.target) * target.costOver;
            }
            return sum;
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
