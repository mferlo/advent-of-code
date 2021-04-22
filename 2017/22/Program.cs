using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace _22 {

    class Map {
        public enum D { Up, Right, Down, Left };

        private (int x, int y) cur;
        private D dir;
        private ImmutableHashSet<(int x, int y)> infected;
        private ImmutableHashSet<(int x, int y)> weakened;
        private ImmutableHashSet<(int x, int y)> flagged;

        public Map(IEnumerable<(int x, int y)> initialInfected) {
            cur = (0, 0);
            dir = D.Up;
            infected = ImmutableHashSet<(int x, int y)>.Empty
                .Union(initialInfected);
            weakened = ImmutableHashSet<(int x, int y)>.Empty;
            flagged = ImmutableHashSet<(int x, int y)>.Empty;
        }

        private bool IsInfected => infected.Contains(cur);
        private bool IsWeakened => weakened.Contains(cur);
        private bool IsFlagged => flagged.Contains(cur);

        private D Right() => (D)(((int)dir + 1) % 4);
        private D UTurn() => (D)(((int)dir + 2) % 4);
        private D Left()  => (D)(((int)dir + 3) % 4);

        private (int x, int y) Move() {
            switch (dir) {
            case D.Up: return (cur.x, cur.y + 1);
            case D.Down: return (cur.x, cur.y - 1);
            case D.Left: return (cur.x - 1, cur.y);
            case D.Right: return (cur.x + 1, cur.y);
            default: throw new Exception("WTF");
            }
        }

        public int Part1() {
            var numInfected = 0;
            for (var i = 0; i < 10000; i++) {
                if (IsInfected) {
                    dir = Right();
                    infected = infected.Remove(cur);
                } else {
                    dir = Left();
                    infected = infected.Add(cur);
                    numInfected += 1;
                }
                cur = Move();
            }
            return numInfected;
        }

        public int Part2() {
            var numInfected = 0;
            for (var i = 0; i < 10000000; i++) {
                if (IsWeakened) {
                    weakened = weakened.Remove(cur);
                    infected = infected.Add(cur);
                    numInfected += 1;
                } else if (IsInfected) {
                    dir = Right();
                    infected = infected.Remove(cur);
                    flagged = flagged.Add(cur);
                } else if (IsFlagged) {
                    dir = UTurn();
                    flagged = flagged.Remove(cur);
                } else {
                    dir = Left();
                    weakened = weakened.Add(cur);
                }
                cur = Move();
            }
            return numInfected;
        }
    }

    class Program {
        static IEnumerable<(int x, int y)> Parse(IEnumerable<string> lines) {
            var n = lines.First().Length;
            var max = n / 2;

            var y = max;
            foreach (var line in lines) {
                for (var x = 0; x < n; x++) {
                    if (line[x] == '#') {
                        yield return (x - max, y);
                    }
                }
                y--;
            }                     
        }

        static void Test() {
            var input = Parse(new[] { "..#", "#..", "..." });
            Console.WriteLine("Expected: 5587");
            Console.WriteLine("Actual:   " + new Map(input).Part1());

            Console.WriteLine("Expected: 2511944");
            Console.WriteLine("Actual:   " + new Map(input).Part2());
        }

        static void Main(string[] args) {
            Test();

            var input = Parse(File.ReadLines("input"));
            Console.WriteLine("Part 1: " + new Map(input).Part1());
            Console.WriteLine("Part 1: " + new Map(input).Part2());
        }
    }
}
