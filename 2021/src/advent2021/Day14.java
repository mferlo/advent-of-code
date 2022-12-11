package advent2021;

import java.util.*;

public class Day14 {

    static Map<String, String> transforms;

    static void parse(List<String> input) {
        transforms = new HashMap<>();
        for (int i = 2; i < input.size(); i++) {
            var parts = input.get(i).split(" -> ");
            transforms.put(parts[0], parts[1]);
        }
    }

    static Map<String, Long> emptyState() {
        Map<String, Long> result = new HashMap<>();
        for (String t : transforms.keySet()) {
            result.put(t, 0L);
        }
        return result;
    }

    static Map<String, Long> initialize(String p) {
        Map<String, Long> result = emptyState();

        for (int i =0; i < p.length() - 1; i++) {
            String key = p.substring(i, i + 2);
            Long value = result.get(key);
            result.put(key, value + 1);
        }

        return result;
    }

    static Map<String, Long> step(Map<String, Long> state) {
        Map<String, Long> newState = emptyState();

        for (String k : state.keySet()) {
            Long v = state.get(k);
            String newLetter = transforms.get(k);

            String key1 = k.charAt(0) + newLetter;
            Long val1 = newState.get(key1);
            newState.put(key1, v + val1);

            String key2 = newLetter + k.charAt(1);
            Long val2 = newState.get(key2);
            newState.put(key2, v + val2);
        }

        return newState;
    }

    static long result(Map<String, Long> state, String initialState) {
        Map<String, Long> frequency = new HashMap<>();
        for (String t : transforms.keySet()) {
            frequency.put(t.substring(0, 1), 0L);
            frequency.put(t.substring(1, 2), 0L);
        }

        String first = initialState.substring(0, 1);
        String last = initialState.substring(initialState.length() - 1);

        if (first.equals(last)) {
            frequency.put(first, 2L);
        } else {
            frequency.put(first, 1L);
            frequency.put(last, 1L);
        }

        for (String key : state.keySet()) {
            Long v = state.get(key);

            String key1 = key.substring(0, 1);
            Long val1 = frequency.get(key1);
            frequency.put(key1, v + val1);

            String key2 = key.substring(1, 2);
            Long val2 = frequency.get(key2);
            frequency.put(key2, v + val2);
        }

        List<Long> values = frequency.values().stream().sorted().toList();
        return (values.get(values.size() - 1) - values.get(0)) / 2L;
    }

    public static String doIt(List<String> input) {
        String p = input.get(0);
        parse(input);
        Map<String, Long> state = initialize(p);

        for (int i = 1; i <= 10; i++) {
            state = step(state);
        }
        long part1 = result(state, p);

        for (int i = 11; i <= 40; i++) {
            state = step(state);
        }
        long part2 = result(state, p);

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }
}
