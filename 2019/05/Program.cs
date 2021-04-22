using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _05
{
    class IntcodeComputer
    {
        string program;
        int[] memory;
        bool done;
        int ip;
        Queue<int> input;
        Queue<int> output;
        Dictionary<int, (MethodInfo method, OpAttribute attribute)> opCache;

        public IntcodeComputer(string program)
        {
            this.program = program;
            PrimeCache();
            Reboot();
        }

        public void Reboot()
        {
            memory = program.Split(",").Select(Int32.Parse).ToArray();
            ip = 0;
            done = false;
            input = new Queue<int>();
            output = new Queue<int>();
        }

        public void Input(int value) => input.Enqueue(value);
        public int Output() => output.Dequeue();
        public IEnumerable<int> AllOutput => output;

        public int this[int i] { get => this.memory[i]; set => this.memory[i] = value; }
        public override string ToString() => $"[{ip}] {string.Join(", ", memory)}";

        int FlagAtPos(int flags, int pos) => (flags / (int)Math.Pow(10, pos)) % 10;

        bool IsImmediate(int flags, int pos) => FlagAtPos(flags, pos) == 1;

        int Arg(int flags, int pos) => IsImmediate(flags, pos) ? this[ip+pos+1] : this[this[ip+pos+1]];

        [Op(1, 4)]
        void Add(int flags) => this[this[ip+3]] = Arg(flags, 0) + Arg(flags, 1);
        [Op(2, 4)]
        void Mul(int flags) => this[this[ip+3]] = Arg(flags, 0) * Arg(flags, 1);
        [Op(3, 2)]
        void ReadInput(int flags) => this[this[ip+1]] = input.Dequeue();
        [Op(4, 2)]
        void WriteOutput(int flags) => output.Enqueue(Arg(flags, 0));
        [Op(5, 0)]
        void JumpIfNonZero(int flags) => ip = Arg(flags, 0) != 0 ? Arg(flags, 1) : ip + 3;
        [Op(6, 0)]
        void JumpIfZero(int flags) => ip = Arg(flags, 0) == 0 ? Arg(flags, 1) : ip + 3;
        [Op(7, 4)]
        void LessThan(int flags) => this[this[ip+3]] = Arg(flags, 0) < Arg(flags, 1) ? 1 : 0;
        [Op(8, 4)]
        void Equals(int flags) => this[this[ip+3]] = Arg(flags, 0) == Arg(flags, 1) ? 1 : 0;

        [Op(99, 1)]
        void Halt(int flags) => this.done = true;

        public void Run()
        {
            while (!done)
            {
                var opCode = this[ip] % 100;
                var flags = this[ip] / 100;
                var opInfo = opCache[opCode];
                opInfo.method.Invoke(this, new Object[] { flags });
                ip += opInfo.attribute.OpSize;
            }
        }

        class OpAttribute : Attribute
        {
            public int OpCode;
            public int OpSize;
            public OpAttribute(int opCode, int opSize)
            {
                OpCode = opCode;
                OpSize = opSize;
            }
        }

        void PrimeCache()
        {
            opCache = new Dictionary<int, (MethodInfo method, OpAttribute attribute)>();
            var methods = typeof(IntcodeComputer).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttributes(typeof(OpAttribute)).Cast<OpAttribute>().SingleOrDefault();
                if (attr != null)
                {
                    opCache[attr.OpCode] = (method, attr);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var program = System.IO.File.ReadAllText("input.txt");

            // Part 1
            var computer = new IntcodeComputer(program);
            computer.Input(1);
            computer.Run();
            var outputs = computer.AllOutput.ToList();
            var result = outputs.Last();
            outputs.RemoveAt(outputs.Count - 1);
            System.Diagnostics.Debug.Assert(outputs.All(val => val == 0));
            Console.WriteLine(result);

            // Part 2
            computer.Reboot();
            computer.Input(5);
            computer.Run();
            Console.WriteLine(computer.Output());
        }
    }
}
