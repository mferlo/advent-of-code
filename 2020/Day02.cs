using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day02
    {
        struct PasswordEntry
        {
            public int Min;
            public int Max;
            public char Ch;
            public string Password;

            public static PasswordEntry Parse(string line)
            {
                var data = line.Split(new[] { "-", ":", " " }, StringSplitOptions.RemoveEmptyEntries);
                return new PasswordEntry
                {
                    Min = int.Parse(data[0]),
                    Max = int.Parse(data[1]),
                    Ch = data[2][0],
                    Password = data[3],
                };
            }

            public bool IsValidPart1()                
            {
                var target = Ch;
                var count = Password.Count(ch => ch == target);
                return Min <= count && count <= Max;
            }

            public bool IsValidPart2()
            {
                var first = Password[Min - 1] == Ch;
                var second = Password[Max - 1] == Ch;
                return first ^ second;
            }
        }

        static List<PasswordEntry> Input;

        public static void Parse()
        {
            Input = File.ReadAllLines("02.txt").Select(line => PasswordEntry.Parse(line)).ToList();
        }

        public static object Part1()
        {
            return Input.Count(pe => pe.IsValidPart1());
        }

        public static object Part2()
        {
            return Input.Count(pe => pe.IsValidPart2());
        }
    }
}