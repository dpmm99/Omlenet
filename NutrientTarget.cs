using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Omlenet.USDAFormat;

namespace Omlenet
{
    public class NutrientTarget
    {
        public ushort nutrientId;
        public ushort bodyType; //0 man, 1 woman, 2 pregnant, 3 child?
        public float min, target, max; //TODO: Collect data and record sources in a separate file
        public float costUnder, costOver; //Cost per unit under target and cost per unit over target

        public static NutrientTarget FromStream(StreamReader sr)
        {
            var fields = parseLine(sr.ReadLine(), 7);
            if (fields.Count < 7) return null;
            return new NutrientTarget
            {
                nutrientId = ushort.Parse(fields[0]),
                bodyType = ushort.Parse(fields[1]),
                min = float.Parse(fields[2]),
                target = float.Parse(fields[3]),
                max = float.Parse(fields[4]),
                costUnder = float.Parse(fields[5]),
                costOver = float.Parse(fields[6]),
            };
        }

        public NutrientTarget Clone()
        {
            return new NutrientTarget
            {
                nutrientId = nutrientId,
                bodyType = bodyType,
                min = min,
                target = target,
                max = max,
                costUnder = costUnder,
                costOver = costOver
            };
        }
    }
}
