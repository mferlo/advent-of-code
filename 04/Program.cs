using System;
using System.Linq;

namespace _04
{
    class Program
    {
        // Your scientists^W programmers were so preoccupied with whether or not they could,
        // they didn't stop to think if they _should_!
        static bool DecreasesRightToLeft(int x, int prev = 9) =>
            x == 0 || (x % 10 <= prev && DecreasesRightToLeft(x / 10, x % 10));

        static bool TwoAdjacent_Part1(int x) =>
            x != 0 && ((x > 10 && (x % 10 == (x/10) % 10)) || TwoAdjacent_Part1(x / 10));

        // Sadly, a Regex proved intractable after a few minutes of poking at it.
        // ...I'm not convinced this approach is any better!
        static bool TwoAdjacent_Part2(int x)
        {
            var s = $"!{x}!";
            return Enumerable.Range(1, 6).Any(i => s[i-1] != s[i] && s[i] == s[i+1] && s[i+1] != s[i+2]);
        }

        static bool Test1(int x) => TwoAdjacent_Part1(x) && DecreasesRightToLeft(x);
        static bool Test2(int x) => TwoAdjacent_Part2(x) && DecreasesRightToLeft(x);
        static void Debug(int x) => Console.WriteLine($"{x} {DecreasesRightToLeft(x)} {TwoAdjacent_Part1(x)} {TwoAdjacent_Part2(x)}");

        static void Main(string[] args)
        {
            var start = 248345;
            var end = 746315;

            Console.WriteLine(Enumerable.Range(start, end - start + 1).Count(Test1));
            Console.WriteLine(Enumerable.Range(start, end - start + 1).Count(Test2));
        }
    }
}
