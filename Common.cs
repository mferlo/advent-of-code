using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    public static class Common
    {
        public static IList<IList<int>> Ints(string input) =>
            File.ReadAllLines(input).Select(IntLine).ToList();


        static Regex IntRegex = new Regex(@"-?\d+");
        static IList<int> IntLine(string line) =>
            IntRegex.Matches(line).Select(match => int.Parse(match.Value)).ToList();
        
        public static long Product(this IEnumerable<int> numbers) => numbers.Product(x => (long)x);
        public static long Product(this IEnumerable<long> numbers) => numbers.Product(x => x);
        public static long Product<T>(this IEnumerable<T> numbers, Func<T, int> projection) =>
            Product(numbers, item => (long)projection(item));
        public static long Product<T>(this IEnumerable<T> numbers, Func<T, long> projection) =>
            numbers.Aggregate(1L, (total, current) => total *= projection(current));

        // For given n, return all 2^n lists of true/false combinations
        public static IEnumerable<IList<bool>> AllBoolStates(int n) =>
            Enumerable.Range(0, 1 << n).Select(
                state => Enumerable.Range(0, n).Select(i => GetBit(state, i)).ToList()
            );

        public static bool GetBit(int x, int bit) => (x & (1 << bit)) != 0;
        public static bool GetBit(long x, int bit) => (x & (1L << bit)) != 0;
        public static int SetBit(int x, int bit, bool on) => on ? x | (1 << bit) : x & ~(1 << bit);
        public static long SetBit(long x, int bit, bool on) => on ? x | (1L << bit) : x & ~(1L << bit);
    }
}