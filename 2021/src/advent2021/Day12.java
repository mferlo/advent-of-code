package advent2021;

import java.util.*;

class Cave {
    public String name;
    public boolean small;
    public List<Cave> connections;

    public Cave(String s) {
        name = s;
        small = s.equals(s.toLowerCase());
        connections = new ArrayList<>();
    }

    public void addConnection(Cave cave) {
        if (!connections.contains(cave)) {
            connections.add(cave);
        }
    }
}

public class Day12 {

    static Map<String, Cave> map;
    static boolean isPart1;

    static void maybeAdd(String name) {
        if (!map.containsKey(name)) {
            map.put(name, new Cave(name));
        }
    }

    static void parse(List<String> input) {
        map = new HashMap<>();

        var pairs = input.stream().map(i -> i.split("-")).toList();
        for (var p : pairs) {
            maybeAdd(p[0]);
            maybeAdd(p[1]);
        }

        for (var p : pairs) {
            Cave c1 = map.get(p[0]);
            Cave c2 = map.get(p[1]);
            c1.addConnection(c2);
            c2.addConnection(c1);
        }
    }

    static String pathString(List<Cave> path) {
        return String.join("-", path.stream().map(c -> c.name).toList());
    }

    static boolean canVisitNextCave(Cave nextCave, List<Cave> currentPath) {
        if (!nextCave.small) {
            return true;
        }

        if (nextCave.name.equals("start")) {
            return false;
        }

        if (!currentPath.contains(nextCave)) {
            return true;
        }

        if (isPart1) {
            return false;
        }

        // We're about to visit a small cave for the second time. Is this the first time we'll revisit a small cave?
        return currentPath.stream().filter(c -> c.small).map(c -> Collections.frequency(currentPath, c)).allMatch(freq -> freq == 1);
    }

    static void findPaths(Set<String> completedPaths, List<Cave> currentPath) {
        Cave currentCave = currentPath.get(currentPath.size() - 1);
        if (currentCave.name.equals("end")) {
            completedPaths.add(pathString(currentPath));
            return;
        }

        for (Cave nextCave : currentCave.connections) {
            if (canVisitNextCave(nextCave, currentPath)) {
                List<Cave> nextCurrentPath = new ArrayList<>(currentPath);
                nextCurrentPath.add(nextCave);
                findPaths(completedPaths, nextCurrentPath);
            }
        }
    }

    static Set<String> findPaths() {
        Set<String> paths = new HashSet<>();
        List<Cave> currentPath = new ArrayList<>();
        currentPath.add(map.get("start"));

        findPaths(paths, currentPath);
        return paths;
    }

    public static String doIt(List<String> input) {
        parse(input);

        isPart1 = true;
        long part1 = findPaths().size();

        isPart1 = false;
        long part2 = findPaths().size();

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }
}
