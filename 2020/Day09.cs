using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day09
    {
        static List<long> Input;

        public static void Parse() => Input = File.ReadAllLines("09.txt").Select(long.Parse).ToList();

        static bool IsValidSum(long target, HashSet<long> numbers)
        {
            foreach (var i in numbers)
            {
                // Assumption: no duplicates
                if (i + i == target)
                    continue;

                if (numbers.Contains(target - i))
                    return true;
            }
            return false;
        }

        static long part1Answer;

        public static object Part1()
        {
            var q = new Queue<long>(Input.Take(25));
            var numbers = new HashSet<long>(q);
            foreach (var i in Input.Skip(25))
            {
                if (!IsValidSum(i, numbers))
                {
                    part1Answer = i;
                    return i;
                }

                var removed = q.Dequeue();
                numbers.Remove(removed);

                q.Enqueue(i);
                if (!numbers.Add(i))
                {
                    throw new Exception("Need to change logic: there is a duplicate");
                }
            }
            return -1;
        }

        public static object Part2()
        {
            var start = 0;
            var end = 0;
            var sum = Input[0];

            while (true)
            {
                if (sum == part1Answer)
                {
                    break;
                }
                else if (sum < part1Answer)
                {
                    end++;
                    sum += Input[end];
                }
                else
                {
                    sum -= Input[start];
                    start++;
                }
            }

            var range = Input.GetRange(start, count: end - start + 1);
            return range.Min() + range.Max();
        }
    }
}