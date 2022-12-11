package advent2021;

import java.util.*;

record Point3(int x, int y, int z) {
    public boolean inside(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax) {
        return xMin <= x && x <= xMax && yMin <= y && y <= yMax && zMin <= z && z <= zMax;
    }
}

record Region(boolean state, int xMin, int xMax, int yMin, int yMax, int zMin, int zMax) {
    public static Region parse(String input) {
        boolean state = input.startsWith("on");
        int[] parts = Arrays.stream(input.split("[^0-9-]")).filter(s -> !s.isEmpty()).mapToInt(Integer::parseInt).toArray();
        return new Region(state, parts[0], parts[1], parts[2], parts[3], parts[4], parts[5]);
    }

    public boolean usedInPart1() {
        return -50 <= xMin && xMin <= 50;
    }

    public List<Point3> points() {
        List<Point3> results = new ArrayList<>();
        for (int x = xMin; x <= xMax; x++) {
            for (int y = yMin; y <= yMax; y++) {
                for (int z = zMin; z <= zMax; z++) {
                    results.add(new Point3(x, y, z));
                }
            }
        }
        return results;
    }
}

public class Day22 {

    public static String doIt(List<String> input) {
        List<Region> regions = input.stream().map(Region::parse).toList();

        Map<Point3, Boolean> state = new HashMap<>();
        for (var region : regions) {
            if (region.usedInPart1()) {
                boolean s = region.state();
                for (var point : region.points()) {
                    state.put(point, s);
                }
            }
        }

        long part1 = state.values().stream().filter(Boolean::booleanValue).count();
        long part2 = 0;

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }
}