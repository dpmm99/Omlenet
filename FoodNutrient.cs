using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Omlenet.USDAFormat;

namespace Omlenet
{
    public class FoodNutrient
    {
        public int foodId; //NDB_No
        public ushort nutrientId; //Nutr_No
        public float nutrientAmount; //Nutr_Val

        public static FoodNutrient FromStream(StreamReader sr)
        {
            var fields = parseLine(sr.ReadLine(), 3);
            if (fields.Count < 3) return null;
            return new FoodNutrient
            {
                foodId = int.Parse(fields[0]),
                nutrientId = ushort.Parse(fields[1]),
                nutrientAmount = float.Parse(fields[2]),
            };
        }
    }
}
