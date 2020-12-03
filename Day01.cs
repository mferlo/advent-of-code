using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2020
{
    public class Day01
    {
        // Find the *TWO* entries that sum to 2020; what do you get if you multiply them together?
        public static object Part1()
        {
            var input = Common.Ints("01.txt").Select(ints => ints[0]).ToList();

            var found = new HashSet<int>();

            foreach (var x in input)
            {
                var target = 2020 - x;
                if (found.Contains(target))
                {
                    return x * target;
                }
                else
                {
                    found.Add(x);
                }
            }

            return "Not found";
        }

        // Find the *THREE* entries that sum to 2020; what do you get if you multiply them together?
        public static object Part2()
        {
            var input = Common.Ints("01.txt").Select(ints => ints[0]).ToList();

            // Nested loops go brrrrr
            foreach (var x in input)
            {
                foreach (var y in input.Where(i => i != x))
                {
                    foreach (var z in input.Where(i => i != x && i != y))
                    {
                        if (x + y + z == 2020)
                        {
                            return (long)x * y * z;
                        }
                    }

                }
            }

            return "Not found";
        }
    }
}