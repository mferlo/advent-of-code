package advent2021;

import java.util.*;

enum Result {
    Corrupted,
    Incomplete
}

class Analysis {
    public Result result;
    public long score;

    public Analysis(Character ch) {
        result = Result.Corrupted;
        score = switch (ch) {
            case ')' -> 3;
            case ']' -> 57;
            case '}' -> 1197;
            case '>' -> 25137;
            default -> throw new IllegalStateException("" + ch);
        };
    }

    public Analysis(Stack<Character> s) {
        result = Result.Incomplete;
        score = 0;
        while (!s.empty()) {
            score *= 5;
            score += switch (s.pop()) {
                case '(' -> 1;
                case '[' -> 2;
                case '{' -> 3;
                case '<' -> 4;
                default -> throw new IllegalStateException();
            };
        }
    }
}

public class Day10 {

    static Analysis analyze(String s) {
        Stack<Character> stack = new Stack<>();

        for (char ch : s.toCharArray()) {
            if (ch == '(' || ch == '{' || ch == '<' || ch == '[') {
                stack.push(ch);
            } else {
                char opening = stack.pop();
                char expected = switch (opening) {
                    case '(' -> ')';
                    case '{' -> '}';
                    case '<' -> '>';
                    case '[' -> ']';
                    default -> throw new IllegalStateException("unknown: " + opening);
                };
                if (ch != expected) {
                    return new Analysis(ch);
                }
            }
        }

        return new Analysis(stack);
    }

    public static String doIt(List<String> input) {
        List<Analysis> analysis = input.stream().map(Day10::analyze).toList();

        long part1 = analysis.stream().filter(a -> a.result == Result.Corrupted).mapToLong(a -> a.score).sum();

        long[] incompleteScores = analysis.stream().filter(a -> a.result == Result.Incomplete).mapToLong(a -> a.score).toArray();
        Arrays.sort(incompleteScores);
        long part2 = incompleteScores[incompleteScores.length / 2];

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }
}