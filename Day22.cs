using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day22
    {
        static ImmutableQueue<int> Player1;
        static ImmutableQueue<int> Player2;

        static ImmutableQueue<int> MakeQueue(System.Collections.Generic.IEnumerable<int> e) =>
            e.Aggregate(ImmutableQueue<int>.Empty, (q, item) => q.Enqueue(item));

        public static void Parse()
        {
            var data = File.ReadAllLines("22.txt");
            Player1 = MakeQueue(data[1..26].Select(int.Parse));
            Player2 = MakeQueue(data[28..53].Select(int.Parse));
        }

        static int Score(IEnumerable<int> deck)
        {
            var multiplier = deck.Count();
            var total = 0;
            foreach (var c in deck)
            {
                total += c * multiplier;
                multiplier--;
            }
            return total;
        }

        public static object Part1()
        {
            var p1 = new Queue<int>(Player1);
            var p2 = new Queue<int>(Player2);
            while (p1.Any() && p2.Any())
            {
                var c1 = p1.Dequeue();
                var c2 = p2.Dequeue();
                if (c1 > c2)
                {
                    p1.Enqueue(c1);
                    p1.Enqueue(c2);
                }
                else
                {
                    p2.Enqueue(c2);
                    p2.Enqueue(c1);
                }
            }

            return Score(p1.Any() ? p1 : p2);
        }

        enum Player { Player1, Player2 }

        static string State(IEnumerable<int> p1, IEnumerable<int> p2) =>
            $"{string.Join(",", p1)} | {string.Join(",", p2)}";

        static (Player Winner, int Score) Game(Queue<int> p1, Queue<int> p2)
        {
            var history = new HashSet<string>();

            while (p1.Any() && p2.Any())
            {
                var state = State(p1, p2);
                if (!history.Add(state))
                {
                    return (Player.Player1, Score(p1));
                }

                var c1 = p1.Dequeue();
                var c2 = p2.Dequeue();
                Player winner;
                if (p1.Count() >= c1 && p2.Count() >= c2)
                {
                    winner = Game(new Queue<int>(p1.Take(c1)), new Queue<int>(p2.Take(c2))).Winner;
                }
                else
                {
                    winner = c1 > c2 ? Player.Player1 : Player.Player2;
                }

                if (winner == Player.Player1)
                {
                    p1.Enqueue(c1);
                    p1.Enqueue(c2);
                }
                else
                {
                    p2.Enqueue(c2);
                    p2.Enqueue(c1);
                }
            }

            return p1.Any() ? (Player.Player1, Score(p1)) : (Player.Player2, Score(p2));
        }

        public static object Part2() =>
            Game(new Queue<int>(Player1), new Queue<int>(Player2)).Score;
    }
}