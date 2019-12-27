using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace _09
{
    class Memory
    {
        Dictionary<long, long> M;
        Dictionary<long, long> ROM;

        public Memory(IEnumerable<long> initialValues)
        {
            ROM = initialValues.Select((value, pos) => (value, pos: (long)pos)).ToDictionary(kvp => kvp.pos, kvp => kvp.value);
            Reboot();
        }

        public void Reboot() => M = new Dictionary<long, long>(ROM);

        public long this[long i]
        {
            get {
                if (i < 0) { throw new Exception(); }
                return M.GetValueOrDefault(i);
            }
            set {
                if (i < 0) { throw new Exception(); }
                M[i] = value;
            }
        }
    }


    class IntcodeComputer
    {
        public enum State { Initialized, Running, BlockedOnInput, Halted };

        public State CurrentState;
        Memory memory;
        long ip;
        long relativeBase;
        Queue<long> input;
        Queue<long> output;
        Dictionary<int, (MethodInfo method, OpAttribute attribute)> opCache;

        public IntcodeComputer(string program)
        {
            Boot();
            SetMemory(program);
        }

        void Boot()
        {
            PrimeCache();
            POST();
        }

        void SetMemory(string program) => memory = new Memory(program.Split(",").Select(long.Parse));

        public void Reboot()
        {
            memory.Reboot();
            ip = 0;
            relativeBase = 0;
            input = new Queue<long>();
            output = new Queue<long>();
            CurrentState = State.Initialized;
        }

        public void Input(long value) => input.Enqueue(value);
        public long Output() => output.Dequeue();
        public IEnumerable<long> AllOutput => output;

        enum ArgType { Position = 0, Immediate = 1, Relative = 2, OpCode = 99 };
        List<ArgType> GetArgTypes(OpAttribute opInfo, int flags)
        {
            IEnumerable<ArgType> GetArgs()
            {
                yield return ArgType.OpCode;
                for (int i = 1; i <= opInfo.ArgCount; i++)
                {
                    if (flags > 0)
                    {
                        yield return (ArgType)(flags % 10);
                        flags /= 10;                    
                    }
                    else
                    {
                        yield return (ArgType)0;
                    }
                }
            }

            return GetArgs().ToList();
        }

        long Value(long pos, ArgType argType)
        {
            var valueAtPosition = memory[pos];
            return argType switch
            {
                ArgType.Immediate => valueAtPosition,
                ArgType.Position => memory[valueAtPosition],
                ArgType.Relative => memory[valueAtPosition + relativeBase],
                _ => throw new Exception()
            };
        }

        void AssignValue(long pos, ArgType argType, long value)
        {
            var pointer = memory[pos];
            if (argType == ArgType.Position)
            {
                memory[pointer] = value;
            }
            else if (argType == ArgType.Relative)
            {
                memory[pointer + relativeBase] = value;
            }
            else
            {
                throw new Exception();
            }
        }

        [Op(1, 4, 3)]
        void Add(List<ArgType> args) => AssignValue(ip+3, args[3], Value(ip+1, args[1]) + Value(ip+2, args[2]));
        [Op(2, 4, 3)]
        void Mul(List<ArgType> args) => AssignValue(ip+3, args[3], Value(ip+1, args[1]) * Value(ip+2, args[2]));
        [Op(3, 2, 1)]
        void ReadInput(List<ArgType> args) => AssignValue(ip+1, args[1], input.Dequeue());
        [Op(4, 2, 1)]
        void WriteOutput(List<ArgType> args) => output.Enqueue(Value(ip+1, args[1]));
        [Op(5, 0, 2)]
        void JumpIfNonZero(List<ArgType> args) => ip = Value(ip+1, args[1]) != 0 ? Value(ip+2, args[2]) : ip + 3;
        [Op(6, 0, 2)]
        void JumpIfZero(List<ArgType> args) => ip = Value(ip+1, args[1]) == 0 ? Value(ip+2, args[2]) : ip + 3;
        [Op(7, 4, 3)]
        void LessThan(List<ArgType> args) => AssignValue(ip+3, args[3], Value(ip+1, args[1]) < Value(ip+2, args[2]) ? 1 : 0);
        [Op(8, 4, 3)]
        void Equals(List<ArgType> args) => AssignValue(ip+3, args[3], Value(ip+1, args[1]) == Value(ip+2, args[2]) ? 1 : 0);
        [Op(9, 2, 1)]
        void RelativeBase(List<ArgType> args) => relativeBase += Value(ip+1, args[1]);

        [Op(99, 1, 0)]
        void Halt(List<ArgType> args) => this.CurrentState = State.Halted;

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
            var opCode = (int)(memory[ip] % 100);
            var flags = (int)(memory[ip] / 100);
            var opInfo = opCache[opCode];

            var args = GetArgTypes(opInfo.attribute, flags);

            if (opCode == 3 && !input.Any())
            {
                this.CurrentState = State.BlockedOnInput;
                return;
            }

            opInfo.method.Invoke(this, new Object[] { args });
            ip += opInfo.attribute.OpSize;
        }

        class OpAttribute : Attribute
        {
            public int OpCode;
            public int OpSize;
            public int ArgCount;
            public OpAttribute(int opCode, int opSize, int argCount)
            {
                OpCode = opCode;
                OpSize = opSize;
                ArgCount = argCount;
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

        void POST()
        {
            var io_and_comp = "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99";
            /* The above example program uses an input instruction to ask for a single number.
               The program will then output 999 if the input value is below 8,
               output 1000 if the input value is equal to 8,
               or output 1001 if the input value is greater than 8. */
            SetMemory(io_and_comp);

            Reboot();
            Input(7);
            this.Run();
            Debug.Assert(this.Output() == 999);

            Reboot();
            Input(8);
            this.Run();
            Debug.Assert(this.Output() == 1000);

            Reboot();
            Input(9);
            this.Run();
            Debug.Assert(this.Output() == 1001);

            // should output the large number in the middle
            var bigNum = "104,1125899906842624,99";
            var bigNumValue = 1125899906842624;
            SetMemory(bigNum);
            Reboot();
            this.Run();
            Debug.Assert(this.Output() == bigNumValue);

            // should output a 16-digit number.
            SetMemory("1102,34915192,34915192,7,4,7,99,0");
            Reboot();
            this.Run();
            Debug.Assert(this.Output().ToString().Length == 16);

            var quine = "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99";
            SetMemory(quine);
            Reboot();
            this.Run();
            var output = string.Join(",", this.AllOutput);
            Debug.Assert(quine == output);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var computer = new IntcodeComputer(System.IO.File.ReadAllText("input.txt").Trim());
            computer.Input(1);
            computer.Run();
            Console.WriteLine(computer.Output());
        }
    }
}
