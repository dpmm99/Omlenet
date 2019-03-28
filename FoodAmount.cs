using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omlenet
{
    public class FoodAmount
    {
        public int foodId;
        public float amount;

        public FoodAmount(int foodId, float amount)
        {
            this.foodId = foodId;
            this.amount = amount;
        }

        public FoodDescription GetFood()
        {
            return ProgramState.foodDescs.First(p => p.id == foodId);
        }

        public FoodNutrient[] GetNutrients()
        {
            return ProgramState.foodNutrientDict[foodId];
        }
    }
}
