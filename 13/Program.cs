
using System;
using System.Collections.Generic;
using System.Linq;

namespace _13
{
    public enum TurnDirection {
        Left,
        Stright,
        Right
    }

    public enum Direction {
        Up,
        Right,
        Down,
        Left
    }

    public enum TrackType { // Curve types from the POV of the curve
        None,
        Horizontal,
        Vertical,
        CurveUpRight,
        CurveUpLeft,
        CurveDownRight,
        CurveDownLeft,
        Intersection
    }

    public static class Extensions {
        public static Direction Turn(this Direction d, TurnDirection t) {
            if (t == TurnDirection.Stright) {
                return d;
            } else if (t == TurnDirection.Right) {
                return (Direction)(((int)d + 1) % 4);
            } else {
                return (Direction)(((int)d + 3) % 4);
            }
        }

        public static char S(this TrackType t) {
            switch (t) {
                case TrackType.None: return ' ';
                case TrackType.Horizontal: return '-';
                case TrackType.Vertical: return '|';
                case TrackType.CurveUpRight: return '\\';
                case TrackType.CurveDownLeft: return '\\';
                case TrackType.CurveUpLeft: return '/';
                case TrackType.CurveDownRight: return '/';
                case TrackType.Intersection: return '+';
                default: throw new Exception(t.ToString());
            }
        }

        public static char S(this Cart c) {
            switch (c.Direction) {
                case Direction.Up: return '^';
                case Direction.Right: return '>';
                case Direction.Down: return 'v';
                case Direction.Left: return '<';
                default: throw new Exception(c.Direction.ToString());
            }
        }
    }

    public class Cart {
        public int X;
        public int Y;
        public Direction Direction;

        private int turnCount = 0;

        public void MakeNextTurn() {
            var turn = (TurnDirection)(turnCount % 3);
            this.Direction = this.Direction.Turn(turn);
            turnCount += 1;
        }
    }

    public class Tracks {
        private TrackType[,] tracks;
        private IList<Cart> carts;

        public Tracks(IList<string> lines) {
            this.carts = new List<Cart>();

            var width = lines.First().Length;
            var height = lines.Count;
            this.tracks = new TrackType[width, height];
            
            for (var y = 0; y < height; y++) {
                var line = lines[y];
                for (var x = 0; x < width; x++) {
                    TrackType trackType;
                    Direction? cartDirection = null;
                    switch (line[x]) {
                        case ' ':
                            trackType = TrackType.None;
                            break;
                        case '-': 
                            trackType = TrackType.Horizontal;
                            break;
                        case '|':
                            trackType = TrackType.Vertical;
                            break;
                        case '/': // Either Down-Right or Up-Left
                            if (x == 0 || line[x-1] == ' ') {
                                trackType = TrackType.CurveDownRight;
                            } else {
                                trackType = TrackType.CurveUpLeft;
                            }
                            break;
                        case '\\': // Either Down-Left or Up-Right
                            if (x == 0 || line[x-1] == ' ') {
                                trackType = TrackType.CurveUpRight;
                            } else {
                                trackType = TrackType.CurveDownLeft;
                            }
                            break;
                        case '+':
                            trackType = TrackType.Intersection;
                            break;
                        case '^':
                            cartDirection = Direction.Up;
                            trackType = TrackType.Vertical;
                            break;
                        case 'v':
                            cartDirection = Direction.Down;
                            trackType = TrackType.Vertical;
                            break;
                        case '>':
                            cartDirection = Direction.Right;
                            trackType = TrackType.Horizontal;
                            break;
                        case '<':
                            cartDirection = Direction.Right;
                            trackType = TrackType.Horizontal;
                            break;
                        default: throw new Exception($"{x} {y} {line[x]}");
                    }
                    this.tracks[x,y] = trackType;

                    if (cartDirection != null) {
                        carts.Add(new Cart { X = x, Y = y, Direction = cartDirection.Value });
                    }
                }
            }
        }

        public override string ToString() {
            var result = new System.Text.StringBuilder();
            for (var y = 0; y < tracks.GetLength(1); y++) {
                for (var x = 0; x < tracks.GetLength(0); x++) {
                    var cart = carts.SingleOrDefault(c => c.X == x && c.Y == y);
                    if (cart == null) {
                        result.Append(tracks[x,y].S());
                    } else {
                        result.Append(cart.S());

                    }
                }
                result.AppendLine();                
            }
            return result.ToString();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadLines("test").ToList();
            var tracks = new Tracks(input);
            Console.WriteLine(tracks);
        }
    }
}

