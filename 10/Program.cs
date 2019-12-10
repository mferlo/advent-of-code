using System;
using System.Collections.Generic;
using System.Linq;

namespace _10
{
    struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y) => (X, Y) = (x, y);
        public override string ToString() => $"({X}, {Y})";

        private int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            return a == 0 ? b : a;
        }

        public Point GetVector(Point to)
        {
            var deltaX = to.X - this.X;
            var deltaY = to.Y - this.Y;

            var gcd = GCD(Math.Abs(deltaX), Math.Abs(deltaY));
            return new Point(deltaX / gcd, deltaY / gcd);
        }

        public Point Add(Point delta) =>
            new Point(this.X + delta.X, this.Y + delta.Y);
        
        public bool InBounds(int maxX, int maxY) =>
            this.X >= 0 && this.X <= maxX && this.Y >= 0 && this.Y <= maxY;

        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Point a, Point b) => !(a == b);
    }

    class AsteroidField
    {
        public List<Point> Asteroids;
        public int MaxX;
        public int MaxY;

        IEnumerable<Point> FindShadowedPoints(Point origin, Point target)
        {
            if (origin == target)
            {
                yield return origin;
                yield break;
            }

            var delta = origin.GetVector(to: target);
            var shadow = target.Add(delta);
            while (shadow.InBounds(MaxX, MaxY))
            {
                yield return shadow;
                shadow = shadow.Add(delta);
            }
        }

        public int CountVisibleFrom(Point asteroid)
        {
            var shadowedPoints = Asteroids.SelectMany(a => FindShadowedPoints(asteroid, a)).ToHashSet();
            var result = Asteroids.Except(shadowedPoints);
            return result.Count();
        }
    }

    class Program
    {
        static IEnumerable<(Point point, int visible)> AnalyzeLocations(AsteroidField asteroidField)
        {
            foreach (var asteroid in asteroidField.Asteroids)
            {
                yield return (asteroid, asteroidField.CountVisibleFrom(asteroid));
            }
        }

        static IEnumerable<Point> FindAsteroids(List<string> lines, int maxX, int maxY)
        {
            for (int x = 0; x <= maxX; x++)
            {
                for (int y = 0; y <= maxY; y++)
                {
                    if (lines[y][x] == '#')
                    {
                        yield return new Point(x, y);
                    }
                }
            }
        }

        static AsteroidField ReadFile(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename).Where(line => !String.IsNullOrEmpty(line)).ToList();
            var maxX = lines[0].Length - 1;
            var maxY = lines.Count - 1;
            return new AsteroidField
            {
                Asteroids = FindAsteroids(lines, maxX, maxY).ToList(),
                MaxX = maxX,
                MaxY = maxY
            };
        }

        static void Main(string[] args)
        {
            Console.WriteLine(AnalyzeLocations(ReadFile("input.txt")).Max(x => x.visible));            
        }
    }
}
