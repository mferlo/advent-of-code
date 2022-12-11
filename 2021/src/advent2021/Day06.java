package advent2021;

import java.util.Arrays;
import java.util.List;

public class Day06 {

    public static String doIt(List<String> input) {
        long[] lanternfish = new long[9];
        for (String s : input.get(0).split(",")) {
            int i = Integer.parseInt(s);
            lanternfish[i]++;
        }

        for (var count = 1; count <= 80; count++) {
            lanternfish = step(lanternfish);
        }
        long part1 = Arrays.stream(lanternfish).sum();

        for (var count = 81; count <= 256; count++) {
            lanternfish = step(lanternfish);
        }
        long part2 = Arrays.stream(lanternfish).sum();

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }

    static long[] step(long[] arr) {
        long[] result = new long[9];
        System.arraycopy(arr, 1, result, 0, 8);
        result[6] += arr[0];
        result[8] += arr[0];
        return result;
    }
}
