using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _19
{
    class Program
    {
        static int[] r;

        delegate void Op(int a, int b, int c);

        static void AddR(int a, int b, int c) =>
            r[c] = r[a] + r[b];

        static void AddI(int a, int b, int c) =>
            r[c] = r[a] + b;

        static void MulR(int a, int b, int c) =>
            r[c] = r[a] * r[b];

        static void MulI(int a, int b, int c) =>
            r[c] = r[a] * b;

        static void BanR(int a, int b, int c) =>
            r[c] = r[a] & r[b];

        static void BanI(int a, int b, int c) =>
            r[c] = r[a] & b;

        static void BorR(int a, int b, int c) =>
            r[c] = r[a] | r[b];

        static void BorI(int a, int b, int c) =>
            r[c] = r[a] | b;

        static void SetR(int a, int b, int c) =>
            r[c] = r[a];

        static void SetI(int a, int b, int c) =>
            r[c] = a;

        static void GtIR(int a, int b, int c) =>
            r[c] = a > r[b] ? 1 : 0;

        static void GtRI(int a, int b, int c) =>
            r[c] = r[a] > b ? 1 : 0;

        static void GtRR(int a, int b, int c) =>
            r[c] = r[a] > r[b] ? 1 : 0;

        static void EqIR(int a, int b, int c) =>
            r[c] = a == r[b] ? 1 : 0;

        static void EqRI(int a, int b, int c) =>
            r[c] = r[a] == b ? 1 : 0;

        static void EqRR(int a, int b, int c) =>
            r[c] = r[a] == r[b] ? 1 : 0;

        static IList<Op> OpList = new List<Op> {
            AddR, AddI,
            MulR, MulI,
            BanR, BanI,
            BorR, BorI,
            SetR, SetI,
            GtIR, GtRI, GtRR,
            EqIR, EqRI, EqRR,
        };

        static Dictionary<string, Op> Ops = OpList.ToDictionary(op => op.GetMethodInfo().Name.ToLower());

        static Action ParseLine(string line)
        {
                var parts = line.Split(' ');
                var op = Ops[parts[0]];
                var a = int.Parse(parts[1]);
                var b = int.Parse(parts[2]);
                var c = int.Parse(parts[3]);
                return () => op(a, b, c);
        }

        static void Main(string[] args)
        {
            var program = input.Split("\r\n").ToList();
            var ipRegister = int.Parse(program[0].Split(' ')[1]);
            var instructions = program.Skip(1).Select(ParseLine).ToList();

            r = new int[6];
            var ip = 0;
            r[0] = 1;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            while (ip >= 0 && ip < instructions.Count)
            {
                r[ipRegister] = ip;
                instructions[ip]();
                ip = r[ipRegister] + 1;
            }
            timer.Stop();
            Console.WriteLine(r[0]);
            Console.WriteLine(timer.Elapsed);
        }

        const string test = @"#ip 0
seti 5 0 1
seti 6 0 2
addi 0 1 0
addr 1 2 3
setr 1 0 0
seti 8 0 4
seti 9 0 5";

        const string input = @"#ip 3
addi 3 16 3
seti 1 0 4
seti 1 7 2
mulr 4 2 1
eqrr 1 5 1
addr 1 3 3
addi 3 1 3
addr 4 0 0
addi 2 1 2
gtrr 2 5 1
addr 3 1 3
seti 2 6 3
addi 4 1 4
gtrr 4 5 1
addr 1 3 3
seti 1 3 3
mulr 3 3 3
addi 5 2 5
mulr 5 5 5
mulr 3 5 5
muli 5 11 5
addi 1 6 1
mulr 1 3 1
addi 1 13 1
addr 5 1 5
addr 3 0 3
seti 0 6 3
setr 3 1 1
mulr 1 3 1
addr 3 1 1
mulr 3 1 1
muli 1 14 1
mulr 1 3 1
addr 5 1 5
seti 0 0 0
seti 0 3 3";
    }
}
