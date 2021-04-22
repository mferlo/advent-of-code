using System;
using System.Linq;

namespace _22
{
    readonly struct Region {
        // private static int Depth = 510;
        private static int Depth = 11394;

        public int ErosionLevel { get; }
        public int Risk { get; }
        
        public Region(int geoIndex) {
            ErosionLevel = (geoIndex + Depth) % 20183;
            Risk = ErosionLevel % 3;
        }
    }

    class Program
    {
        static int Part1((int x, int y) target) {
            var grid = new Region[target.x + 1, target.y + 1];
            for (int x = 0; x <= target.x; x++) {
                grid[x, 0] = new Region(x * 16807);
            }
            for (int y = 0; y <= target.y; y++) {
                grid[0, y] = new Region(y * 48271);
            }

            for (int x = 1; x <= target.x; x++) {
                for (int y = 1; y <= target.y; y++) {
                    var geoIndex = grid[x-1, y].ErosionLevel * grid[x, y-1].ErosionLevel;
                    grid[x, y] = new Region(geoIndex);
                }
            }
            grid[target.x, target.y] = new Region(0);

            return grid.Cast<Region>().Sum(r => r.Risk);
        }

        static void Main(string[] args)
        {
            // Console.WriteLine(Part1((10, 10)));
            var part1 = Part1((7, 701));
            if (part1 != 5637) {
                throw new Exception();
            }
        }
    }
}
