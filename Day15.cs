using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2020
{
    class Day15
    {
        static IList<int> Input;

        public static void Parse() => Input = Common.Ints("15.txt").First();

        public static void Test()
        {
            Console.WriteLine(Game(new[] { 0, 3, 6 }, 2020));
            Console.WriteLine(Game(new[] { 0, 3, 6 }, 30000000));
        }

        static int Game(IList<int> input, int lastTurnNumber)
        {
            var gameHistory = new Dictionary<int, List<int>>();
            var turn = 1;
            foreach (var i in input)
            {
                gameHistory[i] = new List<int> { turn++ };
            }

            var previous = input.Last();
            while (turn <= lastTurnNumber)
            {
                var history = gameHistory[previous];
                var toSay = history.Count == 1 ? 0 : history[history.Count - 1] - history[history.Count - 2];

                if (gameHistory.ContainsKey(toSay))
                {
                    gameHistory[toSay].Add(turn);
                }
                else
                {
                    gameHistory[toSay] = new List<int> { turn };
                }

                previous = toSay;
                turn++;
            }
            return previous;
        }

        public static object Part1() => Game(Input, 2020);

        // Shrug:
        // > Elapsed time: 00:00:07.5070061 
        // Probably a fancier way to do it, but 7 seconds isn't awful (and this impl is linear, so whatever)
        public static object Part2() => Game(Input, 30000000);
    }
}