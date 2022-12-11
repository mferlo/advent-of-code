package advent2021;

import java.util.List;

public class Day15 {

    static int[][] parse(List<String> input) {
        int height = input.size();
        int width = input.get(0).length();
        int[][] map = new int[height][width];
        for (int h = 0; h < height; h++) {
            String s = input.get(h);
            for (int w = 0; w < width; w++) {
                map[h][w] = Integer.parseInt(s.substring(w, w + 1));
            }
        }
        return map;
    }

    static int dp(int[][] map) {
        int height = map.length;
        int width = map[0].length;

        Integer[][] cost = new Integer[height][width];

        cost[0][0] = 0;
        for (int h = 1; h < height; h++) {
            cost[h][0] = cost[h - 1][0] + map[h][0];
        }

        for (int w = 1; w < width; w++) {
            cost[0][w] = cost[0][w - 1] + map[0][w];
        }

        for (int h = 1; h < height; h++) {
            for (int w = 1; w < width; w++) {
                cost[h][w] = map[h][w] + Math.min(cost[h-1][w], cost[h][w-1]);
            }
        }

        return cost[height - 1][width - 1];
    }

    // https://old.reddit.com/r/adventofcode/comments/rgqzt5/2021_day_15_solutions/hopb35x/
    static int evaluateRisk(int[][] riskMap) {
        // A 2d array to track the total risk involved to travel from each and every
        // node to the bottom-right corner of the map.
        int[][] riskSums = new int[riskMap.length][riskMap[0].length];
        // Initialize the riskSum at every node to a large number. 1,000,000 should do
        // fine. We can't use Integer.MAX_VALUE since we will loop through and possibly
        // add to these values. We don't want to overflow.
        for (int r = 0; r < riskSums.length; r++) {
            for (int c = 0; c < riskSums[0].length; c++) {
                riskSums[r][c] = 1_000_000;
            }
        }
        // The riskSum for the bottom-right node is 0.
        riskSums[riskSums.length - 1][riskSums[0].length - 1] = 0;

        // The idea is to loop through the riskSums array from the bottom-right to the
        // top-left and update the riskSum at each node based on the riskSums of it's
        // neighbors. We will change the riskSum at each node to reflect the minimum
        // risk+riskSum of each of it's neighbors. If a change is made to the map, we
        // will loop back through in case that change triggered another potential
        // improvement to the graph. We will continue to loop through until no changes
        // are made.
        boolean changeMade = true;
        while (changeMade) {
            changeMade = false;
            for (int r = riskSums.length - 1; r >= 0; r--) {
                for (int c = riskSums[0].length - 1; c >= 0; c--) {
                    // Four neighbors:
                    // riskMap[r][c] : the risk to enter this 1 node.
                    // riskSum[r][c] : the total risk involved in traveling to the bottom-right
                    // from this node.
                    int min = Integer.MAX_VALUE;
                    if (r - 1 >= 0)
                        min = Math.min(min, riskMap[r - 1][c] + riskSums[r - 1][c]);
                    if (r + 1 < riskSums.length)
                        min = Math.min(min, riskMap[r + 1][c] + riskSums[r + 1][c]);
                    if (c - 1 >= 0)
                        min = Math.min(min, riskMap[r][c - 1] + riskSums[r][c - 1]);
                    if (c + 1 < riskSums[0].length)
                        min = Math.min(min, riskMap[r][c + 1] + riskSums[r][c + 1]);

                    // If a change is being made to a node, we will have to loop back through again.
                    int oldRisk = riskSums[r][c];
                    riskSums[r][c] = Math.min(riskSums[r][c], min);
                    if (riskSums[r][c] != oldRisk)
                        changeMade = true;
                }
            }
        }
        // We now know the riskSum at every single node, but all we wanted was the
        // riskSum at 0,0.
        return (riskSums[0][0]);
    }

    static int getScaledValue(int value, int scaleFactor) {
        int result = value + scaleFactor;
        return result <= 9 ? result : result - 9;
    }

    static int[][] expand(int[][] map) {
        int height = map.length;
        int width = map[0].length;

        int[][] expandedMap = new int[height * 5][width * 5];

        for (int tileHeight = 0; tileHeight < 5; tileHeight++) {
            for (int tileWidth = 0; tileWidth < 5; tileWidth++) {
                int scaleFactor = tileHeight + tileWidth;
                for (int h = 0; h < height; h++) {
                    for (int w = 0; w < width; w++) {
                        expandedMap[tileHeight * height + h][tileWidth * width + w] = getScaledValue(map[h][w], scaleFactor);
                    }
                }
            }
        }
        return expandedMap;
    }

    public static String doIt(List<String> input) {
        int[][] map = parse(input);

        long part1 = dp(map);
        long part2 = dp(expand(map));
        // 2967 is too high

        long part2b = evaluateRisk(expand(map));

        return "Part 1: " + part1 + "; Part 2: " + part2 + "; Part 2b: " + part2b;
    }
}