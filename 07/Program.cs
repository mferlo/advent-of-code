using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        static List<IntcodeComputer> Computers;

        static int RunAmplifiers(List<int> Sequence)
        {
            Debug.Assert(Sequence.Count == Computers.Count);

            var value = 0;
            for (var i = 0; i < Computers.Count; i++)
            {
                var c = Computers[i];
                c.Reboot();
                c.Input(Sequence[i]);
                c.Input(value);
                c.Run();
                value = c.Output();                
            }
            return value;            
        }

        static List<int> ToBase5Digits(int x)
        {
            var digits = new List<int> { 0, 0, 0, 0, 0 };
            for (int i = 4; i >= 0; i--)
            {
                digits[i] = x % 5;
                x /= 5;
            }
            return digits;
        }

        static bool HasAll5Digits(List<int> seq) => Enumerable.Range(0, 5).All(i => seq.Contains(i));

        static IEnumerable<List<int>> AllSequences() =>
            Enumerable.Range(0, (int)Math.Pow(5, 5)).Select(ToBase5Digits).Where(HasAll5Digits);

        static (List<int> Sequence, int Value) Max(string program)
        {
            (List<int> Sequence, int Value) result = (null, Int32.MinValue);
            foreach (var seq in AllSequences())
            {
                var value = RunAmplifiers(seq);
                if (value > result.Value)
                {
                    result = (seq, value);
                }
            }
            return result;
        }

        static void Test(string program)
        {
            Computers = Enumerable.Range(0, 5).Select(_ => new IntcodeComputer(program)).ToList();
            var result = Max(program);
            Console.WriteLine(String.Join(",", result.Sequence) + " " + result.Value);
        }

        static void Tests()
        {
            Test("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0");
            Test("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0");
            Test("3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0");
        }

        static void Main(string[] args)
        {
            // Tests();
            Test(System.IO.File.ReadAllText("input.txt"));
        }
    }
}
