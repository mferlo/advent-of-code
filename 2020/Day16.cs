using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Range
    {
        public int Min { get; }
        public int Max { get; }

        public Range(int min, int max) => (Min, Max) = (min, max);
        public bool IsValid(int x) => Min <= x && x <= Max;
    }


    class Rule
    {
        public string Name { get; }
        Range r1, r2;

        static readonly char[] parseHelper = new char[] { ' ', '-' };

        public Rule(string input)
        {
            var s = input.Split(":");
            Name = s[0];
            var parts = s[1].Split(parseHelper, StringSplitOptions.RemoveEmptyEntries);

            r1 = new Range(int.Parse(parts[0]), int.Parse(parts[1]));
            r2 = new Range(int.Parse(parts[3]), int.Parse(parts[4]));
        }

        public bool IsValid(int x) => r1.IsValid(x) || r2.IsValid(x);
    }


    class Ticket
    {
        public ImmutableList<int> Values { get; }

        public Ticket(string input) =>
            Values = ImmutableList<int>.Empty.AddRange(input.Split(",").Select(int.Parse));

        public bool IsValid(IEnumerable<Rule> rules) =>
            Values.All(value => rules.Any(rule => rule.IsValid(value)));
        
        public IEnumerable<int> InvalidValues(IEnumerable<Rule> rules) =>
            Values.Where(value => rules.All(rule => !rule.IsValid(value)));
    }


    class Day16
    {
        static ImmutableList<Rule> Rules;
        static Ticket MyTicket;
        static ImmutableList<Ticket> Tickets;

        public static void Parse()
        {
            var lines = File.ReadAllLines("16.txt");
            Rules = ImmutableList<Rule>.Empty.AddRange(lines.Take(20).Select(line => new Rule(line)));
            MyTicket = new Ticket(lines[22]);
            Tickets = ImmutableList<Ticket>.Empty.AddRange(lines.Skip(25).Select(line => new Ticket(line)));
        }

        public static object Part1() =>
            Tickets.SelectMany(ticket => ticket.InvalidValues(Rules)).Sum();
        

        static List<string> ValidFieldsForValues(List<int> values, IEnumerable<Rule> rules) =>
            rules.Where(rule => values.All(value => rule.IsValid(value))).Select(rule => rule.Name).ToList();

        static IEnumerable<List<string>> ValidFields(List<Ticket> tickets, IEnumerable<Rule> rules)
        {
            var numFields = tickets.First().Values.Count;
            for (var i = 0; i < numFields; i++)
            {
                yield return ValidFieldsForValues(tickets.Select(t => t.Values[i]).ToList(), rules);
            }
        }

        static Dictionary<string, int> ResolveAmbiguity(List<List<string>> validFieldsPerIndex, ISet<string> names)
        {
            var result = new Dictionary<string, int>();
            var fieldsToAssign = new HashSet<string>(names);
            while (fieldsToAssign.Any())
            {
                for (var index = 0; index < validFieldsPerIndex.Count; index++)
                {
                    var validFields = validFieldsPerIndex[index];
                    if (validFields.Count == 1)
                    {
                        var field = validFields.Single();
                        result[field] = index;
                        fieldsToAssign.Remove(field);
                        foreach (var fieldList in validFieldsPerIndex)
                        {
                            fieldList.Remove(field);
                        }
                        break;
                    }
                }
            }
            return result;            
        }

        public static object Part2()
        {
            var validTickets = Tickets.Where(ticket => ticket.IsValid(Rules)).ToList();
            var fieldNames = Rules.Select(rule => rule.Name).ToHashSet();

            var validFieldsPerIndex = ValidFields(validTickets, Rules).ToList();
            var fieldIndexLookup = ResolveAmbiguity(validFieldsPerIndex, fieldNames);

            var result = 1L;
            foreach (var name in fieldNames.Where(n => n.StartsWith("departure")))
            {
                result *= MyTicket.Values[fieldIndexLookup[name]];
            }
            return result;
        }
    }
}