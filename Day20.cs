using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    enum Edge { Top, Right, Bottom, Left }

    // Edge values are computed clockwise.
    // If (edge1 == edge2.Reverse), they will mesh using only rotations
    // If (edge1 == edge2), they will mesh with a flip and rotations
    struct Edges
    {
        string Top, Right, Bottom, Left;

        private Edges(IEnumerable<char> top, IEnumerable<char> right, IEnumerable<char> bottom, IEnumerable<char> left)
        {
            Top = new string(top.ToArray());
            Right = new string(right.ToArray());
            Bottom = new string(bottom.ToArray());
            Left = new string(left.ToArray());
        }

        public static Edges FromImage(List<string> image)
        {
            return new Edges(
                top: Enumerable.Range(0, 10).Select(i => image[0][i]),
                right: Enumerable.Range(0, 10).Select(i => image[i][9]),
                bottom: Enumerable.Range(0, 10).Reverse().Select(i => image[9][i]),
                left: Enumerable.Range(0, 10).Reverse().Select(i => image[i][0])
            );
        }

        static string Reverse(string s) => new string(s.Reverse().ToArray());

        IEnumerable<(Edge ThisEdge, Edge OtherEdge, bool Flip)> Matches(string otherValue, Edge otherEdge)
        {
            var rev = Reverse(otherValue);

            if (Top == otherValue) yield return (Edge.Top, otherEdge, false);
            if (Right == otherValue) yield return (Edge.Right, otherEdge, false);
            if (Bottom == otherValue) yield return (Edge.Bottom, otherEdge, false);
            if (Left == otherValue) yield return (Edge.Left, otherEdge, false);
            if (Top == rev) yield return (Edge.Top, otherEdge, true);
            if (Right == rev) yield return (Edge.Right, otherEdge, true);
            if (Bottom == rev) yield return (Edge.Bottom, otherEdge, true);
            if (Left == rev) yield return (Edge.Left, otherEdge, true);
        }

        public IEnumerable<(Edge ThisEdge, Edge OtherEdge, bool Flip)> Matches(Edges other) =>
            Matches(other.Top, Edge.Top)
                .Concat(Matches(other.Right, Edge.Right))
                .Concat(Matches(other.Bottom, Edge.Bottom))
                .Concat(Matches(other.Left, Edge.Left));

        public Edges RotateRight() => new Edges(Left, Top, Right, Bottom);
        public Edges Rotate180() => new Edges(Bottom, Left, Top, Right);
        public Edges RotateLeft() => new Edges(Right, Bottom, Left, Top);
        public Edges FlipHorizontal() => new Edges(Reverse(Top), Reverse(Left), Reverse(Bottom), Reverse(Right));
    }

    class Tile
    {
        public long Id { get; }
        public List<string> Image { get; private set; }
        public Edges Edges { get; private set; }

        public Tile AdjacentTop { get; set; }
        public Tile AdjacentRight { get; set; }
        public Tile AdjacentBottom { get; set; }
        public Tile AdjacentLeft { get; set; }

        bool HasAdjacentTileSet => AdjacentTop != null || AdjacentRight != null || AdjacentBottom != null || AdjacentLeft != null;

        public Tile(IEnumerable<string> lines)
        {
            Debug.Assert(lines.Count() == 11);
            var first = lines.First();
            Debug.Assert(first.Length == 10);
            Id = long.Parse(first.Substring(5, 4));

            Image = lines.Skip(1).ToList();
            Edges = Edges.FromImage(Image);
        }

        public IEnumerable<(Edge ThisEdge, Edge OtherEdge, bool Flip)> Matches(Tile other) => Edges.Matches(other.Edges);

        public void RotateRight()
        {
            if (HasAdjacentTileSet)
            {
                throw new Exception("Cannot rotate once an adjacent tile is specified");
            }

            var newImage = new List<string>();
            Image.Reverse();
            for (var i = 0; i < 10; i++)
            {
                newImage.Add(new String(Image.Select(line => line[i]).ToArray()));
            }
            Image = newImage;
            Edges = Edges.RotateRight();
        }

        public void Rotate180()
        {
            RotateRight();
            RotateRight();
        }

        public void RotateLeft()
        {
            RotateRight();
            RotateRight();
            RotateRight();
        }

        public void FlipHorizontal()
        {
            if (HasAdjacentTileSet)
            {
                throw new Exception("Cannot flip once an adjacent tile is specified");
            }

            Image = Image.Select(line => new String(line.Reverse().ToArray())).ToList();
            Edges = Edges.FlipHorizontal();
        }
    }

    class Day20
    {
        static List<Tile> Tiles;

        static void Parse(string fileName)
        {
            Tiles = new List<Tile>();

            IEnumerable<string> lines = File.ReadAllLines(fileName);
            while (lines.Count() >= 11)
            {
                Tiles.Add(new Tile(lines.Take(11)));
                lines = lines.Skip(12);
            }
        }

        public static void Parse() => Parse("20.txt");

        static List<Tile> FindNeighbors(Tile tile) =>
            Tiles.Where(t => t != tile && tile.Matches(t).Any()).ToList();

        static IEnumerable<Tile> FindCorners()
        {
            foreach (var tile in Tiles)
            {
                var neighbors = FindNeighbors(tile);
                if (FindNeighbors(tile).Count() == 2)
                    yield return tile;
            }
        }

        static long CornerProduct() => FindCorners().Product(tile => tile.Id);

        public static void Test()
        {
            Parse("20.test.txt");
            Console.WriteLine(CornerProduct());
        }

        public static object Part1() => CornerProduct();

        public static object Part2()
        {
            return Tiles;
        }
    }
}