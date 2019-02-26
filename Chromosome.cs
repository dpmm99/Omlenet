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

        public Chromosome(int foodCount, int targetFoodUnits) //TODO: Should I get a weight discount for grams of water in a given food?
        {
            foods = new int[foodCount];
            //Set random initial foods
            var rnd = new Random();
            while (targetFoodUnits-- > 0)
            {
                var x = rnd.Next(foodCount);
                foods[x]++;
            }
            //TODO: May try building a decent meal plan with high-nutrient foods for a starting point.
        }

        public Chromosome(Chromosome toMutate)
        {
            foods = toMutate.foods.ToArray();
            //Find some food mass to redistribute
            var rnd = new Random();
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

        public Chromosome(Chromosome ma, Chromosome pa, int targetFoodUnits)
        {
            foods = new int[ma.foods.Length];
            var rnd = new Random();
            var maTarget = rnd.Next(2, targetFoodUnits - 2);
            var taking = 0;
            targetFoodUnits -= maTarget; //targetFoodUnits is now paTarget

            //Crossbreed in such a way that the total number of food units stays constant //TODO: Should I allow some fluctuation to targetFoodUnits?
            for (var x = 0; x < ma.foods.Length && (targetFoodUnits > 0 || maTarget > 0); x++)
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
    }
}
