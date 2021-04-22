using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace _25
{
    public class Star {
        public ImmutableList<int> Coords { get; }
        public HashSet<Star> Neighbors { get; }

        public Star(IEnumerable<int> coords) {
            if (coords.Count() != 4) { throw new Exception("Invalid coordinates"); }

            this.Coords = ImmutableList<int>.Empty.AddRange(coords);
            this.Neighbors = new HashSet<Star>();
        }

        public override string ToString() => string.Join(", ", Coords);

        private int D(int x, int y) => Math.Abs(x - y);

        public int Distance(Star other) =>
            D(Coords[0], other.Coords[0]) +
            D(Coords[1], other.Coords[1]) +
            D(Coords[2], other.Coords[2]) +
            D(Coords[3], other.Coords[3]);
    }

    class Program
    {
        static Star ParseLine(string line) => new Star(line.Split(',').Select(int.Parse));

        static IEnumerable<Star> GetStars(string fileName) => 
            File.ReadAllLines(fileName).Select(ParseLine);

        static IEnumerable<Star> AssignNeighbors(IEnumerable<Star> input) {
            var stars = input.ToList();
            foreach (var star in stars) {
                foreach (var other in stars.Where(s => s != star)) {
                    if (star.Distance(other) <= 3) {
                        star.Neighbors.Add(other);
                        other.Neighbors.Add(star);
                    }
                }
            }
            return stars;
        }

        static IEnumerable<IEnumerable<Star>> FindConstellations(IEnumerable<Star> input) {
            var toProcess = new HashSet<Star>(input);
            while (toProcess.Any()) {
                var star = toProcess.First();
                var constellation = new HashSet<Star>();
                constellation.Add(star);
                var neighbors = new Queue<Star>(star.Neighbors);

                while (neighbors.Any()) {
                    var neighbor = neighbors.Dequeue();
                    if (constellation.Add(neighbor)) {
                        foreach (var nn in neighbor.Neighbors.Where(nn => !constellation.Contains(nn))) {
                            neighbors.Enqueue(nn);
                        }
                    }
                }
                toProcess.ExceptWith(constellation);
                yield return constellation;
            }
        }

        static void Main(string[] args) {
            var input = GetStars("input");
            var stars = AssignNeighbors(input);
            var c = FindConstellations(stars);
            Console.WriteLine(c.Count());
        }
    }
}
