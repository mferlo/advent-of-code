using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    [DebuggerDisplay("{Color}")]
    class Bag
    {
        public Bag(string color)
        {
            Color = color;
            Contents = new List<(int Count, Bag Bag)>();
            ContainedIn = new List<Bag>();
        }

        public string Color;
        public List<(int Count, Bag Bag)> Contents;
        public List<Bag> ContainedIn;
    }

    class Day07
    {
        static Dictionary<string, Bag> Input;

        static List<string> TestData = new List<string>()
        {
            "light red bags contain 1 bright white bag, 2 muted yellow bags.",
            "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
            "bright white bags contain 1 shiny gold bag.",
            "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
            "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags."
        };

        static Regex re = new Regex(@"(?<number>\d+) (?<color>.*) bags?");
        static (int Count, string Color) ParseContent(string content)
        {
            var groups = re.Match(content).Groups;
            return (Count: int.Parse(groups["number"].Value), groups["color"].Value);
        }

        static IEnumerable<(int Count, string Color)> ParseContents(string contents)
        {
            if (contents == "no other bags.")
            {
                return Enumerable.Empty<(int Count, string Color)>();
            }
            else
            {
                return contents.TrimEnd('.').Split(", ").Select(ParseContent);
            }
        }

        static Dictionary<string, Bag> ParseRules(IList<string> lines)
        {
            var lineParts = lines.Select(line => line.Split(" bags contain ")).ToList();

            var bags = lineParts.Select(parts => parts[0])
                .Select(color => new Bag(color))
                .ToDictionary(bag => bag.Color);
            
            foreach (var parts in lineParts)
            {
                var outerBag = bags[parts[0]];
                foreach (var content in ParseContents(parts[1]))
                {
                    var innerBag = bags[content.Color];
                    innerBag.ContainedIn.Add(outerBag);

                    outerBag.Contents.Add((Count: content.Count, Bag: innerBag));
                }
            }
            return bags;
        }

        static IEnumerable<Bag> BagsThatCanContain(string color)
        {
            var work = new Queue<Bag>(Input[color].ContainedIn);
            var result = new HashSet<Bag>();

            while (work.Any())
            {
                var cur = work.Dequeue();
                result.Add(cur);
                foreach (var parent in cur.ContainedIn)
                {
                    work.Enqueue(parent);
                }
            }
            return result;
        }

        static int BagsContainedIn(Bag bag) =>
            bag.Contents.Sum(b => b.Count * (1 + BagsContainedIn(b.Bag)));

        const string MyBagColor = "shiny gold";

        public static void Test()
        {
            Input = ParseRules(TestData);
            Trace.Assert(4 == BagsThatCanContain(MyBagColor).Count());
            Trace.Assert(32 == BagsContainedIn(Input[MyBagColor]));
        }

        public static void Parse() => Input = ParseRules(File.ReadAllLines("07.txt"));
        public static object Part1() => BagsThatCanContain(MyBagColor).Count();
        public static object Part2() => BagsContainedIn(Input[MyBagColor]);
    }
}