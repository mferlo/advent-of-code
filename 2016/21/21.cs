using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent {
    class Problem21 {

        static Func<string, string> SwapLetter(char x, char y) {
            return s => s.Replace(x, '!').Replace(y, x).Replace('!', y);
        }

        static Func<string, string> SwapPosition(int x, int y) {
            return s => SwapLetter(s[x], s[y])(s);
        }

        static Func<string, string> Reverse(int x, int y) {
            return s => s.Substring(0, x)
                + String.Join("", s.Substring(x, y-x+1).Reverse())
                + (y > s.Length ? "" : s.Substring(y+1));
        }

        static Func<string, string> Move(int x, int y) {
            return s => s.Remove(x, 1).Insert(y, s.Substring(x, 1));
        }

        static Func<string, string> RotateLeft(int x) {
            return s => s.Substring(x) + s.Substring(0, x);
        }

        static Func<string, string> RotateRight(int x) {
            return s => s.Substring(s.Length - x) + s.Substring(0, s.Length - x);
        }

        static string Right1(string s) {
            return RotateRight(1)(s);
        }

        static Func<string, string> RotateBasedOn(char x) {
            // Once the index is determined, rotate the string to the right one time,
            // plus a number of times equal to that index, plus one additional time
            // if the index was at least 4.
            return s => {
                var i = s.IndexOf(x);
                s = RotateRight(i)(Right1(s));
                return i >= 4 ? Right1(s) : s;
            };
        }

        // Part 2 inverse
        //
        // Given the input is of fixed length (8), we can cheat a little here.
        //                    01234567
        // abcdefgh by 'a' => habcdefg. rotated pos: 1. LeftRotations to undo: 1
        // abcdefgh by 'b' => ghabcdef. rotated pos: 3. LeftRotations to undo: 2
        // abcdefgh by 'c' => fghabcde. rotated pos: 5. LeftRotations to undo: 3
        // abcdefgh by 'd' => efghabcd. rotated pos: 7. LeftRotations to undo: 4
        // abcdefgh by 'e' => cdefghab. rotated pos: 2. LeftRotations to undo: 6
        // abcdefgh by 'f' => bcdefgha. rotated pos: 4. LeftRotations to undo: 7
        // abcdefgh by 'g' => abcdefgh. rotated pos: 6. LeftRotations to undo: 0
        // abcdefgh by 'h' => habcdefg. rotated pos: 0. LeftRotations to undo: 1
        static int[] LeftRotationsToInvertBasedOnIndex = new[] { 1, 1, 6, 2, 7, 3, 0, 4 };

        static Func<string, string> InverseRotateBasedOn(char x) {
            return s => {
                var rotations = LeftRotationsToInvertBasedOnIndex[s.IndexOf(x)];
                return RotateLeft(rotations)(s);
            };
        }

        static char[] space = new[] { ' ' };
        static int i(string s) { return Int32.Parse(s); }

        static Func<string, string> ParseLine(string line) {
            var p = line.Split(space);
            if (p[0] == "move") {
                return Move(i(p[2]), i(p[5]));
            } else if (p[0] == "reverse") {
                return Reverse(i(p[2]), i(p[4]));
            } else if (p[0] == "swap") {
                return p[1] == "position"
                    ? SwapPosition(i(p[2]), i(p[5]))
                    : SwapLetter(p[2].First(), p[5].First());
            } else if (p[1] == "left") {
                return RotateLeft(i(p[2]));
            } else if (p[1] == "right") {
                return RotateRight(i(p[2]));
            } else { // "based on position of letter"
                return RotateBasedOn(line.Last());
            }
        }

        static Func<string, string> ParseLineInverted(string line) {
            var p = line.Split(space);
            if (p[0] == "move") {
                return Move(i(p[5]), i(p[2])); // invert args
            } else if (p[0] == "reverse") {
                return Reverse(i(p[2]), i(p[4])); // self-inverse
            } else if (p[0] == "swap") {
                return p[1] == "position"
                    ? SwapPosition(i(p[2]), i(p[5])) // self-inverse
                    : SwapLetter(p[2].First(), p[5].First()); // self-inverse
            } else if (p[1] == "left") {
                return RotateRight(i(p[2])); // swap direction
            } else if (p[1] == "right") {
                return RotateLeft(i(p[2])); // swap direction
            } else {
                return InverseRotateBasedOn(line.Last()); // special-case
            }
        }

        static IEnumerable<Func<string, string>> ParsePart1(string filename) {
            foreach (var line in File.ReadAllLines(filename)) {
                yield return ParseLine(line);
            }
        }                

        static IEnumerable<Func<string, string>> ParsePart2(string filename) {
            foreach (var line in File.ReadAllLines(filename).Reverse()) {
                yield return ParseLineInverted(line);
            }
        }                

        static void Main() {
            var p1 = ParsePart1("input.txt");
            var a1 = p1.Aggregate("abcdefgh", (s, f) => f(s));
            Console.WriteLine("Part 1: " + a1);

            // In retrospect, parsing into Functions (as opposed to iterating & calling
            // functions directly while parsing) was foolish, but at least it lets you
            // re-use them...
            var p2 = ParsePart2("input.txt");
            Console.WriteLine("Invert: " + p2.Aggregate(a1, (s, f) => f(s)));
            Console.WriteLine("Part 2: " + p2.Aggregate("fbgdceah", (s, f) => f(s)));
        }
    }
}
