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

        [Op("add", 1, 4)]
        void Op1(int flags) => this[this[ip+3]] = Arg(flags, 0) + Arg(flags, 1);
        [Op("mul", 2, 4)]
        void Op2(int flags) => this[this[ip+3]] = Arg(flags, 0) * Arg(flags, 1);
        [Op("Input", 3, 2)]
        void Op3(int flags) => this[this[ip+1]] = input.Dequeue();
        [Op("Output", 4, 2)]
        void Op4(int flags) => output.Enqueue(Arg(flags, 0));
        [Op("jnz", 5, 0)]
        void Op5(int flags) => ip = Arg(flags, 0) != 0 ? Arg(flags, 1) : ip + 3;
        [Op("jz", 6, 0)]
        void Op6(int flags) => ip = Arg(flags, 0) == 0 ? Arg(flags, 1) : ip + 3;
        [Op("lt", 7, 4)]
        void Op7(int flags) => this[this[ip+3]] = Arg(flags, 0) < Arg(flags, 1) ? 1 : 0;
        [Op("eq", 8, 4)]
        void Op8(int flags) => this[this[ip+3]] = Arg(flags, 0) == Arg(flags, 1) ? 1 : 0;

        [Op("Halt", 99, 1)]
        void Op99(int flags) => this.done = true;

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
            public string Name;
            public int OpCode;
            public int OpSize;
            public OpAttribute(string name, int opCode, int opSize)
            {
                Name = name;
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
