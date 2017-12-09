using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _09 { class Program {

    static (int part1, int part2) Solve(string input) {
        var depth = 0;
        var part1 = 0;
        var part2 = 0;

        var inGarbage = false;
        var cancelNext = false;

        foreach (var ch in input) {
            if (cancelNext) {
                cancelNext = false;
            } else if (inGarbage) {
                if (ch == '>') {
                    inGarbage = false;
                } else if (ch == '!') {
                    cancelNext = true;
                } else {
                    part2 += 1;
                }
            } else {
                if (ch == '{') {
                    depth += 1;
                } else if (ch == '}') {
                    part1 += depth;
                    depth -= 1;
                } else if (ch == '<') {
                    inGarbage = true;
                }
            }
        }

        return (part1, part2);
    }

    static void Test() {
        var inputs = new List<(string text, int expected)> {
            ("{}", 1),
            ("{{{}}}", 6),
            ("{{},{}}", 5),
            ("{{{},{},{{}}}}", 16),
            ("{<a>,<a>,<a>,<a>}", 1),
            ("{{<ab>},{<ab>},{<ab>},{<ab>}}", 9),
            ("{{<!!>},{<!!>},{<!!>},{<!!>}}", 9),
            ("{{<a!>},{<a!>},{<a!>},{<ab>}}", 3)
        };

        foreach (var input in inputs) {
            var actual = Solve(input.text).part1;
            Console.WriteLine($"{actual == input.expected} {actual} {input.expected}");
        }
    }

    static void Main(string[] args) {
        // Test();
        var solution = Solve(File.ReadAllText("input"));
        Console.WriteLine($"Part 1: {solution.part1}");
        Console.WriteLine($"Part 2: {solution.part2}");
    }
}}
