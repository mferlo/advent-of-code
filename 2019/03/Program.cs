using System;
using System.Collections.Generic;
using System.Linq;

namespace _03
{
    class Program
    {
        static (int X, int Y, int Delay) Move((int X, int Y, int Delay) cur, char dir)
        {
            switch (dir)
            {
                case 'D': return (cur.X, cur.Y + 1, cur.Delay + 1);
                case 'U': return (cur.X, cur.Y - 1, cur.Delay + 1);
                case 'R': return (cur.X + 1, cur.Y, cur.Delay + 1);
                case 'L': return (cur.X - 1, cur.Y, cur.Delay + 1);
                default: throw new Exception();
            }
        }

        static IEnumerable<(int X, int Y, int Delay)> ParseLine(string input)
        {
            var current = (X: 0, Y: 0, Delay: 0);

            foreach (var segment in input.Split(','))
            {
                var direction = segment[0];
                var length = int.Parse(segment.Substring(1));

                for (int i = 0; i < length; i++)
                {
                    current = Move(current, direction);
                    yield return current;
                }
            }
        }

        static int ManhattanDistance((int X, int Y) p) => Math.Abs(p.X) + Math.Abs(p.Y);

        static void Main(string[] args)
        {
            // var input = new[] { "R8,U5,L5,D3", "U7,R6,D4,L4" };
            var input = System.IO.File.ReadAllLines("input");

            var wire1 = ParseLine(input[0]).GroupBy(p => (p.X, p.Y)).ToDictionary(p => p.Key, p => p.First().Delay);
            var wire2 = ParseLine(input[1]).GroupBy(p => (p.X, p.Y)).ToDictionary(p => p.Key, p => p.First().Delay);

            var intersections = wire1.Keys.ToHashSet().Intersect(wire2.Keys.ToHashSet());

            var part1 = intersections.Min(ManhattanDistance);
            var part2 = intersections.Min(i => wire1[i] + wire2[i]);

            Console.WriteLine(part1);
            Console.WriteLine(part2);
        }
    }
}
