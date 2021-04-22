using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day10
    {
        static List<int> Input;

        public static void Parse() =>
            Input = File.ReadAllLines("10.txt").Select(int.Parse).OrderBy(x => x).ToList();

        public static object Part1()
        {
            var steps = new int[4];
            var cur = 0;
            foreach (var i in Input)
            {
                steps[i - cur]++;
                cur = i;
            }
            steps[3]++; // Device is always 3 higher than max
            return steps[1] * steps[3];
        }

        /*
        Dynamic programming is probably simpler code-wise, but what about mathematically?

        From Part 1, we observe that there are only gaps of either 1 or 3
        Consider an arbitrary [x-3, x..y, y+3] range where [x..y] is consecutive. If the length of x..y is:
            2, there is 1 possibility
            3, there are 2 possibilities (either we use the number between them, or we don't)
            4, there are 4 possibilities (2 numbers between them: use neither; use either 1; use both)
            5, there are 7 possibilities (we must use at least 1 to bridge the gap -- 3 choose 1 + 3 choose 2 + 3 choose 3)
            N > 5, we can recurse with (N-1) + (N-2) + (N-3)
        */
        // We could memoize this, but in practice things are pretty small
        static long OptionCount(int len)
        {
             switch (len)
             {
                 case 1: return 1; // Degenerate case where x == y
                 case 2: return 1;
                 case 3: return 2;
                 case 4: return 4;
                 case 5: return 7;
                 default: return OptionCount(len - 3) + OptionCount(len - 2) + OptionCount(len - 1);
             }
        }

        static IEnumerable<int> ConsecutiveLengths(List<int> data)
        {
            var cur = 0;
            var consecutiveLength = 1;
            foreach (var i in data)
            {
                if (cur + 3 == i)
                {
                    yield return consecutiveLength;
                    consecutiveLength = 1;
                }
                else
                {
                    consecutiveLength++;
                }
                cur = i;
            }
            yield return consecutiveLength;
        }

        static long Part2(List<int> data) =>
            ConsecutiveLengths(data).Select(OptionCount).Aggregate(1L, (acc, cur) => acc * cur);

        public static void Test() =>
            Console.WriteLine(Part2(File.ReadAllLines("10.test.txt").Select(int.Parse).OrderBy(x => x).ToList()));

        public static object Part2() => Part2(Input);
    }
}