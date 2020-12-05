using System;
using System.Diagnostics;
using System.Reflection;

namespace Advent2020
{
    class Program
    {
        static void Main(string[] args)
        {
            var (parse, test, part1, part2) = GetDayActions(args);

            parse.Invoke(null, null);

            if (test != null)
            {
                test.Invoke(null, null);
            }

            Time(part1, "Part 1");
            Time(part2, "Part 2");
        }

        static void Time(MethodInfo method, string description)
        {
            if (method == null)
            {
                Console.WriteLine($"{description} NYI");
                return;
            }

            var sw = Stopwatch.StartNew();
            object result = method.Invoke(null, null);
            sw.Stop();
            Console.WriteLine(description);
            Console.WriteLine(result);
            Console.WriteLine($"Elapsed time: {sw.Elapsed}");
            Console.WriteLine();
        }

        static (MethodInfo Parse, MethodInfo Test, MethodInfo Part1, MethodInfo Part2) GetDayActions(string[] args)
        {
            // TODO: Allow individually invoking Part1/Part2
            if (args.Length != 1)
            {
                throw new Exception("Usage: DayNumber");
            }

            var type = Type.GetType($"Advent2020.Day{args[0]}");
            if (type == null)
            {
                type = Type.GetType($"Advent2020.Day0{args[0]}");
            }
            if (type == null)
            {
                throw new Exception($"Day '{args[0]}' not found");
            }

            return (
                type.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public),
                type.GetMethod("Test", BindingFlags.Static | BindingFlags.Public),
                type.GetMethod("Part1", BindingFlags.Static | BindingFlags.Public),
                type.GetMethod("Part2", BindingFlags.Static | BindingFlags.Public)
            );
        }
    }
}
