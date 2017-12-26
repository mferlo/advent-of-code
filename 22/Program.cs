using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace _22 {

    class Map {
        public enum D { Up, Right, Down, Left };

        private (int x, int y) cur;
        private D dir;
        private ImmutableHashSet<(int x, int y)> infected;
        private int numInfected;

        public Map(IEnumerable<(int x, int y)> initialInfected) {
            cur = (0, 0);
            dir = D.Up;
            infected = ImmutableHashSet<(int x, int y)>.Empty
                .Union(initialInfected);
            numInfected = 0;
        }

        public int Part1 => numInfected;            
        private bool IsInfected => infected.Contains(cur);

        private D Left() => (D)(((int)dir + 3) % 4);
        private D Right() => (D)(((int)dir + 1) % 4);

        private (int x, int y) Move() {
            switch (dir) {
            case D.Up: return (cur.x, cur.y + 1);
            case D.Down: return (cur.x, cur.y - 1);
            case D.Left: return (cur.x - 1, cur.y);
            case D.Right: return (cur.x + 1, cur.y);
            default: throw new Exception("WTF");
            }
        }

        public void Act() {
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
    }

    class Program {
        const string TestInputString =
@"..#
#..
...";

        static IEnumerable<(int x, int y)> Parse(string input) {
            var lines = input.Split(new [] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

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
            var map = new Map(Parse(TestInputString));
            var i = 0;
            while (true) {
                map.Act();
                i++;
                if (i == 7) {
                    Console.WriteLine("7: " + map.Part1);
                }

                if (i == 70) {
                    Console.WriteLine("70: " + map.Part1);
                }

                if (i == 10_000) {
                    Console.WriteLine("10000: " + map.Part1);
                    break;
                }
            }
        }

        static void Main(string[] args) {
            Test();
        }
    }
}
