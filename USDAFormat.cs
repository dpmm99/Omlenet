using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omlenet
{
    public static class USDAFormat
    {
        public static List<string> parseLine(string line, int limit = 32)
        {
            //Carets between fields; ~ surrounding text fields
            var fields = new List<string>(limit);
            int lastStartIdx = 0;
            for (int x = 0; x <= line.Length && fields.Count < limit; x++)
            {
                if (x == line.Length || line[x] == '^')
                {
                    var toAdd = line.Substring(lastStartIdx, x - lastStartIdx);
                    if (toAdd.EndsWith("~")) fields.Add(toAdd.Substring(0, toAdd.Length - 1));
                    else fields.Add(toAdd);
                    lastStartIdx = x + 1;
                }
                else if (line[x] == '~' && x == lastStartIdx) //Expected only at the start or end of a text field
                {
                    lastStartIdx++;
                }
            }

            return fields;
        }

        public static List<T> loadAsList<T>(Func<StreamReader, T> readerMethod, string filename, ref int lineCount, int expectedLineCount, Action<int, int> reportProgress)
        {
            var list = new List<T>();
            var sr = new StreamReader(new BufferedStream(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read)), Encoding.Default);
            while (!sr.EndOfStream)
            {
                list.Add(readerMethod(sr));
                reportProgress(++lineCount, expectedLineCount);
            }
            return list;
        }
    }
}
