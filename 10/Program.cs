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

        // We want angles *clockwise* from "up"; normal angle measurement
        // is *counterclockwise* from "right".
        // We can make this work by mirroring across the y=x line;
        // which just means swapping the x and y args to Atan2.
        // Since  our Y-axis is inverted compared to normal cartesian,
        // we also negate our Y value
        public double GetAngleTo(Point to)
        {
            var result = Math.Atan2(to.X - X, -(to.Y - Y));
            return result >= 0 ? result : result + 2*Math.PI;
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

        IEnumerable<Point> GetVisiblePoints(Point asteroid) =>
            Asteroids.Except(Asteroids.SelectMany(a => FindShadowedPoints(asteroid, a)).ToHashSet());

        public int CountVisibleFrom(Point asteroid) =>
            GetVisiblePoints(asteroid).Count();

        public IEnumerable<Point> FireTheLaser(Point origin)
        {
            while (Asteroids.Count > 1)
            {
                var targets = GetVisiblePoints(origin).OrderBy(a => origin.GetAngleTo(a));
                foreach (var target in targets)
                {
                    yield return target;
                    Asteroids.Remove(target);
                }
            }
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
            var asteroidField = ReadFile("input.txt");
            (Point asteroid, int visible) monitoringStation = (default, 0);
            foreach (var candidate in AnalyzeLocations(asteroidField))
            {
                if (candidate.visible > monitoringStation.visible)
                {
                    monitoringStation = candidate;
                }
            }
            Console.WriteLine(monitoringStation);

            Console.WriteLine(asteroidField.FireTheLaser(monitoringStation.asteroid).ElementAt(199));
        }
    }
}
