using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace _24 {

    struct Component {
        public int p1 { get; }
        public int p2 { get; }

        public Component(string s) {
            var x = s.Split('/');
            this.p1 = Int32.Parse(x[0]);
            this.p2 = Int32.Parse(x[1]);
        }

        public int Strength => p1 + p2;
        public bool CanAttach(int port) => p1 == port || p2 == port;
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
        public int Length => Components.Count;
        public static Bridge Empty => new Bridge(ImmutableList<Component>.Empty);
        public override string ToString() => $"{Strength}: {String.Join(", ", Components)}";
    }

    class Program {
        const string test = "0/2\n2/2\n2/3\n3/4\n3/5\n0/1\n10/1\n9/10";

        static IImmutableSet<Component> Parse(string input) =>
            ImmutableHashSet<Component>.Empty
                .Union(input.Split('\n').Where(s => s != "").Select(line => new Component(line)));

        static IEnumerable<Component> AvailableComponents(Bridge b, IEnumerable<Component> components) =>
            components.Where(c => !b.Components.Contains(c)).Where(c => c.CanAttach(b.ExposedPort));

        static IEnumerable<Bridge> AddComponents(Bridge b, IEnumerable<Component> availableComponents) =>
            availableComponents.Select(c => b.Add(c));

        // Meh, good enough, but still kinda sad: "real 1m45.274s"
        static void Main(string[] args) {
            var strongest = Bridge.Empty;
            var longest = Bridge.Empty;

            var components = Parse(File.ReadAllText("input")); // Parse(test);
            var bridges = new Queue<Bridge>();
            bridges.Enqueue(Bridge.Empty);

            while (bridges.Any()) {
                var b = bridges.Dequeue();
                var available = AvailableComponents(b, components);
                if (available.Any()) {
                    foreach (var newBridge in AddComponents(b, available)) {
                        bridges.Enqueue(newBridge);
                    }
                } else {
                    if (strongest.Strength < b.Strength) {
                        strongest = b;
                    }

                    if (longest.Length < b.Length ||
                        (longest.Length == b.Length && longest.Strength < b.Strength)) {
                        longest = b;
                    }
                }
            }

            Console.WriteLine($"Strongest: {strongest}");
            Console.WriteLine($"Longest: {longest}");
        }
    }
}
