using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace _05
{
    class IntcodeComputer
    {
        public enum State { Initialized, Running, BlockedOnInput, Halted };

        public State CurrentState;
        string program;
        int[] memory;
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
            input = new Queue<int>();
            output = new Queue<int>();
            CurrentState = State.Initialized;
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
        void Halt(int flags) => this.CurrentState = State.Halted;

        public State Run()
        {
            CurrentState = State.Running;
            while (CurrentState == State.Running)
            {
                ExecuteInstruction();
            }
            return CurrentState;
        }

        private void ExecuteInstruction()
        {
            var opCode = this[ip] % 100;
            var flags = this[ip] / 100;
            var opInfo = opCache[opCode];

            if (opCode == 3 && !input.Any())
            {
                this.CurrentState = State.BlockedOnInput;
                return;
            }

            opInfo.method.Invoke(this, new Object[] { flags });
            ip += opInfo.attribute.OpSize;
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

        static int RunAmplifiers(List<int> sequence)
        {
            Debug.Assert(sequence.Count == Computers.Count);

            var value = 0;
            for (var i = 0; i < Computers.Count; i++)
            {
                var c = Computers[i];
                c.Reboot();
                c.Input(sequence[i]);
                c.Input(value);
                c.Run();
                value = c.Output();                
            }
            return value;            
        }

        static int RunAmplifiersFeedbackMode(List<int> sequence)
        {
            // Initial Input
            for (var i = 0; i < Computers.Count; i++)
            {
                Computers[i].Reboot();
                Computers[i].Input(sequence[i] + 5);
            }

            // Iterate
            var value = 0;
            while (Computers[4].CurrentState != IntcodeComputer.State.Halted)
            {
                for (var i = 0; i < Computers.Count; i++)
                {
                    Computers[i].Input(value);
                    Computers[i].Run();
                    value = Computers[i].Output();
                }
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

        static void Test(string program, bool part2 = false)
        {
            Computers = Enumerable.Range(0, 5).Select(_ => new IntcodeComputer(program)).ToList();
            (List<int> Sequence, int Value) result = (null, Int32.MinValue);
            foreach (var seq in AllSequences())
            {
                var value = part2 ? RunAmplifiersFeedbackMode(seq) : RunAmplifiers(seq);
                if (value > result.Value)
                {
                    result = (seq, value);
                }
            }
            Console.WriteLine(String.Join(",", result.Sequence) + " " + result.Value);
        }

        static void Tests()
        {
            Test("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0");
            Test("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0");
            Test("3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0");
        }

        static void Tests2()
        {
            Test("3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5", part2: true);
            Test("3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10", part2: true);
        }

        static void Main(string[] args)
        {
            var program = System.IO.File.ReadAllText("input.txt");
            Test(program);
            Test(program, part2: true);
        }
    }
}
