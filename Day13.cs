using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    class Day13
    {
        static int EarliestDepartureTime;
        static List<string> BusSchedule;

        public static void Parse()
        {
            var lines = File.ReadAllLines("13.txt");
            EarliestDepartureTime = int.Parse(lines[0]);
            BusSchedule = lines[1].Split(",").ToList();
        }

        public static object Part1()
        {
            var busIds = BusSchedule.Where(b => b != "x").Select(int.Parse).ToList();
            var t = EarliestDepartureTime;
            var busId = busIds.SingleOrDefault(b => t % b == 0);
            while (busId == 0)
            {
                t++;
                busId = busIds.SingleOrDefault(b => t % b == 0);
            }
            return busId * (t - EarliestDepartureTime);
        }

        public static void Test()
        {
            Console.WriteLine($"77=> {Part2("7,13".Split(","))}");
            Console.WriteLine($"3417 => {Part2("17,x,13,19".Split(","))}");
            Console.WriteLine($"1068781 => {Part2("7,13,x,x,59,x,31,19".Split(","))}");
            Console.WriteLine($"1202161486 => {Part2("1789,37,47,1889".Split(","))}");
        }

        // Dammit Jim, I'm a programmer, not a mathematician
        static object Part2(IList<string> input)
        {
            // https://en.wikipedia.org/wiki/Chinese_remainder_theorem#Search_by_sieving

            var schedule = input.Select((b, i) => (BusId: b, Offset: i))
                .Where(x => x.BusId != "x")
                .Select(x => (n: int.Parse(x.BusId), a: x.Offset))
                .OrderByDescending(x => x.n)
                .ToList();

            long result = schedule[0].n - schedule[0].a;
            long increment = schedule[0].n;

            foreach (var bus in schedule.Skip(1))
            {
                while ((result + bus.a) % bus.n != 0)
                {
                    result += increment;
                }
                increment *= bus.n;
            }

            return result;
        }

        public static object Part2() => Part2(BusSchedule);
    }
}