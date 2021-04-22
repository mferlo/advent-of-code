using System;
using System.Collections.Generic;
using System.Linq;

namespace _16
{
    class Program
    {
        static List<int> basePattern = new List<int> { 0, 1, 0, -1 };

        static List<int> MakePattern(int index, int length)
        {
            var position = index + 1;
            
            IEnumerable<int> MakePatternImpl()
            {
                while (true)
                {
                    for (var i = 0; i < basePattern.Count; i++)
                    {
                        for (var repetitions = 0; repetitions < position; repetitions++)
                        {
                            yield return basePattern[i];
                        }
                    }
                }
            }

            return MakePatternImpl().Skip(1).Take(length).ToList();
        }

        static List<int> ApplyPattern(List<int> current)
        {
            IEnumerable<int> ApplyPatternImpl()
            {
                for (var i = 0; i < current.Count; i++)
                {
                    var pattern = MakePattern(i, current.Count);
                    yield return Math.Abs(current.Zip(pattern, (x, y) => x * y).Sum()) % 10;
                }
            }

            return ApplyPatternImpl().ToList();
        }

        static List<int> Iterate100Times(List<int> input)
        {
            for (var i = 0; i < 100; i++)
            {
                input = ApplyPattern(input);
            }
            return input;
        }

        static void Debug(List<int> x) => Console.WriteLine(String.Join("", x));

        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("input.txt").Trim().Select(ch => int.Parse(""+ch)).ToList();
            var part1 = Iterate100Times(input);
            Console.WriteLine(String.Join("", part1.Take(8)));

            
        }
    }
}
