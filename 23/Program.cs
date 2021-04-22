﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _23
{
    struct Position
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Position(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int Distance(Position other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
    }

    struct Nanobot
    {
        private static readonly Regex parseInput = new Regex(@"pos=<(-?\d+),(-?\d+),(-?\d+)>, r=(\d+)");

        public Position Position { get; }
        public int R { get; }

        public Nanobot(string s)
        {
            var values = parseInput.Match(s).Groups.Skip(1).Select(x => int.Parse(x.Value)).ToList();
            Position = new Position(values[0], values[1], values[2]);
            R = values[3];
        }

        public bool InRange(Position other) => Position.Distance(other) <= R;

        public bool InRange(Nanobot other) => InRange(other.Position);
    }

    class Program
    {
        static void Main(string[] args)
        {
            // var bots = test.Split("\r\n").Select(line => new Nanobot(line)).ToList();
            var bots = System.IO.File.ReadLines("input").Select(line => new Nanobot(line)).ToList();
            var strongest = bots.OrderByDescending(b => b.R).First();

            // Part 1
            // Console.WriteLine(bots.Count(b => strongest.InRange(b)));
        }

        const string test = @"pos=<0,0,0>, r=4
pos=<1,0,0>, r=1
pos=<4,0,0>, r=3
pos=<0,2,0>, r=1
pos=<0,5,0>, r=3
pos=<0,0,3>, r=1
pos=<1,1,1>, r=1
pos=<1,1,2>, r=1
pos=<1,3,1>, r=1";
    }
}
