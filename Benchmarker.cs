using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Omlenet
{
    public static class Benchmarker
    {
        private static Stopwatch sw;
        private static Dictionary<string, BenchData> benchmarkData;

        private class BenchData
        {
            public int CallCount;
            public long CallTime;

            public override string ToString()
            {
                return CallCount + "\t" + CallTime;
            }
        }

        public static void Benchmark()
        {
            if (sw == null)
            {
                benchmarkData = new Dictionary<string, BenchData>();
                sw = new Stopwatch();
                sw.Start();
                return;
            }
            sw.Stop();

            var stackTrace = new StackTrace(true).GetFrame(1);
            var key = stackTrace.GetFileName() + "\t" + stackTrace.GetFileLineNumber();
            if (benchmarkData.ContainsKey(key))
            {
                benchmarkData[key].CallCount++;
                benchmarkData[key].CallTime += sw.ElapsedTicks;
            }
            else benchmarkData.Add(key, new BenchData { CallCount = 1, CallTime = sw.ElapsedTicks });

            sw.Reset();
            sw.Start();
        }

        public static void CompleteBenchmark()
        {
            System.IO.File.WriteAllText("benchmark.txt", string.Join(Environment.NewLine, benchmarkData.Select(p => p.Key + "\t" + p.Value).OrderBy(p => p)));
            Process.Start("notepad.exe", "benchmark.txt");
            benchmarkData = null;
            sw = null;
        }
    }
}
