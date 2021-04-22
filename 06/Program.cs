using System;
using System.Collections.Generic;
using System.Linq;

namespace _06
{
    class OrbitMap
    {
        class SpaceObject
        {
            public string Name;
            public List<SpaceObject> OrbitedBy = new List<SpaceObject>();
            public HashSet<string> Children;
            
            public SpaceObject(string name) => Name = name;

            public int Part1(int depth) => depth + OrbitedBy.Sum(ob => ob.Part1(depth + 1));

            public void IdentifyAllChildren()
            {
                foreach (var child in OrbitedBy)
                {
                    child.IdentifyAllChildren();
                }

                Children = new HashSet<string>(OrbitedBy.Select(ob => ob.Name).Concat(OrbitedBy.SelectMany(ob => ob.Children)));
            }
        }

        SpaceObject CenterOfMass;

        public OrbitMap()
        {
            var lines = System.IO.File.ReadAllLines("input.txt");
            var orbits = lines.Select(line => line.Split(')')).ToLookup(x => x[0], x => x[1]);

            CenterOfMass = new SpaceObject("COM");
            var queue = new Queue<SpaceObject>();
            queue.Enqueue(CenterOfMass);

            while (queue.Any())
            {
                var currentObject = queue.Dequeue();
                foreach (var childObject in orbits[currentObject.Name].Select(c => new SpaceObject(c)))
                {
                    currentObject.OrbitedBy.Add(childObject);
                    queue.Enqueue(childObject);
                }
            }
        }

        public int Part1() => CenterOfMass.Part1(0);

        public int Part2()
        {
            CenterOfMass.IdentifyAllChildren();
            var pathToSanta = FindPathToObjectOrbitedBy("SAN").ToList();
            var pathToYou = FindPathToObjectOrbitedBy("YOU").ToList();

            var indexAfterPathsDiverge = 0;
            while (pathToYou[indexAfterPathsDiverge] == pathToSanta[indexAfterPathsDiverge])
            {
                indexAfterPathsDiverge++;
            }

            return pathToSanta.Count() + pathToYou.Count() - 2 * indexAfterPathsDiverge;
        }

        private IEnumerable<SpaceObject> FindPathToObjectOrbitedBy(string target)
        {
            var cur = CenterOfMass;
            while (cur.Children.Contains(target))
            {
                yield return cur;
                if (cur.OrbitedBy.Any(c => c.Name == target))
                {
                    yield break; // Stop before we actually reach the target itself
                }
                cur = cur.OrbitedBy.Single(ob => ob.Children.Contains(target));
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var map = new OrbitMap();

            Console.WriteLine(map.Part1());
            Console.WriteLine(map.Part2());
        }
    }
}
