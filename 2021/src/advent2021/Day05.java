package advent2021;

import java.util.*;

class Point {
    public int X;
    public int Y;
    public Point(int x, int y) {
        X = x;
        Y = y;
    }
}

class Line {
    public boolean IsOrthogonal;
    public List<Point> Points;
    public int MaxX;
    public int MaxY;

    public Line(String s) {
        String[] endpoints = s.split(" -> ");
        String[] p1 = endpoints[0].split(",");
        String[] p2 = endpoints[1].split(",");

        int x1 = Integer.parseInt(p1[0]);
        int y1 = Integer.parseInt(p1[1]);
        int x2 = Integer.parseInt(p2[0]);
        int y2 = Integer.parseInt(p2[1]);

        IsOrthogonal = x1 == x2 || y1 == y2;

        Points = new ArrayList<>();
        int deltaX = Integer.compare(x2, x1);
        int deltaY = Integer.compare(y2, y1);
        int x = x1;
        int y = y1;
        while (!(x == x2 && y == y2)) {
            Points.add(new Point(x, y));
            x += deltaX;
            y += deltaY;
        }
        Points.add(new Point(x, y));

        MaxX = Math.max(x1, x2);
        MaxY = Math.max(y1, y2);
    }
}

public class Day05 {

    public static String doIt(List<String> input) {
        List<Line> lines = input.stream().map(Line::new).toList();
        int maxX = lines.stream().map(l -> l.MaxX).max(Comparator.comparingInt(i -> i)).get();
        int maxY = lines.stream().map(l -> l.MaxY).max(Comparator.comparingInt(i -> i)).get();

        int part1 = calculate(lines, maxX, maxY, false);
        int part2 = calculate(lines, maxX, maxY, true);
        return "Part 1: " + part1 + "; Part 2: " + part2;
    }

    static int calculate(List<Line> lines, int maxX, int maxY, boolean includeDiagonals) {
        int[][] grid = new int[maxX + 1][maxY + 1];

        for (Line line : lines) {
            if (!line.IsOrthogonal && !includeDiagonals) {
                continue;
            }

            for (Point point : line.Points) {
                grid[point.X][point.Y]++;
            }
        }

        int result = 0;
        for (int x = 0; x <= maxX; x++) {
            for (int y = 0; y <= maxY; y++) {
                if (grid[x][y] >= 2) {
                    result++;
                }
            }
        }
        return result;
    }
}
