using System;
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

        public string GetHash() {
            var result = "";
            for (var i = 0; i < length; i += 16) {
                result += values.Skip(i).Take(16).Aggregate((x, y) => x ^ y).ToString("x2");
            }
            return result;
        }

        public int Day10Part1 => values[0] * values[1];
    }

    class Program {
        static void Main(string[] args) {
            var hasher = new KnotHash(256);
            hasher.ProcessInput("102,255,99,252,200,24,219,57,103,2,226,254,1,0,69,216");
            Console.WriteLine(hasher.GetHash());
        }
    }
}
