package advent2021;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

class Grid {

    boolean[][] points;
    int height;
    int width;
    List<String> folds;

    public Grid(List<String> input) {
        List<String[]> pointInput = input.stream().takeWhile(i -> i.length() > 0).map(i -> i.split(",")).toList();
        folds = input.stream().skip(pointInput.size() + 1).toList();

        int[] xPoints = pointInput.stream().mapToInt(i -> Integer.parseInt(i[0])).toArray();
        int[] yPoints = pointInput.stream().mapToInt(i -> Integer.parseInt(i[1])).toArray();

        width = 1 + Arrays.stream(xPoints).max().getAsInt();
        height = 1 + Arrays.stream(yPoints).max().getAsInt();

        points = new boolean[height][width];

        for (int i = 0; i < xPoints.length; i++) {
            points[yPoints[i]][xPoints[i]] = true;
        }
    }

    public int count() {
        int result = 0;
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (points[y][x]) {
                    result++;
                }
            }
        }
        return result;
    }

    void foldX(int xSplit) {
        boolean[][] newPoints = new boolean[height][xSplit];

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < xSplit; x++) {
                newPoints[y][x] = points[y][x] || points[y][width - x - 1];
            }
        }

        width = xSplit;
        points = newPoints;
    }

    void foldY(int ySplit) {
        boolean[][] newPoints = new boolean[ySplit][width];

        for (int y = 0; y < ySplit; y++) {
            for (int x = 0; x < width; x++) {
                newPoints[y][x] = points[y][x] || points[height - y - 1][x];
            }
        }

        height = ySplit;
        points = newPoints;
    }

    void fold(String f) {
        char axis = f.charAt(11);
        int amount = Integer.parseInt(f.split("=")[1]);

        if (axis == 'y') {
            foldY(amount);
        } else {
            foldX(amount);
        }
    }

    public void foldFirst() {
        fold(folds.get(0));
    }

    public void foldRest() {
        for (String f : folds.subList(1, folds.size())) {
            fold(f);
        }
    }

    String line(boolean[] l) {
        List<String> chars = new ArrayList<>();
        for (int x = 0; x < l.length; x++) {
            chars.add(l[x] ? "#" : ".");
        }
        return String.join("", chars);
    }

    public String toString() {
        List<String> lines = new ArrayList<>();
        for (int y = 0; y < points.length; y++) {
            lines.add(line(points[y]));
        }
        return String.join("\n", lines);
    }
}

public class Day13 {

    public static String doIt(List<String> input) {
        Grid grid = new Grid(input);

        grid.foldFirst();
        long part1 = grid.count();

        grid.foldRest();
        String part2 = grid.toString();

        return "Part 1: " + part1 + "; Part 2: \n" + part2;
    }
}
