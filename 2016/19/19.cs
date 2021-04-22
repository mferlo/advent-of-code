namespace Advent {
    class Node {
        public int Num;
        public Node Next;
        public Node Prev; // Used in part 2 only
    }

    class Problem {
        const int numElves = 3012210;

        static Node GetWinnerPart1(Node cur) {
            while (cur != cur.Next) {
                cur.Next = cur.Next.Next;
                cur = cur.Next;
            }
            return cur;
        }

        static void Print(Node start) {
            var n = start;
            do {
                System.Console.Write(n.Num + " ");
                n = n.Next;
            } while (n != start);
        }

        static Node GetWinnerPart2(Node cur, Node target) {
            var isEven = numElves % 2 == 0;
            while (cur != cur.Next) {
                var p = target.Prev;
                var n = target.Next;
                p.Next = n;
                n.Prev = p;

                cur = cur.Next;
                target = isEven ? n : n.Next;
                isEven = !isEven;
            }
            return cur;
        }

        static void Main() {
            var head = new Node { Num = 1 };
            var cur = head;

            int firstTarget = (numElves / 2) + 1;
            Node target = null;

            for (var n = 2; n <= numElves; n++) {
                cur = cur.Next = new Node { Num = n, Prev = cur };
                if (n == firstTarget) {
                    target = cur;
                }
            }
            cur.Next = head;
            head.Prev = cur;

            // These destroy the list, so only run one.
            // System.Console.WriteLine(GetWinnerPart1(head).Num);
            System.Console.WriteLine(GetWinnerPart2(head, target).Num);
        }
    }
}
