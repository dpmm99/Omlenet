using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Omlenet.USDAFormat;

namespace Omlenet
{
    public class FoodDescription
    {
        public int id;
        public ushort foodGroupId;
        public string longDesc;
        public string shortDesc;
        public string commonName;

        public static FoodDescription FromStream(StreamReader sr)
        {
            var fields = parseLine(sr.ReadLine(), 5);
            if (fields.Count < 5) return null;
            return new FoodDescription
            {
                id = int.Parse(fields[0]),
                foodGroupId = ushort.Parse(fields[1]),
                longDesc = fields[2],
                shortDesc = fields[3],
                commonName = fields[4],
            };
        }

        public override string ToString()
        {
            return longDesc;
        }
    }
}
