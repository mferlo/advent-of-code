namespace Advent {
	class Node {
		public int Num;
		public Node Next;
	}

	class Problem {
		const int numElves = 3012210;

		static Node GetWinner(Node cur) {
			while (cur != cur.Next) {
				cur.Next = cur.Next.Next;
				cur = cur.Next;
			}
			return cur;
		}

		static void Main() {
			var head = new Node { Num = 1 };
			var cur = head;

			for (var n = 2; n <= numElves; n++) {
				cur = cur.Next = new Node { Num = n };
			}
			cur.Next = head;

			var winner = GetWinner(head);
			System.Console.WriteLine(winner.Num);
		}
	}
}
