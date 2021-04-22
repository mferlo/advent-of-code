using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    class Day06
    {
        static List<string> Input;

        public static void Parse() => Input = File.ReadAllText("06.txt").Split("\n\n").ToList();

        static int YesCount(string group) => group.Replace("\n", null).Distinct().Count();

        static int UnanimousYesCount(string group)
        {
            var people = group.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var answers = new HashSet<char>(people.First());
            foreach (var person in people)
            {
                answers.IntersectWith(person);
            }
            return answers.Count;
        }

        public static object Part1() => Input.Sum(YesCount);
        public static object Part2() => Input.Sum(UnanimousYesCount);
    }
}