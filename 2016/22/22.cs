using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

// The initial empty one ('_') has 0 usage
// The blockers ('#') have > 90T usage
// Others are free ('.')

/*
First, get the empty space from its initial starting point,
around the immovable wall, and up next to the target data:

*.........8901234567890123456789G
..........7......................
..........6......................
..........5......................
..........4......................
..........3......................
..........2......................
..........1......................
..........0......................
..........9######################
..........8......................
..........7......................
..........6543210987654321_......
.................................
...etc

The 49th move has left us in this state for the top 2 rows:

*.............................._G
.................................

We move G one step to the left. This takes 5 moves:

init   1    2    3    4    5
._G  .G_  .Gc  .Gc  .Gc  _Gc
abc  abc  ab_  a_b  _ab  .ab

After repeating this process 31 times, the top-left looks like this:

_G..
....

Total moves: 49 + 31*5 = 204.

And the last move to put G in the upper left => 205 moves.
*/

namespace Advent { class Problem22_Visualization {

    static Regex r = new Regex(
        @"^/dev/grid/node-x(\d+)-y(\d+)\s+\d+T\s+(\d+)T\s+\d+T\s+\d+%$");

    static void ParseAndPrint(string[] lines) {
        Console.WriteLine(lines.Length);
        var grid = new char[33,30];
        foreach (var line in lines.Skip(2)) {
            var m = r.Match(line);
            var x = Int32.Parse(m.Groups[1].Value);
            var y = Int32.Parse(m.Groups[2].Value);
            var used = Int32.Parse(m.Groups[3].Value);

            var status = used == 0
                ? '@'
                : used > 90 ? '#' : '.';

            grid[x,y] = status;
        }

        for (int y = 0; y < 30; y++) {
            for (int x = 0; x < 33; x++) {
                Console.Write(grid[x,y]);
            }
            Console.WriteLine();
        }
    }

    static void Main() {
        ParseAndPrint(System.IO.File.ReadAllLines("input.txt"));
    }
}}