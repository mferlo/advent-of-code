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

        static IEnumerable<Op> ValidOps(IList<int> before, IList<int> input, IList<int> after) =>
            Ops.Where(op => Equal(after, op(before, input[1], input[2], input[3])));

        static IList<int> ReadRegisters(string s) =>
            new[] {
                int.Parse(s.Substring(9, 1)),
                int.Parse(s.Substring(12, 1)),
                int.Parse(s.Substring(15, 1)),
                int.Parse(s.Substring(18, 1))
            };

        static IList<int> ReadOp(string s) =>
            s.Split(' ').Select(int.Parse).ToList();

        static void DoForEachInput(Action<IList<int>, IList<int>, IList<int>> action) {
            IList<int> before, input, after;
            var lines = System.IO.File.ReadLines("input").ToList();
            var i = 0;
            while (lines[i].StartsWith("Before")) {
                before = ReadRegisters(lines[i]);
                input = ReadOp(lines[i + 1]);
                after = ReadRegisters(lines[i + 2]);
                action(before, input, after);
                i += 4;
            }
        }

        static int Part1() {
            var result = 0;
            DoForEachInput((before, input, after) => {
                if (ValidOps(before, input, after).Count() >= 3) {
                    result += 1;
                }
            });
            return result;
        }

        static Dictionary<int, Op> DetermineCodes() {
            var result = new Dictionary<int, Op>();
            while (result.Count() < Ops.Count()) {
                DoForEachInput((before, input, after) => {
                    var code = input[0];
                    if (result.ContainsKey(code)) {
                        return;
                    }
                    var validOps = ValidOps(before, input, after).Except(result.Values);
                    if (validOps.Count() == 1) {
                        result.Add(code, validOps.Single());
                    }
                });
            }
            return result;
        }

        static IList<int> RunProgram(Dictionary<int, Op> ops) {
            IList<int> registers = new int[4];
            foreach (var line in System.IO.File.ReadLines("input").Skip(3354)) {
                var input = ReadOp(line);
                var op = ops[input[0]];
                registers = op(registers, input[1], input[2], input[3]);
            }
            return registers;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Part1());
            var opCodes = DetermineCodes();
            var result = RunProgram(opCodes);
            Console.WriteLine(result[0]);
        }
    }
}
