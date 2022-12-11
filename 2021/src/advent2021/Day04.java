package advent2021;

import java.util.*;

class BingoCell {
    public int Number;
    public boolean Marked;
    public int Row;
    public int Col;

    public BingoCell(int n, int r, int c) {
        Number = n;
        Row = r;
        Col = c;
        Marked = false;
    }
}

class BingoBoard {
    public boolean Winner = false;
    private final BingoCell[][] Board = new BingoCell[5][5];
    private final Map<Integer, BingoCell> Lookup = new HashMap<>();

    public BingoBoard(List<String> rows) {
        for (int r = 0; r < 5; r++) {
            initializeRow(rows.get(r), r);
        }
    }

    private void initializeRow(String row, int r) {
        String[] values = row.trim().split(" +");
        if (values.length != 5) throw new IllegalStateException(row + ": " + values.length);
        for (int c = 0; c < 5; c++) {
            int n = Integer.parseInt(values[c]);
            BingoCell cell = new BingoCell(n, r, c);
            Board[r][c] = cell;
            Lookup.put(n, cell);
        }
    }

    public int call(int n) {
        BingoCell cell = Lookup.get(n);
        if (cell == null) {
            return 0;
        }

        cell.Marked = true;

        if (isWinner(cell)) {
            Winner = true;
            int unmarked = Lookup.values().stream().filter(c -> !c.Marked).mapToInt(c -> c.Number).sum();
            return n * unmarked;
        } else {
            return 0;
        }
    }

    private boolean isWinner(BingoCell cell) {
        int row = cell.Row;
        int col = cell.Col;

        return Arrays.stream(Board[row]).allMatch(c -> c.Marked)
            || Arrays.stream(Board).allMatch(boardRow -> boardRow[col].Marked);
    }
}

public class Day04 {

    public static String doIt(List<String> input) {
        int[] toCall = Arrays.stream(input.get(0).split(",")).mapToInt(Integer::parseInt).toArray();
        List<BingoBoard> boards = new ArrayList<>();
        for (int boardStart = 2; boardStart + 4 < input.size(); boardStart += 6) {
            boards.add(new BingoBoard(input.subList(boardStart, boardStart + 5)));
        }

        int part1 = 0;
        for (int value : toCall) {
            for (BingoBoard board : boards) {
                int result = board.call(value);
                if (result > 0) {
                    if (part1 == 0) {
                        part1 = result;
                    }
                    if (boards.stream().allMatch(b -> b.Winner)) {
                        return "Part 1: " + part1 + "; Part 2: " + result;
                    }
                }
            }
        }
        throw new IllegalStateException();
    }
}
