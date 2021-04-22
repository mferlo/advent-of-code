using System;
using System.Collections.Generic;
using System.Linq;

namespace _06
{
    class Program
    {
        struct Coordinate { 
            public int X;
            public int Y;
            public Coordinate(int x, int y) { X = x; Y = y; }
            public override string ToString() => $"({X}, {Y})";
        }

        class Cell {
            public Cell(int x, int y) {
                Coordinate = new Coordinate(x, y);
                Distance = int.MaxValue;
                Closest = new List<int>();
            }
            public IList<int> Closest;
            public int Distance;
            public Coordinate Coordinate;
            public int? Winner => Closest.Count == 1 ? (int?)Closest.First() : null;
            //public override string ToString() => $"[ {Coordinate} {Distance} -> {Winner} ]";
        }

        static IList<Coordinate> test = new List<Coordinate> {
            new Coordinate(1, 1),
            new Coordinate(1, 6),
            new Coordinate(8, 3),
            new Coordinate(3, 4),
            new Coordinate(5, 5),
            new Coordinate(8, 9),
        };

        static IEnumerable<Coordinate> ParseCoords() {
            foreach (var line in System.IO.File.ReadLines("input")) {
                var parts = line.Split(',');
                yield return new Coordinate(int.Parse(parts[0]), int.Parse(parts[1].Substring(1)));
            }
        }

        static void Initialize(Cell[,] cells) {
            for (var x = 0; x < cells.GetLength(0); x++) {
                for (var y = 0; y < cells.GetLength(1); y++) {
                    cells[x, y] = new Cell(x, y);
                }
            }
        }

        static int GetDistance(Coordinate c, int x, int y) =>
            Math.Abs(c.X - x) + Math.Abs(c.Y - y);

        static int GetDistance(Coordinate c1, Coordinate c2) =>
            Math.Abs(c1.X - c2.X) + Math.Abs(c1.Y - c2.Y);

        static void MarkTerritory(Coordinate source, int sourceName, Cell[,] cells) {
            for (var x = 0; x < cells.GetLength(0); x++) {
                for (var y = 0; y < cells.GetLength(1); y++) {
                    var c = cells[x, y];
                    var d = GetDistance(source, c.Coordinate);
                    if (d < c.Distance) {
                        c.Closest.Clear();
                        c.Closest.Add(sourceName);
                        c.Distance = d;
                    } else if (d == c.Distance) {
                        c.Closest.Add(sourceName);
                    }
                }
            }
        }

        static void DebugPrint(Cell[,] cells) {
            for (var x = 0; x < cells.GetLength(0); x++) {
                for (var y = 0; y < cells.GetLength(1); y++) {
                    Console.Write(cells[x, y]);
                }
                Console.WriteLine();
            }
        }

        static Cell[,] MakeCells(IList<Coordinate> points) {
            var result = new Cell[1 + points.Max(c => c.X), 1 + points.Max(c => c.Y)];
            Initialize(result);
            for (var i = 0; i < points.Count; i++) {
                MarkTerritory(points[i], i, result);
            }
            return result;
        }

        static int FindLargestFiniteArea(Cell[,] cells, IList<Coordinate> coords) {
            var infinite = new HashSet<int?>();
            var maxX = cells.GetLength(0) - 1;
            var maxY = cells.GetLength(1) - 1;
            for (var x = 0; x <= maxX; x++) {
                infinite.Add(cells[x, 0].Winner);
                infinite.Add(cells[x, maxY].Winner);
            }
            for (var y = 0; y <= maxY; y++) {
                infinite.Add(cells[0, y].Winner);
                infinite.Add(cells[maxX, y].Winner);
            }

            var areas = new Dictionary<int, int>();
            for (var x = 0; x < cells.GetLength(0); x++) {
                for (var y = 0; y < cells.GetLength(1); y++) {
                    var winner = cells[x, y].Winner;
                    if (!infinite.Contains(winner)) {
                        areas.TryAdd(winner.Value, 0);
                        areas[winner.Value] += 1;
                    }
                }
            }

            return areas.Values.Max();
        }

        static int FindClosestPointsArea(int maxX, int maxY, IList<Coordinate> coords) {
            var area = 0;
            for (var x = 0; x < maxX; x++) {
                for (var y = 0; y < maxY; y++) {
                    if (coords.Sum(c => GetDistance(c, x, y)) < 10_000) {
                        area += 1;
                    }
                }
            }
            return area;
        }

        static void Main(string[] args)
        {
            var coords = ParseCoords().ToList(); // test;
            var cells = MakeCells(coords);
            Console.WriteLine("Part 1: " + FindLargestFiniteArea(cells, coords));
            Console.WriteLine("Part 2: " + FindClosestPointsArea(cells.GetLength(0), cells.GetLength(1), coords));
        }
    }
}
