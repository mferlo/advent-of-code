package advent2021;

import java.util.ArrayList;
import java.util.List;

class EnhancementAlgorithm {
    final String s;
    public EnhancementAlgorithm(String s) {
        this.s = s;
        if (s.length() != 512) {
            throw new IllegalStateException("" + s.length());
        }
    }

    public boolean getPixel(int i) {
        return s.charAt(i) == '#';
    }
}

class Picture {
    boolean[][] grid;
    int rows;
    int cols;
    boolean defaultValue;

    public Picture(List<String> input) {
        defaultValue = false;
        rows = input.size();
        cols = input.get(0).length();

        grid = new boolean[rows][cols];

        for (int r = 0; r < rows; r++) {
            String s = input.get(r);
            for (int c = 0; c < cols; c++) {
                grid[r][c] = s.charAt(c) == '#';
            }
        }
    }

    boolean getValue(int r, int c) {
        if (r < 0 || c < 0 || r >= rows || c >= cols) {
            return defaultValue;
        }
        return grid[r][c];
    }

    int getLookupValueFor(int rCenter, int cCenter) {
        int value = 0;
        for (int r = rCenter - 1; r <= rCenter + 1; r++) {
            for (int c = cCenter - 1; c <= cCenter + 1; c++) {
                value <<= 1;
                if (getValue(r, c)) {
                    value |= 1;
                }
            }
        }

        return value;
    }

    public void enhance(EnhancementAlgorithm algorithm) {
        boolean[][] newGrid = new boolean[rows + 2][cols + 2];
        for (int rCenter = -1; rCenter <= rows; rCenter++) {
            for (int cCenter = -1; cCenter <= cols; cCenter++) {
                int lookup = getLookupValueFor(rCenter, cCenter);
                newGrid[rCenter + 1][cCenter + 1] = algorithm.getPixel(lookup);
            }
        }

        defaultValue = algorithm.getPixel(defaultValue ? 511 : 0);
        rows += 2;
        cols += 2;
        grid = newGrid;
    }

    public int litPixels() {
        int result = 0;
        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < cols; c++) {
                if (grid[r][c]) {
                    result++;
                }
            }
        }
        return result;
    }

    public String toString() {
        List<String> sRows = new ArrayList<>();
        for (int r = 0; r < rows; r++) {
            String sRow = "";
            for (int c = 0; c < cols; c++) {
                sRow += grid[r][c] ? "#" : ".";
            }
            sRows.add(sRow);
        }
        return String.join("\n", sRows);
    }
}

public class Day20 {

    public static String doIt(List<String> input) {

        var algo = new EnhancementAlgorithm(input.get(0));
        var picture = new Picture(input.subList(2, input.size()));

        picture.enhance(algo);
        picture.enhance(algo);
        long part1 = picture.litPixels();

        for (int i = 3; i <= 50; i++) {
            picture.enhance(algo);
        }
        long part2 = picture.litPixels();

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }
}
