using System;
using System.Collections.Generic;
using System.Linq;

namespace _15
{
    enum UnitType { Elf, Goblin }
    
    class Unit
    {
        public Unit(char type)
        {
            Type = type == 'E' ? UnitType.Elf : UnitType.Goblin;
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

        public UnitType Type { get; }
        int AttackPower { get; }
        int HP { get; set; }
    }

    class GridCell
    {
        int Row { get; }
        int Col { get; }
        bool IsWall { get; }
        Unit Occupant { get; set; }

        public GridCell(char occupant, int row, int col)
        {
            Row = row;
            Col = col;
            IsWall = occupant == '#';
            if (occupant == 'E' || occupant == 'G')
            {
                Occupant = new Unit(occupant);
            }
            else
            {
                Occupant = null;
            }
        }

        public override string ToString()
        {
            if (Occupant == null)
            {
                return IsWall ? "#" : ".";
            }
            else
            {
                return Occupant.Type == UnitType.Elf ? "E" : "G";
            }
        }
    }

    class Grid
    {
        private int NumRows;
        private int NumCols;
        private GridCell[][] Cells;

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

        private string StringifyRow(GridCell[] row) => String.Join("", row.Select(cell => cell.ToString()));

        public override string ToString() => String.Join("\n", Cells.Select(StringifyRow));
    }

    class Program
    {
        static string TestData = @"#########
#G..G..G#
#.......#
#.......#
#G..E..G#
#.......#
#.......#
#G..G..G#
#########";

        static void Main(string[] args)
        {
            var grid = new Grid(TestData.Split("\r\n"));
            Console.WriteLine(grid);
        }
    }
}
