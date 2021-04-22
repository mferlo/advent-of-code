using System;
using System.Linq;

namespace _01
{
    class Program
    {
        static int CalcFuel(int weight) => Math.Max(0, (weight / 3) - 2);

        static int CalcFuelRec(int weight) => CalcFuelRec(0, weight);

        static int CalcFuelRec(int total, int weight)
        {
            var f = CalcFuel(weight);
            return f == 0 ? total : CalcFuelRec(total + f, f);
        }

        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt").Select(Int32.Parse);
            Console.WriteLine(input.Sum(w => CalcFuel(w)));
            Console.WriteLine(input.Sum(w => CalcFuelRec(w)));
        }
    }
}
