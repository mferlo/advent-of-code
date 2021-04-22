using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class HexGridNavigator
    {
        private int x, y, z;

        public HexGridNavigator() => (x, y, z) = (0, 0, 0);

        public (int X, int Y, int Z) Position => (x, y, z);

        public void Move(string dir)
        {
            switch (dir)
            {
                case "e": x++; y--; break;
                case "w": x--; y++; break;

                case "ne": x++; z--; break;
                case "sw": x--; z++; break;

                case "nw": y++; z--; break;
                case "se": y--; z++; break;

                default: throw new Exception(dir);
            }
        }

        public static IEnumerable<(int X, int Y, int Z)> Neighbors((int X, int Y, int Z) p)
        {
            yield return (p.X + 1, p.Y - 1, p.Z);
            yield return (p.X - 1, p.Y + 1, p.Z);
            yield return (p.X + 1, p.Y, p.Z - 1);
            yield return (p.X - 1, p.Y, p.Z + 1);
            yield return (p.X, p.Y + 1, p.Z - 1);
            yield return (p.X, p.Y - 1, p.Z + 1);
        }
    }

    class Day24
    {
        static List<string> DirectionList;
        public static void Parse() => DirectionList = File.ReadAllLines("24.txt").ToList();

        static IEnumerable<string> ParseDirections(string directions)
        {
            var queue = new Queue<char>(directions);
            while (queue.Any())
            {
                var s = queue.Dequeue().ToString();
                if (s == "e" || s == "w")
                {
                    yield return s;
                }
                else
                {
                    yield return s + queue.Dequeue().ToString();
                }
            }
        }

        static (int X, int Y, int Z) FollowDirections(string directions)
        {
            var navigator = new HexGridNavigator();
            foreach (var dir in ParseDirections(directions))
            {
                navigator.Move(dir);
            }
            return navigator.Position;
        }

        static HashSet<(int X, int Y, int Z)> Part1BlackTiles;

        public static object Part1()
        {
            var isBlack = new Dictionary<(int X, int Y, int Z), bool>();
            foreach (var position in DirectionList.Select(FollowDirections))
            {
                isBlack[position] = isBlack.ContainsKey(position) ? !isBlack[position] : true;
            }
            Part1BlackTiles = isBlack.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToHashSet();
            return Part1BlackTiles.Count;
        }


        static HashSet<(int X, int Y, int Z)> Step(HashSet<(int X, int Y, int Z)> blackTiles)
        {
            var toConsider = blackTiles.Concat(blackTiles.SelectMany(s => HexGridNavigator.Neighbors(s))).Distinct();

            var result = new HashSet<(int X, int Y, int Z)>();
            foreach (var hex in toConsider)
            {
                var blackNeighbors = HexGridNavigator.Neighbors(hex).Count(neighbor => blackTiles.Contains(neighbor));
                if (blackNeighbors == 2 || (blackNeighbors == 1 && blackTiles.Contains(hex)))
                {
                    result.Add(hex);
                }
            }
            return result;
        }

        public static object Part2()
        {
            var blackTiles = Part1BlackTiles;
            for (var i = 1; i <= 100; i++)
            {
                blackTiles = Step(blackTiles);
            }
            return blackTiles.Count;
        }
    }
}