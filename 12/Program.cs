using System;
using System.Collections.Generic;
using System.Linq;

namespace _12
{
    class Moon
    {
        public int pX;
        public int pY;
        public int pZ;

        public int vX;
        public int vY;
        public int vZ;

        public Moon((int x, int y, int z) position)
        {
            (pX, pY, pZ) = position;
            (vX, vY, vZ) = (0, 0, 0);
        }

        public void ApplyVelocity()
        {
            pX += vX;
            pY += vY;
            pZ += vZ;
        }

        public override string ToString() => $"({pX}, {pY}, {pZ}), ({vX}, {vY}, {vZ})";

        public int Energy => PotentialEnergy * KineticEnergy;

        int PotentialEnergy => Math.Abs(pX) + Math.Abs(pY) + Math.Abs(pZ);
        int KineticEnergy => Math.Abs(vX) + Math.Abs(vY) + Math.Abs(vZ);
    }

    class Space
    {
        List<(int x, int y, int z)> InitialState;
        List<Moon> Moons;

        public Space(IEnumerable<(int x, int y, int z)> moons)
        {
            InitialState = moons.ToList();
            Reset();            
        }

        void Reset() => Moons = InitialState.Select(p => new Moon(p)).ToList();

        public int Energy => Moons.Sum(m => m.Energy);

        public void Step()
        {
            ApplyGravity();
            ApplyVelocity();
        }

        public override string ToString() => String.Join("\n", Moons);

        // To apply gravity, consider every pair of moons
        void ApplyGravity()
        {
            for (var i = 0; i < Moons.Count; i++)
                for (var j = i + 1; j < Moons.Count; j++)
                    ApplyGravityPairwise(Moons[i], Moons[j]);
        }

        // On each axis (x, y, and z), the velocity of each moon changes by exactly +1 or -1 to pull the moons together
        // For example, if Ganymede has an x position of 3, and Callisto has a x position of 5,
        // then Ganymede's x velocity changes by +1 (because 5 > 3)
        // and Callisto's x velocity changes by -1 (because 3 < 5).
        // However, if the positions on a given axis are the same, the velocity on that axis does not change for that pair of moons.
        static void ApplyGravityPairwise(Moon m1, Moon m2)
        {
            var x = GravityAdjustmentForAxis(m1.pX, m2.pX);
            var y = GravityAdjustmentForAxis(m1.pY, m2.pY);
            var z = GravityAdjustmentForAxis(m1.pZ, m2.pZ);

            m1.vX += x.v1;
            m1.vY += y.v1;
            m1.vZ += z.v1;

            m2.vX += x.v2;
            m2.vY += y.v2;
            m2.vZ += z.v2;
        }

        static (int v1, int v2) GravityAdjustmentForAxis(int p1, int p2) =>
            p1 == p2 ? (0, 0) : p1 > p2 ? (-1, 1): (1, -1);

        void ApplyVelocity() => Moons.ForEach(m => m.ApplyVelocity());

        public (int x, int y, int z) FindPeriods() =>
            (FindPeriod(m => m.pX, m => m.vX),
             FindPeriod(m => m.pY, m => m.vY),
             FindPeriod(m => m.pZ, m => m.vZ));

        int FindPeriod(Func<Moon, int> p, Func<Moon, int> v)
        {
            string State() => String.Join(" ", Moons.Select(m => $"{p(m)}/{v(m)}"));

            Reset();
            var target = State();
            var period = 0;

            do
            {
                period++;
                Step();
            }
            while (State() != target);

            return period;
        }
    }

    class Program
    {
        static List<(int, int, int)> input = new List<(int, int, int)>
        {
            (13, -13, -2),
            (16, 2, -15),
            (7, -18, -12),
            (-3, -8, -8)
        };

        static List<(int, int, int)> test = new List<(int, int, int)>
        {
            (-1, 0, 2),
            (2, -10, -7),
            (4, -8, 8),
            (3, 5, -1),
        };

        // https://stackoverflow.com/a/29717490
        static long lcm((int x, int y, int z) nums) => lcm(lcm(nums.x, nums.y), nums.z);
        static long lcm(long a, long b) => Math.Abs(a * b) / GCD(a, b);
        static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);

        static void Main(string[] args)
        {
            var space = new Space(input);
            for (var i = 1; i <= 1000; i++)
            {
                space.Step();
            }
            Console.WriteLine(space.Energy);

            var part2 = space.FindPeriods();
            Console.WriteLine(part2 + " " + lcm(part2));
        }
    }
}

