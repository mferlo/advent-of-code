using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day12
    {
        static List<string> Input;
        public static void Parse() => Input = File.ReadAllLines("12.txt").ToList();

        static (int x, int y)[] Facings = new[] { (x: 0, y: 1), (x: 1, y: 0), (x: 0, y: -1), (x: -1, y: 0)};

        static (int x, int y) Rotate((int x, int y) current, char direction, int amount)
        {
            var i = Array.IndexOf<(int x, int y)>(Facings, current);
            var amt = amount / 90;
            var dir = direction == 'R' ? 1 : -1;

            var newIndex = (i + 4 + dir * amt) % 4;
            return Facings[newIndex];
        }

        public static object Part1()
        {
            var x = 0;
            var y = 0;
            var facing = (x: 1, y: 0);

            foreach (var step in Input)            
            {
                var command = step[0];
                var amount = int.Parse(step.Substring(1));
                switch (command)
                {
                    case 'N': y += amount; break;
                    case 'S': y -= amount; break;
                    case 'E': x += amount; break;
                    case 'W': x -= amount; break;

                    case 'L':
                    case 'R': facing = Rotate(facing, command, amount); break;

                    case 'F':
                        x += facing.x * amount;
                        y += facing.y * amount;
                        break;
                    
                    default: throw new Exception(step);
                }
            }

            return Math.Abs(x) + Math.Abs(y);
        }

        static (int x, int y) RotateRight(int x, int y, int amount) =>
            amount switch
            {
                90 => (y, -x),
                180 => (-x, -y),
                270 => (-y, x),
                _ => throw new Exception()
            };

        public static object Part2()
        {
            // Relative to ship
            var x = 10;
            var y = 1;

            var ship = (x: 0, y: 0);

            foreach (var step in Input)            
            {
                var command = step[0];
                var amount = int.Parse(step.Substring(1));
                switch (command)
                {
                    case 'N': y += amount; break;
                    case 'S': y -= amount; break;
                    case 'E': x += amount; break;
                    case 'W': x -= amount; break;
                    case 'L': (x, y) = RotateRight(x, y, 360 - amount); break;
                    case 'R': (x, y) = RotateRight(x, y, amount); break;
                    case 'F': ship = (ship.x + amount * x, ship.y + amount * y); break;
                    default: throw new Exception(step);
                }
            }

            return Math.Abs(ship.x) + Math.Abs(ship.y);
        }
    }
}