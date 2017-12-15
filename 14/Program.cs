using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _14 {

    class KnotHash {
        private int[] values;
        private int current;
        private int skip;
        private int length;

        public KnotHash(int length) {
            this.current = 0;
            this.skip = 0;
            this.length = length;
            this.values = new int[length];
            for (var i = 0; i < length; i++) {
                this.values[i] = i;
            }
        }

        private void Reverse(int lengthToReverse) {
            for (var i = 0; i < lengthToReverse / 2; i++) {
                var j = (current + i) % length;
                var k = (current + lengthToReverse - i - 1) % length;
                var tmp = values[j];
                values[j] = values[k];
                values[k] = tmp;
            }
        }

        public void ProcessInput(IEnumerable<int> inputs) {
            foreach (var input in inputs) {
                Reverse(input);
                current = (current + input + skip) % length;
                skip++;
            }
        }

        public void ProcessInput(string input) {
            var suffix = new[] { 17, 31, 73, 47, 23 };
            var numericInput = input.Select(ch => (int)ch).Concat(suffix).ToList();
            for (var i = 0; i < 64; i++) {
                ProcessInput(numericInput);
            }
        }

        private IEnumerable<int> GetBatches() {
            for (var i = 0; i < length; i += 16) {
                yield return values.Skip(i).Take(16).Aggregate((x, y) => x ^ y);
            }
        }

        public string GetHash() {
            return String.Join("", GetBatches().Select(x => x.ToString("x2")));
        }

        public IList<bool> GetBits() {
            var result = new bool[128];
            var i = 0;
            foreach (var val in GetBatches()) {
                var copy = val;
                for (int j = 7; j >= 0; j--) {
                    result[i + j] = (copy & 1) == 1;
                    copy >>= 1;
                }
                i += 8;
            }
            return result;
        }
    }

    class Program {

        static bool[,] GetBits() {
            var result = new bool[128,128];
            for (var i = 0; i < 128; i++) {
                var hasher = new KnotHash(256);
                hasher.ProcessInput("nbysizxe-" + i);
                var bits = hasher.GetBits();
                for (var j = 0; j < 128; j++) {
                    result[i, j] = bits[j];
                }
            }
            return result;
        }

        static void Main(string[] args) {
            var bits = GetBits();
            var part1 = bits.Cast<bool>().Count(b => b);
            Console.WriteLine(part1);
        }
    }
}
