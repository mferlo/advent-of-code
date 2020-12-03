using System.Collections.Generic;
using System.Linq;

namespace Advent2020
{
    public class Day01
    {
        static List<int> Input;

        public static void Parse() =>
            Input = Common.Ints("01.txt").Select(ints => ints[0]).ToList();

        // Find the *TWO* entries that sum to 2020; what do you get if you multiply them together?
        public static object Part1()
        {
            var found = new HashSet<int>();

            foreach (var x in Input)
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
            // Nested loops go brrrrr
            foreach (var x in Input)
            {
                foreach (var y in Input.Where(i => i != x))
                {
                    foreach (var z in Input.Where(i => i != x && i != y))
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