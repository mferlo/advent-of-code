using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    class Day03
    {
        class Terrain
        {
            public int Height;

            int Width;
            List<string> Lines;

            public static Terrain Parse(IList<string> lines) =>
                new Terrain
                {
                    Height = lines.Count,
                    Width = lines[0].Length,
                    Lines = lines.ToList()
                };

            public bool IsTree(int x, int y) =>
                Lines[y][x % Width] == '#';
        }

        static Terrain Input;

        public static void Parse() =>
            Input = Terrain.Parse(File.ReadAllLines("03.txt"));

        static long TreesEncounteredWithSlope(int deltaX, int deltaY)
        {
            long result = 0;
            var x = 0;
            for (var y = 0; y < Input.Height; y += deltaY, x += deltaX)
            {
                if (Input.IsTree(x, y))
                {
                    result++;
                }
            }
            return result;
        }

        public static object Part1() =>
            TreesEncounteredWithSlope(3, 1);

        public static object Part2() =>
            TreesEncounteredWithSlope(1, 1) *
            TreesEncounteredWithSlope(3, 1) *
            TreesEncounteredWithSlope(5, 1) *
            TreesEncounteredWithSlope(7, 1) *
            TreesEncounteredWithSlope(1, 2);
    }
}