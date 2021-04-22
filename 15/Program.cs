using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _15 { class Program {
    static IEnumerable<long> Generator(long factor, long seed) {
        long prev = seed;
        while (true) {
            long cur = (prev * factor) % Int32.MaxValue;
            yield return cur;
            prev = cur;
        }
    }

    static bool Judge(long a, long b) => (a & 0xFFFF) == (b & 0xFFFF);

    static int Part1(IEnumerable<long> a, IEnumerable<long> b) =>
        a.Zip(b, Judge).Take(40_000_000).Count(x => x);

    static int Part2(IEnumerable<long> a, IEnumerable<long> b) =>
        a.Where(x => x % 4 == 0).Zip(b.Where(y => y % 8 == 0),
            Judge).Take(5_000_000).Count(x => x);

    static void Main(string[] args) {
        Console.WriteLine(Part1(Generator(16807, 679), Generator(48271, 771)));
        Console.WriteLine(Part2(Generator(16807, 679), Generator(48271, 771)));
    }        
}}
