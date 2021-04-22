using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace _24
{
    struct Grid
    {
        private bool[] Bugs;

        public Grid(string input)
        {
            Bugs = input.Where(ch => ch == '.' || ch == '#').Select(ch => ch == '#').ToArray();
            Debug.Assert(Bugs.Length == 25);
        }

        private Grid(IEnumerable<bool> bugs) => Bugs = bugs.ToArray();

        public override bool Equals(object obj)
        {
            if (!(obj is Grid))
            {
                return false;
            }

            return ((Grid)obj).Bugs.SequenceEqual(this.Bugs);
        }

        public override int GetHashCode() => BiodiversityRating.GetHashCode();

        public override string ToString() => String.Join("\n", Enumerable.Range(0, 5).Select(Line));
        private string Line(int i) => String.Join("", Bugs.Skip(5 * i).Take(5).Select(b => b ? '#' : '.'));

        private static long BugValue(bool bug, int i) => bug ? ((long)Math.Pow(2, i)) : 0L;
        public long BiodiversityRating => Bugs.Select(BugValue).Sum();

        private int Left(int i) => (i % 5 == 0) ? 0 : Bugs[i - 1] ? 1 : 0;
        private int Right(int i) => (i % 5 == 4) ? 0 : Bugs[i + 1] ? 1 : 0;
        private int Up(int i) => i <= 4 ? 0 : Bugs[i - 5] ? 1 : 0;
        private int Down(int i) => i >= 20 ? 0 : Bugs[i + 5] ? 1 : 0;

        private int AdjacentBugs(int i) => Left(i) + Right(i) + Up(i) + Down(i);

        private bool NextValue(int i)
        {
            var adjacentBugs = AdjacentBugs(i);
            if (Bugs[i])
            {
                // A bug dies (becoming an empty space) unless there is exactly one bug adjacent to it.
                return adjacentBugs == 1;
            }
            else
            {
                // An empty space becomes infested with a bug if exactly one or two bugs are adjacent to it.
                return adjacentBugs == 1 || adjacentBugs == 2;
            }
        }

        public Grid Iterate() => new Grid(Enumerable.Range(0, 25).Select(NextValue));
    }

    class Program
    {
        static string Test = @"....#
#..#.
#..##
..#..
#....";

        static void Main(string[] args)
        {
            var grids = new HashSet<Grid>();
            var grid = new Grid(System.IO.File.ReadAllText("input.txt"));

            while (grids.Add(grid))
            {
                grid = grid.Iterate();
            }

            Console.WriteLine(grid.BiodiversityRating);
        }
    }
}
