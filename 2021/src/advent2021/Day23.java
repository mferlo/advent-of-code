package advent2021;

import java.util.*;

class AmphipodBurrow {
    static boolean debug = false;

    final char EMPTY = '.';
    final int A = 0;
    final int B = 1;
    final int C = 2;
    final int D = 3;
    int[] Costs = new int[] { 1, 10, 100, 1000 };

    final int height;
    char[][] rooms; // rooms[A][0] is closest room in left-most hallway. rooms[D][height] is farthest/right-most
    char[] hallway; // Hallway goes from L-R, with [2], [4], [6], and [8] being invalid spots
    boolean[] roomDone;

    public int cost;

    public AmphipodBurrow(List<String> rows) {
        height = rows.size();
        rooms = new char[4][height];

        hallway = new char[11];
        Arrays.fill(hallway, EMPTY);

        roomDone = new boolean[4];

        for (int i = 0; i < height; i++) {
            rooms[A][i] = rows.get(i).charAt(0);
            rooms[B][i] = rows.get(i).charAt(1);
            rooms[C][i] = rows.get(i).charAt(2);
            rooms[D][i] = rows.get(i).charAt(3);
        }
    }

    private AmphipodBurrow(char[][] rooms, char[] hallway, int cost) {
        this.rooms = rooms;
        this.hallway = hallway;
        this.cost = cost;
        height = rooms[0].length;

        roomDone = new boolean[4];
        Arrays.fill(roomDone, true);
        for (int room = 0; room < 4; room++) {
            for (int h = 0; h < height; h++) {
                char occupant = rooms[room][h];
                if (! (occupant == EMPTY || occupant == 'A' + room)) {
                    roomDone[room] = false;
                }
            }
        }
    }

    private AmphipodBurrow withMoveFromHallway(int hallwayPos, int moveCost) {
        char toMove = hallway[hallwayPos];
        int room = toMove - 'A';
        int destinationHeight = height;
        do {
            destinationHeight--;
        } while (rooms[room][destinationHeight] != EMPTY);

        char[][] newRooms = Arrays.stream(rooms).map(char[]::clone).toArray(char[][]::new);
        newRooms[room][destinationHeight] = toMove;

        char[] newHallway = hallway.clone();
        newHallway[hallwayPos] = EMPTY;

        int newCost = cost + moveCost;

        return new AmphipodBurrow(newRooms, newHallway, newCost);
    }

    public boolean isSolved() {
        for (int i = 0; i < height; i++) {
            if (rooms[A][i] != 'A') return false;
            if (rooms[B][i] != 'B') return false;
            if (rooms[C][i] != 'C') return false;
            if (rooms[D][i] != 'D') return false;
        }
        return true;
    }

    private int calculateCost(int hallwayPos, char candidate) {
        int targetRoom = candidate - 'A';
        int normalizedRoomPosition = 2 * (targetRoom + 1);
        int horizontalDistance = Math.abs(hallwayPos - normalizedRoomPosition);
        int verticalDistance = 0;
        for (int i = 0; i < height; i++) {
            if (rooms[targetRoom][i] == EMPTY) {
                verticalDistance++;
            }
        }

        int totalDistance = horizontalDistance + verticalDistance;
        return totalDistance * Costs[targetRoom];
    }

    private boolean pathClear(int hallwayPos) {
        int targetRoom = hallway[hallwayPos] - 'A';
        int targetRoomPosition = 2 * (targetRoom + 1);

        int start, increment;
        if (hallwayPos < targetRoomPosition) {
            start = hallwayPos + 1;
            increment = 1;
        } else {
            start = hallwayPos - 1;
            increment = -1;
        }

        for (int pos = start; pos != targetRoomPosition; pos += increment) {
            if (hallway[pos] != EMPTY) {
                return false;
            }
        }
        return true;
    }

    private boolean roomCanReceive(char candidate) {
        int targetRoom = candidate - 'A';
        for (int i = 0; i < height; i++) {
            if (! (rooms[targetRoom][i] == EMPTY || rooms[targetRoom][i] == candidate)) {
                return false;
            }
        }
        return true;
    }

    private List<AmphipodBurrow> nextStatesMoveFromHallway() {
        List<AmphipodBurrow> results = new ArrayList<>();

        for (int pos = 0; pos < 11; pos++) {
            char candidate = hallway[pos];
            if (candidate == EMPTY) {
                continue;
            }
            if (roomCanReceive(candidate) && pathClear(pos)) {
                var moveCost = calculateCost(pos, hallway[pos]);
                results.add(withMoveFromHallway(pos, moveCost));

                if (debug) {
                    System.out.println("Current");
                    System.out.println(this);
                    System.out.println("Moving '" + hallway[pos] + "' from hallway position " + pos + " to its room at cost " + moveCost);
                    System.out.println(results.get(results.size() - 1));
                    System.out.println();
                }
            }
        }

        return results;
    }


    private AmphipodBurrow withMoveToHallway(int hallwayPos, int room, int roomHeight, int moveCost) {
        char toMove = rooms[room][roomHeight];

        char[][] newRooms = Arrays.stream(rooms).map(char[]::clone).toArray(char[][]::new);
        newRooms[room][roomHeight] = EMPTY;

        char[] newHallway = hallway.clone();
        newHallway[hallwayPos] = toMove;

        int newCost = cost + moveCost;

        return new AmphipodBurrow(newRooms, newHallway, newCost);
    }

    int[] validHallwayPositions = new int[] { 0, 1, 3, 5, 7, 9, 10 };

    private int findTopRoomIndex(int room) {
        if (roomDone[room]) {
            return -1;
        }

        for (int i = 0; i < height; i++) {
            if (rooms[room][i] != EMPTY) {
                return i;
            }
        }
        return -1;
    }

    private int calculateCost(int room, int roomHeight, int hallwayPos) {
        char candidate = rooms[room][roomHeight];
        int normalizedRoomPosition = 2 * (room + 1);
        int horizontalDistance = Math.abs(hallwayPos - normalizedRoomPosition);
        int verticalDistance = roomHeight + 1;
        int totalDistance = horizontalDistance + verticalDistance;
        return totalDistance * Costs[candidate - 'A'];
    }

    private List<AmphipodBurrow> nextStatesMoveToHallway() {
        List<AmphipodBurrow> results = new ArrayList<>();

        int[] roomIndexes = new int[4];
        for (int i = 0; i < 4; i++) {
            roomIndexes[i] = findTopRoomIndex(i);
        }

        for (int validHallwayPosition : validHallwayPositions) {
            if (hallway[validHallwayPosition] == EMPTY) {
                for (int room = 0; room < 4; room++) {
                    if (roomIndexes[room] != -1) {
                        int moveCost = calculateCost(room, roomIndexes[room], validHallwayPosition);
                        results.add(withMoveToHallway(validHallwayPosition, room, roomIndexes[room], moveCost));

                        if (false && debug) {
                            System.out.println("Moving '" + rooms[room][roomIndexes[room]] + "' from top of room " + room + " to hallway position " + validHallwayPosition + " at cost " + moveCost);
                            System.out.println(results.get(results.size() - 1));
                        }
                    }
                }
            }
        }
        return results;
    }

    public List<AmphipodBurrow> makeNextStates() {
        List<AmphipodBurrow> nextStates = new ArrayList<>();
        nextStates.addAll(nextStatesMoveFromHallway());
        nextStates.addAll(nextStatesMoveToHallway());
        return nextStates;
    }

    final String wallFormat = "#############\n";
    final String hallwayFormat = "#%s#\n";
    final String roomFormat = "###%c#%c#%c#%c###\n";

    public String toString() {
        String roomStrings = "";
        for (int i = 0; i < height; i++) {
            roomStrings += String.format(roomFormat, rooms[A][i], rooms[B][i], rooms[C][i], rooms[D][i]);
        }

        return cost + "\n" + wallFormat + String.format(hallwayFormat, String.valueOf(hallway)) + roomStrings + wallFormat;
    }
}

public class Day23 {

    static void solve(AmphipodBurrow state, List<Integer> solutions) {
        if (state.isSolved()) {
            solutions.add(state.cost);
            return;
        }

        for (AmphipodBurrow nextState : state.makeNextStates()) {
            solve(nextState, solutions);
        }
    }

    static int solve(AmphipodBurrow startState) {
        int minCost = Integer.MAX_VALUE;
        Queue<AmphipodBurrow> states = new LinkedList<>();
        states.add(startState);

        while (!states.isEmpty()) {
            var cur = states.poll();
            if (cur.isSolved()) {
                System.out.println("Found a solution with cost " + cur.cost);
                if (cur.cost < minCost) {
                    minCost = cur.cost;
                }
                continue;
            }

            if (cur.cost > 11_000) {
                continue;
            }

            states.addAll(cur.makeNextStates());
        }
        return minCost;
    }

    public static String doIt() {
        String input1 = "CBDD";
        String input2 = "BCAA";

        String part2Input1 = "DCBA";
        String part2Input2 = "DBAC";

        var burrow1 = new AmphipodBurrow(List.of(input1, input2));
        var part2 = new AmphipodBurrow(List.of(input1, part2Input1, part2Input2, input2));

        //AmphipodBurrow.debug = true;
        int part1 = solve(burrow1);

        return "Part 1:\n" + part1 + "\n\nPart 2:\n" + part2;
    }
}
