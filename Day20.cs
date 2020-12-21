using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    enum Edge {Top = 1, Right = 2, Bottom = 4, Left = 8 }

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

        (Edge ThisEdge, Edge OtherEdge, bool Flip)? Match(string otherValue, Edge otherEdge)
        {
            if (Top == otherValue) return (Edge.Top, otherEdge, true);
            if (Right == otherValue) return (Edge.Right, otherEdge, true);
            if (Bottom == otherValue) return (Edge.Bottom, otherEdge, true);
            if (Left == otherValue) return (Edge.Left, otherEdge, true);

            var rev = Reverse(otherValue);
            if (Top == rev) return (Edge.Top, otherEdge, false);
            if (Right == rev) return (Edge.Right, otherEdge, false);
            if (Bottom == rev) return (Edge.Bottom, otherEdge, false);
            if (Left == rev) return (Edge.Left, otherEdge, false);

            return null;
        }

        public (Edge ThisEdge, Edge OtherEdge, bool Flip)? Match(Edges other) =>
            Match(other.Top, Edge.Top) ??
                Match(other.Right, Edge.Right) ??
                Match(other.Bottom, Edge.Bottom) ??
                Match(other.Left, Edge.Left);
    }

    class Tile
    {
        public long Id { get; }
        public List<string> Image { get; private set; }
        public Edges Edges { get; private set; }

        public bool IsSet { get; set; }

        public Tile(IEnumerable<string> lines)
        {
            Debug.Assert(lines.Count() == 11);
            var first = lines.First();
            Debug.Assert(first.Length == 10);
            Id = long.Parse(first.Substring(5, 4));

            Image = lines.Skip(1).ToList();
            Edges = Edges.FromImage(Image);
        }

        public override string ToString() => Id.ToString();

        public (Edge ThisEdge, Edge OtherEdge, bool Flip)? Match(Tile other) =>
            this == other ? null : Edges.Match(other.Edges);

        public void RotateRight()
        {
            if (IsSet)
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
            Edges = Edges.FromImage(Image);
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
            if (IsSet)
            {
                throw new Exception("Cannot flip once an adjacent tile is specified");
            }

            Image = Image.Select(line => new String(line.Reverse().ToArray())).ToList();
            Edges = Edges.FromImage(Image);
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

        static List<Tile> FindNeighborTiles(Tile tile) =>
            Tiles.Where(t => tile.Match(t) != null).ToList();

        static (Tile Tile, Edge Edge, bool Flip)? FindEdgeMatch(Tile tile, Edge edge)
        {
            foreach (var t in Tiles)
            {
                var match = tile.Match(t);
                if (match.HasValue && match.Value.ThisEdge == edge)
                {
                    return (t, match.Value.OtherEdge, match.Value.Flip);
                }

            }
            return null;
        }

        static IEnumerable<Tile> FindCorners()
        {
            foreach (var tile in Tiles)
            {
                var neighbors = FindNeighborTiles(tile);
                if (FindNeighborTiles(tile).Count() == 2)
                    yield return tile;
            }
        }

        static long CornerProduct() => FindCorners().Product(tile => tile.Id);

        // Pick an arbitrary corner and orient it so its edges are facing Down/Right
        static Tile TopLeftCorner()
        {
            var topLeftCorner = FindCorners().First();
            var neighbors = FindNeighborTiles(topLeftCorner);

            var edge1 = topLeftCorner.Match(neighbors[0]).Value.ThisEdge;
            var edge2 = topLeftCorner.Match(neighbors[1]).Value.ThisEdge;

            var edges = edge1 | edge2;

            if (edges == (Edge.Top | Edge.Right))
            {
                topLeftCorner.RotateRight();
            }
            else if (edges == (Edge.Left | Edge.Top))
            {
                topLeftCorner.Rotate180();
            }
            else if (edges == (Edge.Bottom | Edge.Left))
            {
                topLeftCorner.RotateLeft();
            }

            // Double-check
            edge1 = topLeftCorner.Match(neighbors[0]).Value.ThisEdge;
            edge2 = topLeftCorner.Match(neighbors[1]).Value.ThisEdge;
            Debug.Assert((edge1 | edge2) == (Edge.Bottom | Edge.Right));

            topLeftCorner.IsSet = true;
            return topLeftCorner;
        }

        static Tile StartOfNextRow(Tile above)
        {
            var match = FindEdgeMatch(above, Edge.Bottom).Value;

            var tile = match.Tile;
            var edge = match.Edge;
            if (match.Flip)
            {
                tile.FlipHorizontal();
                edge = above.Match(tile).Value.OtherEdge;
            }

            if (edge == Edge.Right)
            {
                tile.RotateLeft();
            }
            else if (edge == Edge.Bottom)
            {
                tile.Rotate180();
            }
            else if (edge == Edge.Left)
            {
                tile.RotateRight();
            }

            Debug.Assert((Edge.Bottom, Edge.Top, false) == above.Match(tile).Value);
            tile.IsSet = true;
            return tile;
        }


        static IEnumerable<Tile> AssembleRow(Tile start)
        {
            Tile current = start;
            while (true)
            {
                current.IsSet = true;
                yield return current;

                var match = FindEdgeMatch(current, Edge.Right);
                if (match == null) break;

                var (tile, otherEdge, flip) = (match.Value.Tile, match.Value.Edge, match.Value.Flip);
                if (flip)
                {
                    tile.FlipHorizontal();
                    otherEdge = current.Match(tile).Value.OtherEdge;
                }

                if (otherEdge == Edge.Top)
                {
                    tile.RotateLeft();
                }
                else if (otherEdge == Edge.Right)
                {
                    tile.Rotate180();
                }
                else if (otherEdge == Edge.Bottom)
                {
                    tile.RotateRight();
                }

                current = tile;
            }
        }

        static List<List<Tile>> Assemble()
        {
            var left = TopLeftCorner();
            var rows = new List<List<Tile>>() { AssembleRow(left).ToList() };

            while (FindEdgeMatch(left, Edge.Bottom) != null)
            {
                left = StartOfNextRow(left);
                rows.Add(AssembleRow(left).ToList());
            }

            Verify(rows);
            return rows;
        }

        static void VerifyRow(List<Tile> row)
        {
            for (var i = 0; i < row.Count - 1; i++)
            {
                var match = row[i].Match(row[i + 1]);
                Debug.Assert((Edge.Right, Edge.Left, false) == match.Value);
            }
        }

        static void VerifyColumn(List<Tile> col)
        {
            for (var i = 0; i < col.Count - 1; i++)
            {
                var match = col[i].Match(col[i + 1]);
                Debug.Assert((Edge.Bottom, Edge.Top, false) == match.Value);
            }
        }

        static void VerifyUnique(List<List<Tile>> grid)
        {
            var inputIds = Tiles.Select(t => t.Id).OrderBy(x => x);
            var usedIds = grid.SelectMany(row => row).Select(t => t.Id).OrderBy(x => x);

            Debug.Assert(inputIds.SequenceEqual(usedIds));
        }

        static void Verify(List<List<Tile>> grid)
        {
            foreach (var row in grid)
            {
                VerifyRow(row);
            }

            for (var i = 0; i < grid[0].Count; i++)
            {
                VerifyColumn(grid.Select(row => row[i]).ToList());
            }

            VerifyUnique(grid);
        }

        public static void Test()
        {
            Parse("20.test.txt");
            Console.WriteLine(CornerProduct());
            var grid = Assemble();

            foreach (var row in grid)
            {
                Console.WriteLine(string.Join(" ", row.Select(t => t.Id)));
            }

            var picture = MakePicture(grid);

            // By visual inspection, the orientation of picture that I've generated
            // has the same upper-left as the provided finished sample picture, but
            // with X & Y reversed. Confusingly, this means that indexing into
            // correctTestPicture[x][y] (not [y][x]) will give that result.
            var correctTestPicture = File.ReadAllLines("20.test-result.txt");
            for (var x = 0; x < picture.GetLength(0); x++)
            {
                for (var y = 0; y < picture.GetLength(1); y++)
                {
                    Debug.Assert(picture[x, y] == correctTestPicture[x][y]);
                }
            }
        }

        public static object Part1() => CornerProduct();

        static void InsertImage(char[,] picture, List<string> image, int xStart, int yStart)
        {
            for (int y = 0; y < image.Count; y++)
            {
                for (int x = 0; x < image[0].Length; x++)
                {
                    picture[x + xStart, y + yStart] = image[y][x];
                }
            }
        }

        static List<string> RemoveBorder(List<string> image)
        {
            var result = image.Select(row => row.Substring(1, row.Length - 2)).ToList();
            result.RemoveAt(0);
            result.RemoveAt(result.Count - 1);
            return result;
        }

        static char[,] MakePicture(List<List<Tile>> grid)
        {
            var gridSize = grid.Count;
            var trimmedTileSize = grid[0][0].Image.Count - 2;

            var result = new char[gridSize * trimmedTileSize, gridSize * trimmedTileSize];
            for (var y = 0; y < gridSize; y++)
            {
                var row = grid[y];
                for (var x = 0; x < gridSize; x++)
                {
                    var trimmedImage = RemoveBorder(row[x].Image);
                    InsertImage(result, trimmedImage, x * trimmedTileSize, y * trimmedTileSize);
                }
            }
            return result;
        }

        public static object Part2()
        {
            var grid = Assemble();
            var picture = MakePicture(grid);
            return 0;
        }
    }
}