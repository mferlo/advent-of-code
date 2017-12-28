using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace _24 {

    struct Component {
        public int p1 { get; }
        public int p2 { get; }

        public Component(int p1, int p2) {
            this.p1 = p1;
            this.p2 = p2;
        }

        public int Strength => p1 + p2;
        public override string ToString() => $"{p1}/{p2}";
    }

    struct Bridge {
        public ImmutableList<Component> Components { get; }
        public int ExposedPort { get; }

        private Bridge(IEnumerable<Component> components) {
            Components = ImmutableList<Component>.Empty.AddRange(components);

            var exposedPort = 0;
            foreach (var c in Components) {
                if (exposedPort == c.p1) {
                    exposedPort = c.p2;
                } else if (exposedPort == c.p2) {
                    exposedPort = c.p1;
                } else {
                    throw new Exception($"Can't add {c} to {String.Join(", ", components)}");
                }
            }
            ExposedPort = exposedPort;
        }

        public Bridge Add(Component c) {
            return new Bridge(this.Components.Add(c));
        }

        public int Strength => Components.Sum(c => c.Strength);
        public static Bridge Empty => new Bridge(ImmutableList<Component>.Empty);
        public override string ToString() => $"{String.Join(", ", Components)} -> {ExposedPort}";

    }

    class Program {
        const string test = "0/2\n2/2\n2/3\n3/4\n3/5\n0/1\n10/1\n9/10";

        static Component C(int x, int y) => new Component(x, y);

        static void Main(string[] args) {
            var bridge = Bridge.Empty;

            bridge = bridge.Add(C(0, 1)).Add(C(10, 1)).Add(C(9, 10));

            Console.WriteLine(bridge);

            Console.WriteLine(bridge.Strength);
        }
    }
}
