using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace _09
{
    public class LinkedListImpl : MarbleGame {
        LinkedList<int> marbles;
        LinkedListNode<int> current;

        public LinkedListImpl(int lastMarbleScore, int numPlayers)
            : base(lastMarbleScore, numPlayers)
        {
            marbles = new LinkedList<int>(new[] { 0 });
            current = marbles.First;
        }

        private LinkedListNode<int> Next() =>
            current = current == marbles.Last ? marbles.First : current.Next;
        
        private LinkedListNode<int> Back() =>
            current = current == marbles.First ? marbles.Last : current.Previous;

        protected override void NormalStep() =>
            current = marbles.AddAfter(Next(), currentMarbleScore);

        protected override int FancyStep() {
            for (var i = 1; i <= 7; i++) {
                Back();
            }
            var toRemove = current;
            var result = current.Value;
            Next();
            marbles.Remove(toRemove);
            return result;
        }
    }

    public class ArrayListImpl : MarbleGame {
        List<int> marbles;
        int i;

        public ArrayListImpl(int lastMarbleScore, int numPlayers)
            : base(lastMarbleScore, numPlayers)
        {
            marbles = new List<int>(lastMarbleScore) { 0 };
            i = 0;
        }

        protected override void NormalStep() {
            var next = (i + 2) % marbles.Count;
            marbles.Insert(next, currentMarbleScore);
            i = next;
        }

        protected override int FancyStep() {
            var toRemove = (marbles.Count + i - 7) % marbles.Count;
            i = toRemove;
            var result = marbles[toRemove];
            marbles.RemoveAt(toRemove);
            return result;
        }

    }

    public abstract class MarbleGame {
        int lastMarbleScore;

        int numPlayers;
        IList<long> playerScores;

        protected int currentPlayer = 0;
        protected int currentMarbleScore = 1;

        public MarbleGame(int lastMarbleScore, int numPlayers) {
            this.lastMarbleScore = lastMarbleScore;
            this.numPlayers = numPlayers;
            this.playerScores = new long[numPlayers];
        }

        public long Run() {
            while (currentMarbleScore <= this.lastMarbleScore) {
                Step();
            }
            return playerScores.Max();
        }

        protected abstract void NormalStep();
        protected abstract int FancyStep();

        void Step() {
            if (currentMarbleScore % 23 == 0) {
                playerScores[currentPlayer] += currentMarbleScore;
                playerScores[currentPlayer] += FancyStep();
            } else {
                NormalStep();
            }
            currentMarbleScore += 1;
            currentPlayer = (currentPlayer + 1) % numPlayers;
        }
    }

    class Program
    {
        static long SolveArrayList(int max, int players) =>
            new ArrayListImpl(max, players).Run();

        static long SolveLinkedList(int max, int players) =>
            new LinkedListImpl(max, players).Run();

        static TimeSpan Time(Action a) {
            var sw = new Stopwatch();
            sw.Start();
            a();
            sw.Stop();
            return sw.Elapsed;
        }

        static void Solve(int max, int players) {
            long linkedResult = 0;
            var linkedTime = Time(() => linkedResult = SolveLinkedList(max, players));
            Console.WriteLine($"Linked: {linkedResult} ({linkedTime})");

            long arrayResult = 0;
            var arrayTime = Time(() => arrayResult = SolveArrayList(max, players));
            Console.WriteLine($"Array:  {arrayResult} ({arrayTime})");

            if (linkedResult != arrayResult) {
                throw new Exception();
            }
        }

        static void Test() {
            Solve(25, 9);
            Solve(1618, 10);
            Solve(7999, 13);
            Solve(1104, 17);
            Solve(6111, 21);
            Solve(5807, 30);
        }

        static void Main(string[] args)
        {
            var max = int.Parse(args[0]);
            var players = int.Parse(args[1]);

            Test();
            Solve(max, players);
            Solve(100 * max, players);
        }
    }
}
