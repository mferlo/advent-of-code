using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent {

	// You know what? To hell with an elegant solution.
	// Let's just take the dumb, obvious approach.
	// Surely that won't come back to haunt us in part 2.
	// (Update: It didn't! Hooray!)

	// Didn't bother to remove a bunch of debugging stuff,
	// nor cleanup in general.

    public class Node {
        public int X;
        public int Y;
        public bool Wall;
        public int Distance;
        public bool Visited;
        public string Ch { get {
            if (Wall) {
				return "#";
			} else if (!Visited) {
				return " ";
			} else if (Distance <= 40) {
				return ".";
			} else {
				return (Distance % 10).ToString();
			}
		}}
        public string Debug => String.Join(", ", 
            new object[] { X, Y, Ch, Distance, Visited });
        public int DistanceTo(int x, int y) =>
            Math.Abs(X - x) + Math.Abs(Y - y);
    }

    public class Closest : IComparer<Node> {
        public int X;
        public int Y;

        public int Compare(Node a, Node b) =>
            a.DistanceTo(X, Y).CompareTo(b.DistanceTo(X, Y));
    }

    static class Problem13 {
        //
        // We're looking for the shortest path from _(1, 1) to
        //   test:   7,   4   (Answer: 11)
        // part 1:  31,  39   (Answer: ???)
        // part 2: ???, ???   (Answer: ?????)
        const int TargetX = 31; //7;
        const int TargetY = 39; //4;
        const int size = 45;

        // true means wall, x++ is right, y++ is down. avoid negative.
        // the grid goes to -1 just to avoid dealing with that
        // Deliberately making this a class so I can edit it
        static readonly Node Wall = new Node { Wall = true };
        static readonly Node[,] M = new Node[size, size];

        static Node Map(int x, int y) {
            return (x < 0 | y < 0)
                ? Wall
                : M[x,y];
        }

		static Node Init() {
            for (var x = 0; x < size; x++) {
                for (var y = 0; y < size; y++) {
                    M[x,y] = new Node {
                        X = x,
                        Y = y,
                        Wall = IsWall(x, y),
                        Distance = Int32.MaxValue,
                        Visited = false
                    };
                }
            }

            var start = Map(1, 1);
            start.Distance = 0;
            start.Visited = true;
			return start;
		}

        const int favoriteNumber = 1350;  // real input
        // const int favoriteNumber = 10; // test input
        static bool IsWall(int x, int y) {
            if (x < 0 || y < 0) {
                return true;
            }

            int n = x*x + 3*x + 2*x*y + y + y*y + favoriteNumber;
            int sum = 0;
            while (n > 0) {
                sum += n & 1;
                n = n >> 1;
            }

            return sum % 2 == 1;
        }

        static void PrintMap(Node cur = null) {
            var c = cur ?? new Node { X = -1, Y = -1 };
            // y is columns
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    if (x == TargetX && y == TargetY) {
                        Console.Write("%");
                    } else if (x == c.X && y == c.Y) {
                        Console.Write("*");
                    } else {
                        Console.Write(Map(x,y).Ch);
                    }
                }
                Console.WriteLine();
            }
        }

        static void MaybePushNeighbor(Stack<Node> s, int x, int y) {
            var n = Map(x, y);
            if (x >= 0 & y >= 0 & !n.Wall & !n.Visited) {
            }
            if (!n.Visited) {
                s.Push(n);
            }
        }

        static bool CanMove(Node from, Node to) {
            return !to.Wall & !to.Visited & from.Distance < to.Distance;
        }

        static IEnumerable<Node> ValidNeighbors(Node c) {
            var neighbors = new List<Node> {
                Map(c.X, c.Y-1),
                Map(c.X-1, c.Y),
                Map(c.X, c.Y+1),
                Map(c.X+1, c.Y)
            };

            return neighbors.Where(n => CanMove(c, n));
        }

        // Shortest path to (TargetX, TargetY)
        static Node Part1DFS(Node start) {
            var closest = new Closest { X = TargetX, Y = TargetY };
            var nodes = new List<Node> { start };

            while (nodes.Any()) {
                var cur = nodes[0];
                nodes.RemoveAt(0);

                if (cur.X == TargetX & cur.Y == TargetY) {
                    return cur;
                }

                foreach (var validNeighbor in ValidNeighbors(cur)) {
                    validNeighbor.Visited = true;
                    validNeighbor.Distance = cur.Distance + 1;
                    nodes.Add(validNeighbor);
                }

                nodes.Sort(closest);
            }

            throw new Exception();
        }

        // How many locations can you reach in <= 50 steps?
        static int Part2BFS(Node start) {
            var q = new Queue<Node>();
            q.Enqueue(start);

            while (q.Any()) {
                var cur = q.Dequeue();

				Console.WriteLine(cur.Debug);
				Console.WriteLine();
				PrintMap();
				Console.Read();

                if (cur.Distance >= 50) {
                    continue;
                }
                foreach (var validNeighbor in ValidNeighbors(cur)) {
                    validNeighbor.Visited = true;
                    validNeighbor.Distance = cur.Distance + 1;
					q.Enqueue(validNeighbor);
                }
            }

            var visited = 0;
            for (var x = 0; x < size; x++) {
                for (var y = 0; y < size; y++) {
                    if (Map(x, y).Visited) {
                        visited++;
                    }
                }
            }
            return visited;
        }


        public static void Main() {
            Console.WriteLine(Part1DFS(Init()).Debug);
            Console.WriteLine("Number visited was " + Part2BFS(Init()));
        }
    }
}
