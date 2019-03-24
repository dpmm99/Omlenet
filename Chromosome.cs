using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omlenet
{
    public class Chromosome
    {
        public int[] foods; //Each element is a quantity multiplier for the food with that index matching its (temporary) internal index in the list that was used to create it
        public float score;

        private Chromosome() { }

        //TODO: Everything is designed around 100g units of food with a fixed number of total units.
        //Consider an alternate approach: what if you fix the food mass and adjust the number of food items around a goal? (All nutrient amounts would be multiplied by targetMass/totalMass/100)
        //Are there other ways of doing it that work better? Is it more important to go for a total number of calories than an amount of mass? How do you control the number of possibilities, then?
        //If you target a calorie amount, you can randomly flip bits on or off until you're near that amount when mutating. Crossover is a different story.
        //For crossover, you might have to take ~half the calories from one chromosome and then take from the other until you reach the target.

        //Every method needs to accept a random number generator because: 
        //1. Chromosomes would otherwise end up with the same random numbers when generated at about the same time and
        //2. I don't want to waste time synchronizing if we use a single, static Random and use these constructors on multiple threads simultaneously
        public Chromosome(int foodCount, int targetFoodUnits, Random rnd) //TODO: Should I get a weight discount for grams of water in a given food?
        {
            foods = new int[foodCount];
            //Set random initial foods
            while (targetFoodUnits-- > 0)
            {
                var x = rnd.Next(foodCount);
                foods[x]++;
            }
        }

        public Chromosome(Chromosome toMutate, Random rnd)
        {
            foods = toMutate.foods.ToArray();
            //Find some food mass to redistribute
            var toRedistribute = 0;
            for (var x = 0; x < foods.Length; x++)
            {
                if (foods[x] > 0 && rnd.Next(100) < 30) //% chance
                {
                    toRedistribute += foods[x];
                    foods[x] = 0;
                }
            }

            //Redistribute
            while (toRedistribute-- > 0)
            {
                var x = rnd.Next(foods.Length);
                foods[x]++;
            }
        }

        public Chromosome(Chromosome ma, Chromosome pa, int targetFoodUnits, Random rnd)
        {
            foods = new int[ma.foods.Length];
            var maTarget = targetFoodUnits > 4 ? rnd.Next(2, targetFoodUnits - 2) : rnd.Next(targetFoodUnits) + 1;
            var taking = 0;
            targetFoodUnits -= maTarget; //targetFoodUnits is now paTarget

            //Crossbreed in such a way that the total number of food units stays constant //TODO: Should I allow some fluctuation to targetFoodUnits?
            int x = rnd.Next(foods.Length); //Pick a random starting point to decrease probability of duplicate results in one generation
            for (; x < foods.Length && (targetFoodUnits > 0 || maTarget > 0); x++)
            {
                if (maTarget > 0 && ma.foods[x] > 0)
                {
                    taking = Math.Min(ma.foods[x], maTarget);
                    maTarget -= taking;
                    foods[x] += taking;
                }
                else if (targetFoodUnits > 0 && pa.foods[x] > 0)
                {
                    taking = Math.Min(pa.foods[x], targetFoodUnits);
                    foods[x] += taking;
                    targetFoodUnits -= taking;
                }
                if (targetFoodUnits > 0 && maTarget > 0 && x == foods.Length - 1) x = -1; //Make sure you always end with enough foods--restart the loop at x=0
            }
        }

        public Chromosome Clone()
        {
            return new Chromosome
            {
                score = score,
                foods = foods.ToArray()
            };
        }

#if DEBUG
        //List the indices that matter, with an 'x' and a count if there's more than 1 of that food
        public override string ToString()
        {
            return string.Join(",", foods.Select((count, index) => new { count, index }).Where(p => p.count > 0).Select(p => p.index + (p.count != 1 ? "x" + p.count : "")));
        }
#endif
    }
}
