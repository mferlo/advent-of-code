using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class ConwayCube
    {
        Dictionary<(int X, int Y, int Z, int W), bool> points;
        bool Is4D { get; }

        public ConwayCube(IList<string> lines, bool is4D)
        {
            Is4D = is4D;
            points = new Dictionary<(int X, int Y, int Z, int W), bool>();

            for (int y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    points[(x, y, 0, 0)] = line[x] == '#';
                }
            }
        }

        public int ActiveCount() => points.Values.Count(v => v);

        ConwayCube(Dictionary<(int X, int Y, int Z, int W), bool> points, bool is4D) =>
            (this.points, Is4D) = (points, is4D);

        bool Value((int X, int Y, int Z, int W) point) =>
            points.ContainsKey(point) ? points[point] : false;

        IEnumerable<(int X, int Y, int Z, int W)> Neighbors((int X, int Y, int Z, int W) p)
        {
            for (var x = p.X - 1; x <= p.X + 1; x++)
            {
                for (var y = p.Y - 1; y <= p.Y + 1; y++)
                {
                    for (var z = p.Z - 1; z <= p.Z + 1; z++)
                    {
                        if (Is4D)
                        {
                            for (var w = p.W - 1; w <= p.W + 1; w++)
                            {
                                var n = (x, y, z, w);
                                if (n == p) continue;
                                yield return n;
                            }
                        }
                        else
                        {
                            var n = (x, y, z, 0);
                            if (n == p) continue;
                            yield return n;
                        }
                    }
                }
            }
        }

        int NeighborCount((int X, int Y, int Z, int W) point) =>
            Neighbors(point).Count(n => Value(n));

        public ConwayCube Step()
        {
            var result = new Dictionary<(int X, int Y, int Z, int W), bool>();

            var allPointsToConsider = points.Keys.SelectMany(p => Neighbors(p)).Distinct();
            foreach (var point in allPointsToConsider)
            {
                var count = NeighborCount(point);
                result[point] = count == 3 || (count == 2 && Value(point));
            }
            return new ConwayCube(result, Is4D);
        }
    }

    class Day17
    {
        static IList<string> Input;

        public static void Parse() => Input = File.ReadAllLines("17.txt");

        public static void Test()
        {
            Console.WriteLine(Step6(".#.\n..#\n###".Split("\n"), false));
            Console.WriteLine(Step6(".#.\n..#\n###".Split("\n"), true));
        }

        static int Step6(IList<string> data, bool is4D)
        {
            var cube = new ConwayCube(data, is4D);
            for (var x = 1; x <= 6; x++)
            {
                cube = cube.Step();
            }
            return cube.ActiveCount();
        }

        public static object Part1() => Step6(Input, false);
        public static object Part2() => Step6(Input, true);
    }
}