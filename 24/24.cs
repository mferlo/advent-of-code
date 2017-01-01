using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent { class Problem24 {

    class Cell {
        public int X;
        public int Y;
        public char C;
        public int D;

        public bool IsInteresting => Char.IsDigit(C);
        public bool IsWall => C == '#';
        public bool Visited => D != -1;

        public override bool Equals(object o) => (o is Cell) && X == ((Cell)o).X && Y == ((Cell)o).Y;
        public override int GetHashCode() => X * 128 + Y;
        public override string ToString() => C.ToString();
    }

    class Grid {
        private Cell[,] cells;
        private Dictionary<char, Cell> interestingCells;
        public IEnumerable<char> Locations => interestingCells.Keys.OrderBy(c => c);
        public char Max => Locations.Last();

        public Grid(string[] lines) {
            var width = lines.First().Length;
            var height = lines.Length;

            this.cells = new Cell[width, height];
            this.interestingCells = new Dictionary<char, Cell>();

            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    var cell = new Cell { X = x, Y = y, C = lines[y][x], D = -1 };
                    cells[x, y] = cell;
                    if (cell.IsInteresting) {
                        interestingCells[cell.C] = cell;
                    }
                }
            }
        }

        public void Reset() {
            for (var y = 0; y < cells.GetLength(1); y++) {
                for (var x = 0; x < cells.GetLength(0); x++) {
                    cells[x, y].D = -1;
                }
            }
        }

        public Cell Find(char target) {
            return interestingCells[target];
        }

        public IEnumerable<Cell> AllNeighbors(Cell c) {
            if (c.X > 0) {
                yield return cells[c.X - 1, c.Y];
            }
            if (c.Y > 0) {
                yield return cells[c.X, c.Y - 1];
            }
            if (c.X < cells.GetLength(0) - 1) {
                yield return cells[c.X + 1, c.Y];
            }
            if (c.Y < cells.GetLength(1) - 1) {
                yield return cells[c.X, c.Y + 1];
            }
        }

        public IEnumerable<Cell> Neighbors(Cell c) {
            return AllNeighbors(c).Where(n => !n.IsWall);
        }
    }

    static bool FoundAll(char max, IEnumerable<char> found) {
        for (char c = '0'; c <= max; c++) {
            if (!found.Contains(c)) {
                return false;
            }
        }
        return true;
    }

    // BFS
    static Dictionary<char, int> GetDistancesFrom(char start, Grid grid) {
        grid.Reset();

        var result = new Dictionary<char, int>();
        var visited = new HashSet<Cell>();
        var queue = new Queue<Cell>();

        var first = grid.Find(start);
        first.D = 0;
        queue.Enqueue(first);

        while (!FoundAll(grid.Max, result.Keys)) {
            var c = queue.Dequeue();
            if (visited.Contains(c)) {
                continue;
            }
            visited.Add(c);

            if (c.IsInteresting) {
                result[c.C] = c.D;
            }

            foreach (var neighbor in grid.Neighbors(c).Where(n => !n.Visited)) {
                neighbor.D = c.D + 1;
                queue.Enqueue(neighbor);
            }
        }
        return result;
    }

    static Dictionary<char, Dictionary<char, int>> GetDistances(Grid grid) {
        var result = new Dictionary<char, Dictionary<char, int>>();
        foreach (var c in grid.Locations) {
            result[c] = GetDistancesFrom(c, grid);
        }
        return result;
    }

    static IEnumerable<IList<char>> Permute(IList<char> input) {
        if (input.Count == 1) {
            yield return input;
        } else {
            foreach (var e in input) {
                var temp = new List<char>(input);
                temp.Remove(e);
                
                foreach (var p in Permute(temp)) {
                    p.Insert(0, e);
                    yield return p;
                }
            }            
        }
    }

    static IEnumerable<IEnumerable<char>> AllPaths(IEnumerable<char> locations) {
        var result = locations.OrderBy(x => x).ToList();
        result.Remove('0');

        var zero = new List<char> { '0' };

        foreach (var permutation in Permute(result)) {
            // Part 1
            // yield return zero.Concat(permutation);

            // Part 2
            yield return zero.Concat(permutation).Concat(zero);
        }
    }

    static int Walk(Dictionary<char, Dictionary<char, int>> distances, IEnumerable<char> path) {
        var cur = '0';
        var sum = 0;
        foreach (var p in path) {
            sum += distances[cur][p];
            cur = p;
        }
        return sum;
    }

    static int GetShortestPath(Dictionary<char, Dictionary<char, int>> distances) {
        var shortest = Int32.MaxValue;
        foreach (var path in AllPaths(distances.Keys)) {
            var distance = Walk(distances, path);
            if (distance < shortest) {
                shortest = distance;
            }
        }
        return shortest;
    }

    static void Main() {
        var grid = new Grid(File.ReadAllLines("input.txt"));
        var distances = GetDistances(grid);
        Console.WriteLine(GetShortestPath(distances));
    }
}}