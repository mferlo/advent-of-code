// Transpilation of clojure solution

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent {
    class Day13 {

        static IList<int> Parse(string line) =>
            line.Split(new[] { ": " }, StringSplitOptions.None).Select(x => Int32.Parse(x)).ToList();

        static IDictionary<int, int> Scanners;
        static IDictionary<int, int> GetScanners() =>
            File.ReadAllLines("input").Select(Parse).ToDictionary(i => i[0], i => i[1]);

        static int TMax => Scanners.Keys.Max() + 1; // + 1 because Range() is exclusive

        static int ScanPos(int t, int scanRange) {
            var period = 2 * (scanRange - 1);
            var tMod = t % period;
            return (tMod < scanRange) ? tMod : period - tMod;
        }

        static bool IsCaught(int t, int scanRange) => ScanPos(t, scanRange) == 0;

        static int CalcSeverity(int t) =>
            Scanners.ContainsKey(t) && IsCaught(t, Scanners[t]) ? t * Scanners[t] : 0;

        static bool IsCaughtWithDelay(int initialDelay, int t) => Scanners.ContainsKey(t) && IsCaught(t + initialDelay, Scanners[t]);

        static bool SuccessfulWithDelay(int initialDelay) => Enumerable.Range(0, TMax).All(t => !IsCaughtWithDelay(initialDelay, t));

        static void Main() {
            Scanners = GetScanners();
            Console.WriteLine($"Part 1: {Enumerable.Range(0, TMax).Select(CalcSeverity).Sum()}");
            Console.WriteLine($"Part 2: {Enumerable.Range(0, Int32.MaxValue).First(delay => SuccessfulWithDelay(delay))}");
        }
    }
}
