using System;
using System.Collections.Generic;
using System.Linq;

namespace _15
{
    enum UnitType { Elf, Goblin }
    enum CombatStatus { InProgress, Done }

    struct Position
    {
        public int Row;
        public int Col;

        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override string ToString() => $"{Row}, {Col}";

        public IEnumerable<Position> AdjacentPositions() // In reading order
        {
            yield return new Position(Row - 1, Col);
            yield return new Position(Row, Col - 1);
            yield return new Position(Row, Col + 1);
            yield return new Position(Row + 1, Col);
        }

        public static bool operator ==(Position lhs, Position rhs) => lhs.Row == rhs.Row && lhs.Col == rhs.Col;
        public static bool operator !=(Position lhs, Position rhs) => !(lhs == rhs);
    }

    class Unit
    {
        public UnitType Type { get; }
        public Position Position { get; set; }

        int AttackPower { get; }
        public int HP { get; private set; }

        public Unit(char type, Position position)
        {
            Type = type == 'E' ? UnitType.Elf : UnitType.Goblin;
            Position = position;
            AttackPower = 3;
            HP = 200;
        }

        public void Attack(Unit enemy)
        {
            if (this.Type == enemy.Type)
            {
                throw new Exception("Friendly fire");
            }

            enemy.HP -= this.AttackPower;
        }

        public override string ToString() =>
            $"{Type} ({Position}) - AP: {AttackPower}, HP: {HP}";
    }

    class GridCell
    {
        public Position Position { get; }
        bool IsWall { get; }
        public Unit Occupant { get; set; }
        public bool IsEmpty => !IsWall && Occupant == null;

        public GridCell(char occupant, int row, int col)
        {
            Position = new Position { Row = row, Col = col };
            IsWall = occupant == '#';
            if (occupant == 'E' || occupant == 'G')
            {
                Occupant = new Unit(occupant, Position);
            }
            else
            {
                Occupant = null;
            }
        }

        public bool IsOccupiedByEnemyOf(UnitType type) =>
            Occupant != null && Occupant.Type != type;

        public override string ToString() =>
            Occupant == null ? (IsWall ? "#" : ".") : (Occupant.Type == UnitType.Elf ? "E" : "G");
    }

    class Grid
    {
        public int NumRows { get; }
        public int NumCols { get; }
        public GridCell[][] Cells;

        public Grid(IList<string> mapRows)
        {
            NumRows = mapRows.Count;
            NumCols = mapRows[0].Length;
            Cells = new GridCell[NumRows][];

            for (int r = 0; r < NumRows; r++) {
                Cells[r] = new GridCell[NumCols];
                for (int c = 0; c < NumCols; c++) {
                    var occupant = mapRows[r][c];
                    Cells[r][c] = new GridCell(occupant, r, c);
                }
            }
        }

        GridCell Cell(Position position) => Cells[position.Row][position.Col];

        public void Move(Unit unit, Position destination)
        {
            Cell(unit.Position).Occupant = null;
            Cell(destination).Occupant = unit;
            unit.Position = destination;
        }

        public IEnumerable<GridCell> InReadingOrder(Func<GridCell, bool> predicate) =>
            InReadingOrder(predicate, cell => cell);

        public IEnumerable<T> InReadingOrder<T>(Func<GridCell, bool> predicate, Func<GridCell, T> accessor)
        {
            for (int r = 0; r < NumRows; r++)
            {
                for (int c = 0; c < NumCols; c++)
                {
                    var cell = Cells[r][c];
                    if (predicate(cell))
                    {
                        yield return accessor(cell);
                    }
                }
            }
        }

        public IEnumerable<Unit> AdjacentEnemies(Unit unit) =>
            unit.Position.AdjacentPositions().Where(p => IsValid(p) && Cell(p).IsOccupiedByEnemyOf(unit.Type)).Select(p => Cell(p).Occupant);

        public IEnumerable<Position> ClosestPositions(Position from, ISet<Position> to)
        {
            var visited = new bool[NumRows][];
            for (int r = 0; r < NumRows; r++)
            {
                visited[r] = new bool[NumCols];
            }

            var currentPositions = new List<Position> { from };

            while (currentPositions.Any())
            {
                foreach (var p in currentPositions)
                {
                    visited[p.Row][p.Col] = true;
                }

                var adjacentPositions = currentPositions.SelectMany(p => p.AdjacentPositions())
                    .Where(p => IsValid(p) && Cell(p).IsEmpty && !visited[p.Row][p.Col]);
                
                var foundPositions = to.Intersect(adjacentPositions);
                if (foundPositions.Any())
                {
                    return foundPositions;
                }

                currentPositions = adjacentPositions.ToList();
            }

            return Enumerable.Empty<Position>();
        }

        public Position NextStepOnShortestPath(Position from, Position to)
        {
            // Go backwards, recording distance
            var distance = new int?[NumRows][];
            for (int r = 0; r < NumRows; r++)
            {
                distance[r] = new int?[NumCols];
            }

            distance[to.Row][to.Col] = 0;
            var currentPositions = new List<Position> { to };
            int steps = 0;

            while (distance[from.Row][from.Col] == null)
            {
                steps++;
                var adjacentPositions = currentPositions.SelectMany(p => p.AdjacentPositions())
                    .Where(p => IsValid(p) && distance[p.Row][p.Col] == null && (p == from || Cell(p).IsEmpty))
                    .ToList();
                
                foreach (var p in adjacentPositions)
                {
                    distance[p.Row][p.Col] = steps;
                }

                currentPositions = adjacentPositions;
            }

            return from.AdjacentPositions().First(p => steps - 1 == distance[p.Row][p.Col]);
        }

        public void Kill(Unit unit)
        {
            if (unit.HP > 0)
            {
                throw new Exception($"{unit} is not dead yet");
            }
            Cell(unit.Position).Occupant = null;
        }


        public bool IsValid(Position position) =>
            position.Row >= 0 && position.Row < NumRows && position.Col >= 0 && position.Col < NumCols;

        private string StringifyRow(GridCell[] row) => String.Join("", row.Select(cell => cell.ToString()));

        public override string ToString() => String.Join("\n", Cells.Select(StringifyRow));
    }

    class Program
    {
        static IEnumerable<Unit> GetUnitOrder(Grid grid) =>
            grid.InReadingOrder(cell => cell.Occupant != null, cell => cell.Occupant);

        static IEnumerable<Unit> FindTargets(Grid grid, Unit current) =>
            grid.InReadingOrder(cell => cell.IsOccupiedByEnemyOf(current.Type), cell => cell.Occupant);

        static IEnumerable<Position> GetAdjacentSpaces(Grid grid, IEnumerable<Unit> units)
        {
            foreach (var unit in units)
            {
                foreach (var adjacentSpace in unit.Position.AdjacentPositions())
                {
                    if (grid.IsValid(adjacentSpace))
                    {
                        yield return adjacentSpace;
                    }
                }
            }
        }

        static Position? FindNextStep(Grid grid, Unit unit, ISet<Position> adjacentTargetSpaces)
        {
            var emptyAdjacentTargetPositions = grid.InReadingOrder(cell => adjacentTargetSpaces.Contains(cell.Position) && cell.IsEmpty)
                .Select(c => c.Position)
                .ToHashSet();
            var closestEmptyAdjacentTargetSpaces = grid.ClosestPositions(unit.Position, emptyAdjacentTargetPositions);

            if (!closestEmptyAdjacentTargetSpaces.Any()) // Can't make it to any spaces adjacent to an enemy
            {
                return null;
            }

            var spaceToMoveTowards = closestEmptyAdjacentTargetSpaces.OrderBy(p => p.Row).ThenBy(p => p.Col).First();
            return grid.NextStepOnShortestPath(unit.Position, spaceToMoveTowards);
        }

        static void MaybeAttack(Grid grid, Unit unit)
        {
            var enemies = grid.AdjacentEnemies(unit);
            if (!enemies.Any())
            {
                return;
            }
            var minHP = enemies.Min(e => e.HP);
            var target = enemies.OrderBy(e => e.Position.Row).ThenBy(e => e.Position.Col).First(e => e.HP == minHP);
            unit.Attack(target);
            if (target.HP <= 0)
            {
                grid.Kill(target);
            }
        }

        static CombatStatus TakeTurn(Grid grid, Unit unit)
        {
            if (unit.HP <= 0)
            {
                return CombatStatus.InProgress;
            }

            var targets = FindTargets(grid, unit).ToList();
            if (!targets.Any())
            {
                return CombatStatus.Done;
            }

            var adjacentTargetSpaces = GetAdjacentSpaces(grid, targets).Distinct().ToHashSet();

            if (!adjacentTargetSpaces.Contains(unit.Position)) // We have to move
            {
                var nextStep = FindNextStep(grid, unit, adjacentTargetSpaces);
                if (nextStep == null)
                {
                    return CombatStatus.InProgress;
                }
                grid.Move(unit, nextStep.Value);
            }

            MaybeAttack(grid, unit);

            return CombatStatus.InProgress;
        }

        static CombatStatus DoRound(Grid grid)
        {
            var units = GetUnitOrder(grid).ToList();
            foreach (var unit in units)
            {
                if (TakeTurn(grid, unit) == CombatStatus.Done)
                {
                    return CombatStatus.Done;
                }
            }

            return CombatStatus.InProgress;
        }

        private const string TestData =
@"#########
#G..G..G#
#.......#
#.......#
#G..E..G#
#.......#
#.......#
#G..G..G#
#########";

        private const string TestCombat =
@"#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######";

        private const string TestCombat2 =
@"#######
#G..#E#
#E#E.E#
#G.##.#
#...#E#
#...E.#
#######";

        private const string Input = 
@"################################
##########..........############
########G..................#####
#######..G.GG...............####
#######....G.......#......######
########.G.G...............#E..#
#######G.................#.....#
########.......................#
########G.....G....#.....##....#
########.....#....G.........####
#########..........##....E.E#.##
##########G..G..........#####.##
##########....#####G....####E.##
######....G..#######.....#.....#
###....#....#########......#####
####........#########..E...#####
###.........#########......#####
####G....G..#########......#####
####..#.....#########....#######
######.......#######...E.#######
###.G.....E.G.#####.....########
#.....G........E.......#########
#......#..#..####....#.#########
#...#.........###.#..###########
##............###..#############
######.....E####..##############
######...........###############
#######....E....################
######...####...################
######...###....################
###.....###..##..###############
################################";

        static void Main(string[] args)
        {
            var grid = new Grid(Input.Split("\r\n"));
            var rounds = 0;
            while (DoRound(grid) != CombatStatus.Done)
            {
                rounds++;
                Console.WriteLine(rounds);
                Console.WriteLine(grid);
                Console.WriteLine(String.Join("\n", GetUnitOrder(grid)));
            }

            Console.WriteLine(rounds);
            Console.WriteLine(grid);
            Console.WriteLine(String.Join("\n", GetUnitOrder(grid)));

            var hpRemaining = GetUnitOrder(grid).Sum(u => u.HP);
            Console.WriteLine($"*** Result: {hpRemaining * rounds}");

        }
    }
}
