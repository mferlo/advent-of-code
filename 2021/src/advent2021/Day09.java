package advent2021;

import java.util.*;

class Coord {
    public int H;
    public int W;
    public Coord(int h, int w) {
        H = h;
        W = w;
    }
}

public class Day09 {

    static int[][] map;
    static boolean[][] visited;
    static int height;
    static int width;

    static void parse(List<String> input) {
        height = input.size();
        width = input.get(0).length();
        map = new int[height][width];
        visited = new boolean[height][width];
        for (int h = 0; h < height; h++) {
            String s = input.get(h);
            for (int w = 0; w < width; w++) {
                map[h][w] = Integer.parseInt(s.substring(w, w + 1));
            }
        }
    }

    static boolean isLowest(int h, int w) {
        int value = map[h][w];
        if (h > 0 && map[h-1][w] <= value) {
            return false;
        }
        if (h < height - 1 && map[h+1][w] <= value) {
            return false;
        }
        if (w > 0 && map[h][w-1] <= value) {
            return false;
        }
        if (w < width - 1 && map[h][w+1] <= value) {
            return false;
        }
        return true;
    }

    static int basinSize(int hInit, int wInit) {
        Queue<Coord> queue = new LinkedList<>();
        queue.add(new Coord(hInit, wInit));
        int size = 0;

        while (!queue.isEmpty()) {
            Coord c = queue.poll();
            int h = c.H;
            int w = c.W;
            if (h < 0 || h >= height || w < 0 || w >= width || visited[h][w] || map[h][w] == 9) {
                continue;
            }
            visited[h][w] = true;
            size++;

            queue.add(new Coord(h - 1, w));
            queue.add(new Coord(h + 1, w));
            queue.add(new Coord(h, w - 1));
            queue.add(new Coord(h, w + 1));
        }

        return size;
    }

    public static String doIt(List<String> input) {
        parse(input);

        List<Coord> minima = new ArrayList<>();
        for (int h = 0; h < height; h++) {
            for (int w = 0; w < width; w++) {
                if (isLowest(h, w)) {
                    minima.add(new Coord(h, w));
                }
            }
        }

        int part1 = 0;
        List<Integer> basinSizes = new ArrayList<>();
        for (Coord c : minima) {
            part1 += map[c.H][c.W] + 1;
            basinSizes.add(basinSize(c.H, c.W));
        }

        Collections.sort(basinSizes);
        Collections.reverse(basinSizes);
        int part2 = basinSizes.get(0) * basinSizes.get(1) * basinSizes.get(2);

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }
}
