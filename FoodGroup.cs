using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Omlenet.USDAFormat;

namespace Omlenet
{
    public class FoodGroup
    {
        public ushort id;
        public string name;

        public static FoodGroup FromStream(StreamReader sr)
        {
            var fields = parseLine(sr.ReadLine(), 2);
            if (fields.Count < 2) return null;
            return new FoodGroup
            {
                id = ushort.Parse(fields[0]),
                name = fields[1],
            };
        }

        public override string ToString()
        {
            return name;
        }
    }
}
