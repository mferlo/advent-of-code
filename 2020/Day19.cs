using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    class MessageRule
    {
        public int Number { get; }
        public int Options { get; }
        public char Constant { get; }
        public IReadOnlyList<int> Refs { get; }
        public IReadOnlyList<int> AltRefs { get; }

        public override string ToString()
        {
            if (Options == 0)
            {
                return $"'{Constant}'";
            }
            var result = string.Join(" ", Refs);
            if (Options == 2)
            {
                result += $" | {string.Join(" ", AltRefs)}";
            }
            return result;
        }

        public MessageRule(string line)
        {
            var foo = line.Split(": ");
            Number = int.Parse(foo[0]);
            var rest = foo[1];

            if (rest.StartsWith('"'))
            {
                Constant = rest[1];
                Options = 0;
                Refs = null;
                AltRefs = null;
            }
            else
            {
                Constant = default(char);
                if (!rest.Contains('|'))
                {
                    Options = 1;
                    Refs = rest.Split(' ').Select(int.Parse).ToList();
                    AltRefs = null;
                }
                else
                {
                    Options = 2;
                    var bar = rest.Split(" | ");
                    Refs = bar[0].Split(' ').Select(int.Parse).ToList();
                    AltRefs = bar[1].Split(' ').Select(int.Parse).ToList();
                }
            }
        }
    }

    class Day19
    {
        static Dictionary<int, MessageRule> Rules;
        static List<string> Input;
        static Dictionary<int, string> Cache;
        static Dictionary<int, int> MinLength;

        static void Parse(string rules, string input)
        {
            Rules = rules.Split("\n").Select(r => new MessageRule(r)).ToDictionary(r => r.Number);
            Input = input.Split("\n").ToList();
        }

        public static void Parse()
        {
            var lines = File.ReadAllText("19.txt");
            var parts = lines.Split("\n\n");
            Parse(parts[0], parts[1]);
        }

        static string AssembleRegex(int ruleNumber, bool part2)
        {
            if (Cache.ContainsKey(ruleNumber))
            {
                return Cache[ruleNumber];
            }

            string result;
            var rule = Rules[ruleNumber];

            if (rule.Options == 0)
            {
                result = rule.Constant.ToString();
                MinLength[ruleNumber] = 1;
            }
            else
            {
                if (part2 && ruleNumber == 8)
                {
                    // Rules[8] = new MessageRule("8: 42 | 42 8");
                    result = $"({AssembleRegex(42, part2)})+";
                    MinLength[8] = MinLength[42];
                }
                else if (part2 && ruleNumber == 11)
                {
                    // Rules[11] = new MessageRule("11: 42 31 | 42 11 31");

                    // As the problem reminded us: "you only need to handle the rules you have"
                    // The same applies to the input...
                    //
                    // That said, I'm both disgusted and proud of this approach.

                    var first = AssembleRegex(42, part2);
                    var last = AssembleRegex(31, part2);
                    var matchLength = MinLength[42] + MinLength[31];
                    MinLength[11] = matchLength;

                    var piece = $"{first}{last}";
                    var pieces = new List<string>();
                    var maxInputLength = Input.Max(i => i.Length);
                    while (pieces.Count * matchLength < maxInputLength)
                    {
                        pieces.Add(piece);
                        piece = $"{first}{piece}{last}";
                    }
                    result = $"({string.Join("|", pieces)})";
                }
                else
                {
                    result = string.Join("", rule.Refs.Select(n => AssembleRegex(n, part2)));

                    if (rule.Options == 2)
                    {
                        var alt = string.Join("", rule.AltRefs.Select(n => AssembleRegex(n, part2)));
                        result = $"({result}|{alt})";
                    }
                }

                var refSum = rule.Refs.Sum(n => MinLength[n]);
                if (rule.Options == 1)
                {
                    MinLength[ruleNumber] = refSum;
                }
                else
                {
                    var altSum = rule.AltRefs.Sum(n => MinLength[n]);
                    MinLength[ruleNumber] = Math.Min(refSum, altSum);
                }
            }

            Cache[ruleNumber] = result;
            return result;
        }

        static Regex AssembleRegex(bool part2)
        {
            Cache = new Dictionary<int, string>();
            MinLength = new Dictionary<int, int>();
            return new Regex($"^{AssembleRegex(0, part2)}$");
        }

        public static void Test()
        {
            Parse(TestRules.Replace("\r\n", "\n"), TestInput.Replace("\r\n", "\n"));
            var re = AssembleRegex(false);
            foreach (var input in Input)
            {
                Console.WriteLine($"{input} : {re.IsMatch(input)}");
            }
            Console.WriteLine();
        }

        public static object Part1()
        {
            var re = AssembleRegex(false);
            return Input.Count(input => re.IsMatch(input));
        }

        public static object Part2()
        {
            Rules[8] = new MessageRule("8: 42 | 42 8");
            Rules[11] = new MessageRule("11: 42 31 | 42 11 31");
            var re = AssembleRegex(true);
            // Fun fact: Regex's string length is 30715 (part 1 was only 2145)
            return Input.Count(input => re.IsMatch(input));
        }


        static string TestRules = @"0: 4 1 5
1: 2 3 | 3 2
2: 4 4 | 5 5
3: 4 5 | 5 4
4: ""a""
5: ""b""";
        static string TestInput = @"ababbb
bababa
abbbab
aaabbb
aaaabbb";
    }
}