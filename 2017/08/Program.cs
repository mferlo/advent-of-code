using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _08 { 

    class Registers {
        private Dictionary<string, int> r;
        private int max;

        public Registers() {
            r = new Dictionary<string, int>();
            max = 0;
        }

        public int CurrentMax => r.Values.Max();
        public int HistoricMax => max;

        public void ProcessLine(string inputLine) {
            var tokens = inputLine.Split(' ');

            var reg = tokens[0];
            var instr = tokens[1];
            var value = int.Parse(tokens[2]);
            var testReg = tokens[4]; // skip [3]
            var cmp = tokens[5];
            var testValue = int.Parse(tokens[6]);
                    
            if (Test(testReg, cmp, testValue)) {
                Set(reg, instr, value);
            }
        }

        private int Get(string reg) {
            if (!r.ContainsKey(reg)) {
                r[reg] = 0;
            }

            return r[reg];
        }

        private void Set(string reg, string instr, int value) {
            var cur = Get(reg);
            if (instr == "inc") {
                cur += value;
            } else {
                cur -= value;
            }

            r[reg] = cur;
            if (cur > max) {
                max = cur;
            }
        }

        private bool Test(string lhsReg, string cmp, int rhs) {
            var lhs = Get(lhsReg);
            switch (cmp) {
                case "==": return lhs == rhs;
                case "!=": return lhs != rhs;
                case ">=": return lhs >= rhs;
                case ">":  return lhs >  rhs;
                case "<=": return lhs <= rhs;
                case "<":  return lhs <  rhs;
                default: throw new Exception(cmp);
            }
        }
    }

    class Program {
        static void Main(string[] args)
        {
            var registers = new Registers();
            File.ReadAllLines("input")
                .ToList()
                .ForEach(line => registers.ProcessLine(line));

            Console.WriteLine($"Part 1: {registers.CurrentMax}");
            Console.WriteLine($"Part 2: {registers.HistoricMax}");
        }
    }
}
