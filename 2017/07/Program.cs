using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace _07 {

    using Nodes = Dictionary<string, Node>;

    class Node {
        public string Id;
        public int SelfWeight;
        public int TotalWeight;
        public IList<string> Children;

        public override string ToString() => $"{Id}";
    }

    class Program {

    static Nodes GetNodes() {
        var re = new Regex(@"^([a-z]+) \((\d+)\)( -> ([a-z,].*))?$");
        var comma = new[] { ", " };
        var nodes = new Nodes();
        foreach (var line in File.ReadAllLines("input")) {
            var g = re.Match(line).Groups;
            var n = new Node {
                Id = g[1].Value,
                SelfWeight = Int32.Parse(g[2].Value),
                Children = g[4].Value.Split(comma, StringSplitOptions.RemoveEmptyEntries)
            };
            nodes[n.Id] = n;
        }
        return nodes;
    }

    static Node FindRoot(Nodes nodes) {
        var children = new HashSet<string>(nodes.Values.SelectMany(n => n.Children));
        return nodes.Values.Single(n => !children.Contains(n.Id));
    }

    static void PopulateWeights(Nodes nodes, Node current) {
        foreach (var child in current.Children) {
            PopulateWeights(nodes, nodes[child]);
        }

        current.TotalWeight =
            current.SelfWeight +
            current.Children.Sum(c => nodes[c].TotalWeight);
    }

    static bool PrintBadWeight(Nodes nodes, Node current) {
        if (current.Children.Count == 0) {
            // My total weight is off, and I don't have any kids.
            // It's all my fault.
            return true;
        }

        if (current.Children.Count == 1 || current.Children.Count == 2) {
            throw new Exception("I'm assuming this can't happen");
        }

        var childWeights = current.Children
            .Select(c => nodes[c].TotalWeight)
            .OrderBy(x => x)
            .ToList();

        var low = childWeights[0];
        var high = childWeights[childWeights.Count - 1];
        if (low == high) {
            // My kids are well-balanced, it's me that's the problem.
            // Parent will calc diff between me and their normal kids
            return true; 
        }

        int oddWeightOut;
        if (low == childWeights[1]) {
            oddWeightOut = high;
        } else if (high == childWeights[childWeights.Count - 2]) {
            oddWeightOut = low;
        } else {
            throw new Exception("I'm assuming this can't happen");
        }

        var maybeBadNode = nodes[current.Children.Single(c => nodes[c].TotalWeight == oddWeightOut)];

        if (PrintBadWeight(nodes, maybeBadNode)) {
            var good = childWeights.First(cw => cw != oddWeightOut);
            var delta = good - oddWeightOut;
            var desiredSelfWeight = maybeBadNode.SelfWeight + delta;
            Console.WriteLine($"Part 2: {desiredSelfWeight}");
        }
        
        return false;
    }

    static void Main(string[] args)
    {
        var nodes = GetNodes();
        var root = FindRoot(nodes);
        Console.WriteLine($"Part 1: {root}");

        PopulateWeights(nodes, root);
        PrintBadWeight(nodes, root);
    }
}}
