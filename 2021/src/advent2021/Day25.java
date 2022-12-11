package advent2021;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

enum Square {
    Empty("."),
    Right(">"),
    Down("v");

    final String string;

    Square(String s) {
        string = s;
    }

    public String toString() {
        return string;
    }

    static Square fromChar(char ch) {
        return switch (ch) {
            case '>' -> Square.Right;
            case 'v' -> Square.Down;
            case '.' -> Square.Empty;
            default -> throw new IllegalStateException(ch + "");
        };
    }
}

record Coordinate(int r, int c) {}

class SeaFloor {
    final int rows;
    final int cols;
    Square[][] state;

    public SeaFloor(List<String> input) {
        rows = input.size();
        cols = input.get(0).length();
        state = new Square[rows][cols];
        for (int r = 0; r < rows; r++) {
            String inputRow = input.get(r);
            for (int c = 0; c < cols; c++) {
                state[r][c] = Square.fromChar(inputRow.charAt(c));
            }
        }
    }

    public String toString() {
        List<String> stringRows = new ArrayList<>();
        for (int r = 0; r < rows; r++) {
            stringRows.add(String.join("", Arrays.stream(state[r]).map(sq -> sq.toString()).toList()));
        }
        return String.join("\n", stringRows);
    }

    boolean moveEast() {
        List<Coordinate> toMove = new ArrayList<>();
        for (int r = 0; r < rows; r++) {
            if (state[r][cols-1] == Square.Right && state[r][0] == Square.Empty) {
                toMove.add(new Coordinate(r, cols - 1));
            }

            for (int c = 0; c < cols - 1; c++) {
                if (state[r][c] == Square.Right && state[r][c + 1] == Square.Empty) {
                    toMove.add(new Coordinate(r, c));
                }
            }
        }

        for (Coordinate coord : toMove) {
            state[coord.r()][coord.c()] = Square.Empty;
        }

        for (Coordinate coord : toMove) {
            state[coord.r()][(coord.c() + 1) % cols] = Square.Right;
        }

        return !toMove.isEmpty();
    }

    boolean moveSouth() {
        List<Coordinate> toMove = new ArrayList<>();
        for (int c = 0; c < cols; c++) {
            if (state[rows - 1][c] == Square.Down && state[0][c] == Square.Empty) {
                toMove.add(new Coordinate(rows - 1, c));
            }

            for (int r = 0; r < rows - 1; r++) {
                if (state[r][c] == Square.Down && state[r + 1][c] == Square.Empty) {
                    toMove.add(new Coordinate(r, c));
                }
            }
        }

        for (Coordinate coord : toMove) {
            state[coord.r()][coord.c()] = Square.Empty;
        }

        for (Coordinate coord : toMove) {
            state[(coord.r() + 1) % rows][coord.c()] = Square.Down;
        }

        return !toMove.isEmpty();
    }

    public boolean move() {
        return moveEast() | moveSouth();
    }
}

public class Day25 {

    public static String doIt(List<String> input) {
        SeaFloor seaFloor = new SeaFloor(input);
        long part1 = 1;
        while (seaFloor.move()) {
            part1++;
        }
        long part2 = 0;

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }
}