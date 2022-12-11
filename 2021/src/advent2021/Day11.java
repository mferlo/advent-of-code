package advent2021;

import java.util.LinkedList;
import java.util.List;
import java.util.Queue;

public class Day11 {

    static int[][] parse(List<String> input) {
        int[][] result = new int[10][10];
        for (int row = 0; row < 10; row++) {
            String line = input.get(row);
            for (int col = 0; col < 10; col++) {
                result[row][col] = Integer.parseInt(line.substring(col, col + 1));
            }
        }
        return result;
    }

    static long flashCount;
    static boolean synced;

    static int[][] step(int[][] levels) {
        Queue<Point> toFlash = new LinkedList<>();
        int[][] next = new int[10][10];
        boolean[][] flashed = new boolean[10][10];
        for (int r = 0; r < 10; r++) {
            for (int c = 0; c < 10; c++) {
                next[r][c] = levels[r][c] + 1;
                if (next[r][c] > 9) {
                    toFlash.add(new Point(r, c));
                }
            }
        }

        while (!toFlash.isEmpty()) {
            Point p = toFlash.poll();
            int rFlash = p.X;
            int cFlash = p.Y;
            if (flashed[rFlash][cFlash]) {
                continue;
            }

            flashCount++;
            flashed[rFlash][cFlash] = true;

            for (int r = Math.max(rFlash - 1, 0); r <= Math.min(rFlash + 1, 9); r++) {
                for (int c = Math.max(cFlash - 1, 0); c <= Math.min(cFlash + 1, 9); c++) {
                    if (rFlash == r && cFlash == c) {
                        continue;
                    }

                    next[r][c]++;
                    if (next[r][c] > 9) {
                        toFlash.add(new Point(r, c));
                    }
                }
            }
        }

        synced = true;
        for (int r = 0; r < 10; r++) {
            for (int c = 0; c < 10; c++) {
                if (flashed[r][c]) {
                    next[r][c] = 0;
                } else {
                    synced = false;
                }
            }
        }

        return next;
    }

    public static String doIt(List<String> input) {
        int[][] energyLevels = parse(input);

        flashCount = 0;
        /*
        for (int i = 1; i <= 100; i++) {
            energyLevels = step(energyLevels);
        }
        */
        long part1 = flashCount;

        synced = false;
        long part2 = 0;
        while (!synced) {
            part2++;
            energyLevels = step(energyLevels);
        }

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }
}
