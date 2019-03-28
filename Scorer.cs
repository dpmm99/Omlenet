using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omlenet
{
    /// <summary>
    /// Sums nutrients and scores combinations thereof. Parallel usage of the same Scorer is viable, as long as the targets, always-included nutrients, and food nutrients are constant.
    /// </summary>
    public class Scorer
    {
        private int nutrientArraySize;
        private NutrientTarget[] adjustedTargets;
        private float[] alwaysInclude;
        private FoodNutrient[][] foodNutrientLookup;

        public NutrientTarget[] Targets { get { return adjustedTargets.Select(p => p.Clone()).ToArray(); } } //Just wasting time doing a deep copy <3

        //Pass in ProgramState.GetTrueTargets()
        //FoodNutrientLookup is only needed if you're going to use ScoreChromosome currently
        public Scorer(List<NutrientTarget> targets, float targetMultiplier, float[] presum, List<Nutrient> nutrients, FoodNutrient[][] foodNutrientLookup = null)
        {
            this.foodNutrientLookup = foodNutrientLookup;
            nutrientArraySize = nutrients.Max(p => p.id) + 1;
            adjustedTargets = new NutrientTarget[nutrientArraySize];
            foreach (var target in targets)
            {
                adjustedTargets[target.nutrientId] = target.Clone();
                adjustedTargets[target.nutrientId].costUnder /= targetMultiplier;
                adjustedTargets[target.nutrientId].costOver /= targetMultiplier;
                adjustedTargets[target.nutrientId].target *= targetMultiplier;
                adjustedTargets[target.nutrientId].min *= targetMultiplier;
                adjustedTargets[target.nutrientId].max *= targetMultiplier;
                if (adjustedTargets[target.nutrientId].min <= 0) adjustedTargets[target.nutrientId].min = -0.1f; //To avoid machine epsilon when calculating nutrient cost differences without slowing down scoring itself
            }

            alwaysInclude = presum;
            //alwaysInclude = new float[nutrientArraySize];
            //foreach (var nut in presum)
            //{
            //    alwaysInclude[nut.nutrientId] += nut.nutrientAmount;
            //}
        }

        /// <summary>
        /// Calculate the score of the chromosome as-is minus the same chromosome with the given FoodNutrients subtracted from it
        /// </summary>
        public float ScoreDifference(Chromosome c, FoodNutrient[] excludeForDifference, ref float[] scoreSpace)
        {
            var scoreWith = Score(c, ref scoreSpace);

            //scoreSpace now has the sum of that chromosome's foods plus the alwaysInclude foods, so all we have to do is modify it and use the protected score()
            foreach (var change in excludeForDifference)
            {
                scoreSpace[change.nutrientId] -= change.nutrientAmount;
            }

            var scoreWithout = score(scoreSpace);
            return scoreWith - scoreWithout;
        }
        //TODO: Make sure these accept the optimally efficient inputs. (Maybe you should accept a count in the above, for example)

        //Expects oldNutrients to be the nutrientAmounts reference from a previous call to Score(Chromosome c, ref float[] nutrientAmounts)
        public float ScoreDifference(float oldScore, float[] oldNutrients, FoodNutrient[] changes, float amount, ref float[] scoreSpace)
        {
            if (scoreSpace == null) scoreSpace = new float[nutrientArraySize];
            Array.Copy(oldNutrients, scoreSpace, oldNutrients.Length);

            //scoreSpace now has the sum of that chromosome's foods plus the alwaysInclude foods, so all we have to do is modify it and use the protected score()
            foreach (var change in changes)
            {
                scoreSpace[change.nutrientId] += change.nutrientAmount * amount;
            }

            var newScore = score(scoreSpace);
            return oldScore - newScore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="nutrientAmounts">Per-thread preallocated scoring space, to avoid the cost of allocating and garbage collecting for every scoring</param>
        /// <returns></returns>
        public float Score(Chromosome c, ref float[] nutrientAmounts)
        {
            if (nutrientAmounts == null) nutrientAmounts = new float[nutrientArraySize];
            Array.Copy(alwaysInclude, nutrientAmounts, alwaysInclude.Length);

            for (int x = c.foods.Length - 1; x >= 0; x--)
            {
                if (c.foods[x] != 0)
                {
                    for (int y = foodNutrientLookup[x].Length - 1; y >= 0; y--)
                    {
                        nutrientAmounts[foodNutrientLookup[x][y].nutrientId] += foodNutrientLookup[x][y].nutrientAmount * c.foods[x];
                    }
                }
            }
            return score(nutrientAmounts);
        }

        //For the hand-calculator
        public float Score(List<FoodAmount> foods)
        {
            var nutrientAmounts = new float[nutrientArraySize];

            foreach (var food in foods)
            {
                foreach (var nut in food.GetNutrients())
                {
                    nutrientAmounts[nut.nutrientId] += nut.nutrientAmount * food.amount / 100;
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
        protected float score(float[] foodNutrients)
        {
            float sum = 0;
            foreach (var target in adjustedTargets)
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
