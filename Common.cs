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
    }
}