using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Omlenet.USDAFormat;

namespace Omlenet
{
    public class Nutrient
    {
        public ushort id;
        public string unitOfMeasure;
        public string name;
        //TODO: Need safe minimum, safe maximum, and target for each

        public static Nutrient FromStream(StreamReader sr)
        {
            var fields = parseLine(sr.ReadLine(), 4);
            if (fields.Count < 4) return null;
            return new Nutrient
            {
                id = ushort.Parse(fields[0]),
                unitOfMeasure = fields[1], //May be g, mg, μg
                name = fields[3],
            };
        }
    }
}
