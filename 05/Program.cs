using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _05
{
        class IntcodeComputer
    {
        int[] memory;
        bool done;
        int ip;
        Dictionary<int, (MethodInfo method, OpAttribute attribute)> opCache;

        public IntcodeComputer(string input)
        {
            memory = input.Split(",").Select(Int32.Parse).ToArray();
            ip = 0;
            done = false;
            PrimeCache();
        }

        public int this[int i] { get => this.memory[i]; set => this.memory[i] = value; }
        public override string ToString() => $"[{ip}] {string.Join(", ", memory)}";

        [Op(1, 4)]
        void Op1() => this[this[ip+3]] = this[this[ip+1]] + this[this[ip+2]];
        [Op(2, 4)]
        void Op2() => this[this[ip+3]] = this[this[ip+1]] * this[this[ip+2]];
        [Op(99, 1)]
        void Op99() => this.done = true;

        public void Run()
        {
            var noArgs = new object[0];
            while (!done)
            {
                var opInfo = opCache[this[ip]];
                opInfo.method.Invoke(this, noArgs);
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
            Console.WriteLine("Hello World!");
        }
    }
}
