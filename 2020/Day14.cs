using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    class Day14
    {
        static string[] Input;

        public static void Parse() => Input = File.ReadAllLines("14.txt");
        
        static long ThirtySixBits;
        static long OnesMask;
        static long ZeroesMask;

        static Day14()
        {
            for (var i = 0; i < 36; i++)
            {
                ThirtySixBits |= 1L << i;
            }
        }

        static void SetMask(string mask)
        {
            OnesMask = 0;
            ZeroesMask = ~0 & ThirtySixBits;
            var i = 0;
            foreach (var ch in mask.Reverse())
            {
                switch (ch)
                {
                    case '1': OnesMask |= 1L << i; break;
                    case '0': ZeroesMask &= ~(1L << i); break;
                    default: break;
                }
                i++;
            }
        }

        static long ApplyMask(long value) => (value & ZeroesMask) | OnesMask;

        static object Part1(IList<string> program)
        {
            var memory = new Dictionary<int, long>();

            foreach (var line in program)
            {
                var parts = line.Split(" = ");
                if (parts[0] == "mask")
                {
                    SetMask(parts[1]);
                }
                else
                {
                    var address = int.Parse(parts[0].Substring(4, parts[0].Length - 5));
                    var value = int.Parse(parts[1]);
                    memory[address] = ApplyMask(value);
                }
            }

            return memory.Values.Sum();
        }

        public static void Test()
        {
            var testData = new List<string>
            {
                "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
                "mem[8] = 11",
                "mem[7] = 101",
                "mem[8] = 0"
            };
            Console.WriteLine(Part1(testData));

            var testData2 = new List<string>
            {
                "mask = 000000000000000000000000000000X1001X",
                "mem[42] = 100",
                "mask = 00000000000000000000000000000000X0XX",
                "mem[26] = 1"
            };
            Console.WriteLine(Part2(testData2));
        }

        public static object Part1() => Part1(Input);
        public static object Part2() => Part2(Input);
        
        static IEnumerable<long> Addresses(long address, string mask)
        {
            var floating = new List<int>();
            var i = 0;
            foreach (var ch in mask.Reverse())
            {
                if (ch == '1')
                {
                    address = Common.SetBit(address, i, true);
                }
                else if (ch == 'X')
                {
                    floating.Add(i);
                }
                i++;
            }

            foreach (var state in Common.AllBoolStates(floating.Count))
            {
                yield return state.Zip(floating, (isOn, bit) => new { isOn, bit })
                    .Aggregate(address, (res, bitState) => Common.SetBit(res, bitState.bit, bitState.isOn));
            }
        }

        static object Part2(IList<string> program)
        {
            var memory = new Dictionary<long, long>();
            var mask = "";

            foreach (var line in program)
            {
                var parts = line.Split(" = ");
                if (parts[0] == "mask")
                {
                    mask = parts[1];
                }
                else
                {
                    var givenAddress = long.Parse(parts[0].Substring(4, parts[0].Length - 5));
                    var value = int.Parse(parts[1]);
                    var addresses = Addresses(givenAddress, mask).ToList();
                    foreach (var address in addresses)
                    {
                        memory[address] = value;
                    }
                }
            }

            return memory.Values.Sum();
        }
    }
}