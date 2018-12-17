using System;
using System.Collections.Generic;
using System.Linq;

namespace _16
{
    class Program
    {
        delegate IList<int> Op(IList<int> r, int a, int b, int c);

        static IList<int> AddR(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a] + r[b];
            return result;
        }

        static IList<int> AddI(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a] + b;
            return result;
        }

        static IList<int> MulR(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a] * r[b];
            return result;
        }

        static IList<int> MulI(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a] * b;
            return result;
        }

        static IList<int> BanR(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a] & r[b];
            return result;
        }

        static IList<int> BanI(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a] & b;
            return result;
        }

        static IList<int> BorR(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a] | r[b];
            return result;
        }

        static IList<int> BorI(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a] | b;
            return result;
        }

        static IList<int> SetR(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a];
            return result;
        }

        static IList<int> SetI(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = a;
            return result;
        }

        static IList<int> GtIR(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = a > r[b] ? 1 : 0;
            return result;
        }

        static IList<int> GtRI(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a] > b ? 1 : 0;
            return result;
        }

        static IList<int> GtRR(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a] > r[b] ? 1 : 0;
            return result;
        }

        static IList<int> EqIR(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = a == r[b] ? 1 : 0;
            return result;
        }

        static IList<int> EqRI(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a] == b ? 1 : 0;
            return result;
        }

        static IList<int> EqRR(IList<int> r, int a, int b, int c) {
            var result = new List<int>(r);
            result[c] = r[a] == r[b] ? 1 : 0;
            return result;
        }

        static IList<Op> Ops = new List<Op> {
            AddR, AddI,
            MulR, MulI,
            BanR, BanI,
            BorR, BorI,
            SetR, SetI,
            GtIR, GtRI, GtRR,
            EqIR, EqRI, EqRR
        };

        static bool Equal(IList<int> x, IList<int> y) {
            for (var i = 0; i < x.Count; i++) {
                if (x[i] != y[i]) {
                    return false;
                }
            }
            return true;
        }

        static int ValidOpCodes(IList<int> before, IList<int> input, IList<int> after) =>
            Ops.Count(op => Equal(after, op(before, input[1], input[2], input[3])));

        static IList<int> ReadRegisters(string s) =>
            new[] {
                int.Parse(s.Substring(9, 1)),
                int.Parse(s.Substring(12, 1)),
                int.Parse(s.Substring(15, 1)),
                int.Parse(s.Substring(18, 1))
            };

        static IList<int> ReadOp(string s) =>
            s.Split(' ').Select(int.Parse).ToList();

        static int Part1() {
            var result = 0;
            IList<int> before, input, after;
            var lines = System.IO.File.ReadLines("input").ToList();
            var i = 0;
            while (lines[i].StartsWith("Before")) {
                before = ReadRegisters(lines[i]);
                input = ReadOp(lines[i + 1]);
                after = ReadRegisters(lines[i + 2]);
                if (ValidOpCodes(before, input, after) >= 3) {
                    result += 1;
                }
                i += 4;
            }
            return result;
        }

        static Dictionary<int, Op> DetermineCodes() {
            throw new Exception();
        }

        static void Test() {
            var before = new[] { 3, 2, 1, 1 };
            var input = new[] { 9, 2, 1, 2 };
            var after = new[] { 3, 2, 2, 1 };
            Console.WriteLine(ValidOpCodes(before, input, after));
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Part1());
        }
    }
}
