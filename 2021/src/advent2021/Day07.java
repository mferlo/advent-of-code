package advent2021;

import java.io.Console;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Function;

public class Day07 {

    static List<Integer> positions;
    static Map<Integer, Integer> geometricCosts;

    public static String doIt(List<String> input) {
        positions = Arrays.stream(input.get(0).split(",")).map(Integer::valueOf).sorted().toList();
        geometricCosts = new HashMap<>();
        geometricCosts.put(0, 0);

        long part1 = optimize(Day07::cost1);
        long part2 = optimize(Day07::cost2);

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }

    static int cost1(int target) {
        return positions.stream().mapToInt(p -> Math.abs(p - target)).sum();
    }

    static int getCost(int distance) {
        if (!geometricCosts.containsKey(distance)) {
            geometricCosts.put(distance, distance + getCost(distance - 1));
        }
        return geometricCosts.get(distance);
    }

    static int cost2(int target) {
        return positions.stream().mapToInt(p -> getCost(Math.abs(p - target))).sum();
    }

    static long optimize(Function<Integer, Integer> cost) {
        int guess = positions.get(positions.size() / 2);
        int costX = cost.apply(guess - 1);
        int costCur = cost.apply(guess);
        int costZ = cost.apply(guess + 1);

        int delta, costNext;
        if (costX < costCur) {
            delta = -1;
            costNext = costX;
        } else if (costZ < costCur) {
            delta = 1;
            costNext = costZ;
        } else {
            return costCur;
        }

        while (costNext < costCur) {
            guess += delta;
            costCur = costNext;
            costNext = cost.apply(guess + delta);
        }

        return costCur;
    }
}
