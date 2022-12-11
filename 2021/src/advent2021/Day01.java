package advent2021;

import java.util.*;
import java.util.stream.Collectors;

public class Day01 {

    static String doIt(List<String> input) {
        List<Integer> data = input.stream().map(Integer::valueOf).collect(Collectors.toList());
        var part1 = part1(data);
        var part2 = part2(data);

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }

    static int part1(List<Integer> data) {
        var o = new Object() { Integer prev = data.get(0); int count = 0; };

        data.stream().skip(1).forEach(i -> {
            if (o.prev < i) {
                o.count++;
            }
            o.prev = i;
        });

        return o.count;
    }

    static int part2(List<Integer> data) {
        Integer[] a = data.toArray(new Integer[0]);

        var prev = a[0] + a[1] + a[2];
        int count = 0;

        for (int i = 3; i < a.length; i++) {
            var sum = prev - a[i-3] + a[i];
            if (sum > prev) {
                count++;
            }
            prev = sum;
        }

        return count;
    }
}
