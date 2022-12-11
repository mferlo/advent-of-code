package advent2021;

import java.util.*;

class Player {
    public int pos;
    public int score;

    public Player(int p) {
        pos = p;
        score = 0;
    }

    public int move(int roll) {
        pos += roll;
        pos %= 10;
        if (pos == 0) {
            pos = 10;
        }
        score += pos;
        return score;
    }
}

class Game {
    Player p1, p2;

    int numRolls, die;

    public Game(int p1Pos, int p2Pos) {
        p1 = new Player(p1Pos);
        p2 = new Player(p2Pos);

        numRolls = 0;
        die = 1;
    }

    int roll() {
        numRolls++;
        int result = die;
        die++;
        if (die > 100) {
            die = 1;
        }
        return result;
    }

    int roll3() {
        return roll() + roll() + roll();
    }

    public int play() {
        while (true) {
            if (p1.move(roll3()) >= 1000) {
                return p2.score * numRolls;
            }
            if (p2.move(roll3()) >= 1000) {
                return p1.score * numRolls;
            }
        }
    }
}

record State(int p1, int p2, int s1, int s2, boolean p1Turn) {

    public static State init(int p1, int p2) {
        return new State(p1, p2, 0, 0, true);
    }

    static int advance(int cur, int roll) {
        int next = cur + roll;
        if (cur + roll > 10) {
            next -= 10;
        }
        return next;
    }

    State p1Move(int roll) {
        int newP1 = advance(p1, roll);
        return new State(newP1, p2, s1 + newP1, s2, false);
    }

    State p2Move(int roll) {
        int newP2 = advance(p2, roll);
        return new State(p1, newP2, s1, s2 + newP2, true);
    }

    public List<State> nextTurn() {
        List<State> result = new ArrayList<>();

        for (int i = 1; i <= 3; i++) {
            for (int j = 1; j <= 3; j++) {
                for (int k = 1; k <= 3; k++) {
                    result.add(p1Turn ? p1Move(i + j + k) : p2Move(i + j + k));
                }
            }
        }

        return result;
    }
}

class DiracGame {
    Map<State, Long> states;
    long p1Wins;
    long p2Wins;
    public DiracGame(int p1, int p2) {
        State startingState = State.init(p1, p2);
        states = new HashMap<>();
        states.put(startingState, 1L);
        p1Wins = 0;
        p2Wins = 0;
    }

    void incr(Map<State, Long> map, State state, Long value) {
        Long current;
        if (map.containsKey(state)) {
            current = map.get(state);
        } else {
            current = 0L;
        }

        map.put(state, current + value);
    }

    void step() {
        Map<State, Long> newStates = new HashMap<>();
        for (var state : states.keySet()) {
            Long count = states.get(state);
            if (state.s1() >= 21) {
                p1Wins += count;
            } else if (state.s2() >= 21) {
                p2Wins += count;
            } else {
                for (State newState : state.nextTurn()) {
                    incr(newStates, newState, count);
                }
            }
        }
        states = newStates;
    }

    public long play() {
        while (!states.isEmpty()) {
            step();
        }
        return Math.max(p1Wins, p2Wins);
    }
}

public class Day21 {

    public static String doIt(List<String> input) {
        int pawn1 = Integer.parseInt(input.get(0).substring(input.get(0).length() - 1));
        int pawn2 = Integer.parseInt(input.get(1).substring(input.get(1).length() - 1));

        Game game1 = new Game(pawn1, pawn2);
        DiracGame game2 = new DiracGame(pawn1, pawn2);

        long part1 = game1.play();
        long part2 = game2.play();

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }
}