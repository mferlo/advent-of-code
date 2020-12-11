using System;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day11
    {
        static char[,] Input;
        static int Width;
        static int Height;

        public static void Parse()
        {
            var data = File.ReadAllLines("11.txt");
            Width = data.First().Length;
            Height = data.Length;

            Input = new char[Width, Height];
            for (var y = 0; y < Height; y++)
            {
                var line = data[y];
                for (var x = 0; x < Width; x++)
                {
                    Input[x,y] = line[x];
                }
            }
        }

        static int OccupiedSeat(char[,] grid, int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return 0;
            else
                return grid[x,y] == '#' ? 1 : 0;
        }

        static int OccupiedNeighborSeats(char[,] grid, int x, int y) =>
            OccupiedSeat(grid, x - 1, y - 1) +
            OccupiedSeat(grid, x - 1, y) +
            OccupiedSeat(grid, x - 1, y + 1) +
            OccupiedSeat(grid, x, y - 1) +
            OccupiedSeat(grid, x, y + 1) +
            OccupiedSeat(grid, x + 1, y - 1) +
            OccupiedSeat(grid, x + 1, y) +
            OccupiedSeat(grid, x + 1, y + 1);

        static char[,] IteratePart1(char[,] current)
        {
            var next = new char[Width, Height];
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    char nextChar;
                    if (current[x,y] == '.')
                    {
                        nextChar = '.';
                    }
                    else if (current[x,y] == 'L')
                    {
                        nextChar = OccupiedNeighborSeats(current, x, y) == 0 ? '#' : 'L';                    
                    }
                    else
                    {
                        nextChar = OccupiedNeighborSeats(current, x, y) >= 4 ? 'L' : '#';
                    }
                    next[x,y] = nextChar;
                }
            }
            return next;
        }

        static int VisibleSeat(char[,] grid, int x, int y, int dx, int dy)
        {
            x += dx;
            y += dy;
            while (0 <= x && x < Width && 0 <= y && y < Height)
            {
                if (grid[x,y] == 'L')
                    return 0;
                else if (grid[x,y] == '#')
                    return 1;

                x += dx;
                y += dy;
            }
            return 0;
        }

        static int OccupiedVisibleSeats(char[,] grid, int x, int y) =>
            VisibleSeat(grid, x, y, -1, -1) +
            VisibleSeat(grid, x, y, -1, 0) +
            VisibleSeat(grid, x, y, -1, 1) +
            VisibleSeat(grid, x, y, 0, -1) +
            VisibleSeat(grid, x, y, 0, 1) +
            VisibleSeat(grid, x, y, 1, -1) +
            VisibleSeat(grid, x, y, 1, 0) +
            VisibleSeat(grid, x, y, 1, 1);


        static char[,] IteratePart2(char[,] current)
        {
            var next = new char[Width, Height];
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    char nextChar;
                    if (current[x,y] == '.')
                    {
                        nextChar = '.';
                    }
                    else if (current[x,y] == 'L')
                    {
                        nextChar = OccupiedVisibleSeats(current, x, y) == 0 ? '#' : 'L';                    
                    }
                    else
                    {
                        nextChar = OccupiedVisibleSeats(current, x, y) >= 5 ? 'L' : '#';
                    }
                    next[x,y] = nextChar;
                }
            }
            return next;
        }

        static bool Equal(char[,] a, char[,] b) => a.Cast<char>().SequenceEqual(b.Cast<char>());

        static char[,] IterateToEquilibrium(Func<char[,], char[,]> iterate)
        {
            var current = Input;
            var next = iterate(current);
            while (!Equal(current, next))
            {
                current = next;
                next = iterate(current);
            }
            return next;
        }

        public static object Part1() =>
            IterateToEquilibrium(IteratePart1).Cast<char>().Count(c => c == '#');

        public static object Part2() =>
            IterateToEquilibrium(IteratePart2).Cast<char>().Count(c => c == '#');
    }
}