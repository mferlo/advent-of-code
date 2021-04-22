using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    class Day05
    {
        static List<string> Input;

        static int SeatId(string boardingPass) =>
            Convert.ToInt32(
                boardingPass.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1'),
                2
            );

        public static void Parse() => Input = File.ReadAllLines("05.txt").ToList();

        public static void Test()
        {
            var inputs = new List<string> { "FBFBBFFRLR", "BFFFBBFRRR", "FFFBBBFRRR", "BBFFBBFRLL" };
            var expecteds = new List<int> { 357, 567, 119, 820 };

            for (var i = 0; i < 4; i++)
            {
                if (expecteds[i] != SeatId(inputs[i])) throw new Exception($"{inputs[i]} {expecteds[i]} {SeatId(inputs[i])}");
            }
        }

        public static object Part1() => Input.Max(SeatId);

        public static object Part2()
        {
            var seatIds = Input.Select(SeatId).OrderBy(x => x);
            var prev = seatIds.First();
            foreach (var seatId in seatIds.Skip(1))
            {
                if (prev + 1 != seatId)
                {
                    return prev + 1;
                }
                prev = seatId;
            }
            throw new Exception("Shouldn't happen");
        }
    }
}