using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent {
    struct Range {
        public long Start;
        public long End;
    }

    class Problem20 {
        static string[] test = new[] { "0-2", "4-7", "5-8" };

        static Range FromString(string s) {
            var parts = s.Split(new [] { '-' });
            return new Range {
                Start = Int64.Parse(parts[0]),
                End = Int64.Parse(parts[1])
            };
        }

        static long Problem1(IList<Range> ranges) {
            long answer = 0;
            foreach (var range in ranges) {
                if (answer < range.Start) {
                    return answer;
                }
                answer = Math.Max(answer, range.End + 1);
            }
            throw new Exception("Didn't find answer");
        }

        static long Problem2(IList<Range> ranges) {
            long numValid = 0;
            long current = 0;
            foreach (var range in ranges) {
                if (current < range.Start) {
                    numValid += (range.Start - current);
                }
                current = Math.Max(current, range.End + 1);
            }

            if (current < UInt32.MaxValue) {
                numValid += UInt32.MaxValue - current;
            }

            return numValid;
        }

        static void Main() {
            var lines = System.IO.File.ReadAllLines("sorted.txt"); // Pre-sorted input
            var ranges = lines.Select(FromString).ToList();

            Console.WriteLine(Problem1(ranges));
            Console.WriteLine(Problem2(ranges));
        }
    }
}
