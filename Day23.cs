using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Node
    {
        public Node Next;
        public int Value;

        public Node(int value) => Value = value;
        public override string ToString() => Value.ToString();
    }

    class CircularList
    {
        public Node Head;
        public int Max;

        private Dictionary<int, Node> Lookup;

        public CircularList(IEnumerable<int> collection)
        {
            Debug.Assert(collection.Any());

            Lookup = new Dictionary<int, Node>();
            Head = new Node(collection.First());
            var cur = Head;
            Lookup[cur.Value] = cur;
            foreach (var item in collection.Skip(1))
            {
                cur.Next = new Node(item);
                cur = cur.Next;
                Lookup[cur.Value] = cur;
            }
            cur.Next = Head;

            Max = this.AsEnumerable(Head).Max();
        }

        public Node RemoveThreeAfter(Node node)
        {
            Head = node;
            var result = node.Next;
            node.Next = node.Next.Next.Next.Next;
            return result;
        }

        public void InsertThreeAfter(Node node, Node threeToInsert)
        {
            threeToInsert.Next.Next.Next = node.Next;
            node.Next = threeToInsert;
        }

        public Node Find(int value) => Lookup.ContainsKey(value) ? Lookup[value] : null;

        public IEnumerable<int> AsEnumerable(Node start)
        {
            var cur = start;
            yield return cur.Value;
            cur = cur.Next;
            while (cur != start)
            {
                yield return cur.Value;
                cur = cur.Next;
            }
        }

        public override string ToString() => string.Join(", ", this.AsEnumerable(Head));
    }

    class Day23
    {
        static IEnumerable<int> Input;

        static IEnumerable<int> FromString(string s) => s.Select(ch => int.Parse(ch.ToString()));
        public static void Parse() => Input = FromString(File.ReadAllText("23.txt"));

        static Node Move(CircularList state, Node current)
        {
            var removedThree = state.RemoveThreeAfter(current);
            var removedValues = new HashSet<int>
            {
                removedThree.Value,
                removedThree.Next.Value,
                removedThree.Next.Next.Value
            };

            var destinationValue = current.Value - 1;
            while (destinationValue == 0 || removedValues.Contains(destinationValue))
            {
                if (destinationValue == 0)
                {
                    destinationValue = state.Max;
                }
                else
                {
                    destinationValue--;
                }
            }
            state.InsertThreeAfter(state.Find(destinationValue), removedThree);
            return current.Next;
        }

        static CircularList Game(IEnumerable<int> initialState, int iterations)
        {
            var state = new CircularList(initialState);
            var current = state.Head;

            for (var i = 1; i <= iterations; i++)
            {
                current = Move(state, current);
            }
            return state;
        }

        static string Part1(IEnumerable<int> initialState)
        {
            var result = Game(initialState, 100);
            var one = result.AsEnumerable(result.Find(1));
            return string.Join("", one.Skip(1));
        }

        public static void Test() => Debug.Assert("67384529" == Part1(FromString("389125467")));
        public static object Part1() => Part1(Input);

        public static object Part2()
        {
            var part2Input = Input.Concat(
                Enumerable.Range(start: 1, count: 1_000_000).Skip(Input.Count())
            );
            var result = Game(part2Input, 10_000_000);
            var one = result.Find(1);
            return (long)one.Next.Value * (long)one.Next.Next.Value;
        }
    }
}