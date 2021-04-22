using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;


namespace Advent2020
{
    enum Op { acc, jmp, nop }

    class Instruction
    {
        public Op Op { get; private set; }
        public int Arg { get; private set; }
        public int ExecutionCount { get; set; }

        public static Instruction Parse(string line)
        {
            var tmp = line.Split(' ');
            return new Instruction
            {
                Op = Enum.Parse<Op>(tmp[0]),
                Arg = int.Parse(tmp[1]),
                ExecutionCount = 0
            };
        }
    }

    enum State { Running, Stopped, InfiniteLoop }

    class Program
    {
        public int Accumulator { get; private set; }
        public int IP { get; private set; }
        public State State { get; private set; }
        public int InstructionCount => Instructions.Count;

        private List<Instruction> Instructions;

        private Program(List<Instruction> instructions)
        {
            Instructions = instructions;
            Accumulator = 0;
            IP = 0;
        }

        public static Program Parse(IEnumerable<string> lines) =>
            new Program(lines.Select(line => Instruction.Parse(line)).ToList());


        public void Run()
        {
            while (true)
            {
                if (IP < 0 || IP >= Instructions.Count)
                {
                    State = State.Stopped;
                    break;
                }
                if (Instructions[IP].ExecutionCount > 0)
                {
                    State = State.InfiniteLoop;
                    break;
                }
                Step();
            }
        }

        private void Step()
        {
            var inst = Instructions[IP];
            switch (inst.Op)
            {
                case Op.nop:
                    IP++;
                    break;
                case Op.acc:
                    IP++;
                    Accumulator += inst.Arg;
                    break;
                case Op.jmp:
                    IP += inst.Arg;
                    break;
            }
            inst.ExecutionCount++;
        }
    }

    class Day08
    {
        static List<string> TestInput = new List<string>
        {
            "nop +0",
            "acc +1",
            "jmp +4",
            "acc +3",
            "jmp -3",
            "acc -99",
            "acc +1",
            "jmp -4",
            "acc +6"
        };

        static IList<string> Input;

        public static void Parse() => Input = File.ReadAllLines("08.txt");

        public static void Test()
        {
            var program = Program.Parse(TestInput);
            program.Run();
            Console.WriteLine(program.Accumulator);
            Console.WriteLine(Part2(TestInput));
        }

        public static object Part1()
        {
            var program = Program.Parse(Input);
            program.Run();
            return program.Accumulator;
        }

        // Somewhere in the program, either a jmp is supposed to be a nop, or a nop is supposed to be a jmp.
        // (No acc instructions were harmed in the corruption of this boot code.)
        static IEnumerable<Program> GeneratedChangedPrograms(IList<string> input)
        {
            var original = ImmutableList<string>.Empty.AddRange(input);

            for (var i = 0; i < original.Count; i++)
            {
                var inst = original[i];
                if (inst.StartsWith("acc"))
                {
                    continue;
                }
                var newInst = inst.StartsWith("jmp") ? inst.Replace("jmp", "nop") : inst.Replace("nop", "jmp");
                yield return Program.Parse(original.SetItem(i, newInst));
            }
        }

        static object Part2(IList<string> input)
        {
            foreach (var program in GeneratedChangedPrograms(input))
            {
                program.Run();
                if (program.State == State.Stopped && program.IP == program.InstructionCount)
                {
                    return program.Accumulator;
                }
            }
            return 0;
        }

        public static object Part2() => Part2(Input);
    }
}