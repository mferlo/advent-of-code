package advent2021;

import java.util.ArrayList;
import java.util.List;

class Program {
    List<int[]> data;

    public Program(List<String> input) {
        data = new ArrayList<>();
        for (int i = 0; i < input.size(); i += 18) {
            data.add(parseSnippet(input.subList(i, i + 18)));
        }
    }

    int[] parseSnippet(List<String> input) {
        int[] result = new int[3];
        result[0] = Integer.parseInt(input.get(3).split(" ")[2]);
        result[1] = Integer.parseInt(input.get(4).split(" ")[2]);
        result[2] = Integer.parseInt(input.get(15).split(" ")[2]);
        return result;
    }

    int[] digits(long n) {
        int[] result = new int[14];
        for (int i = 13; i >= 0; i--) {
            result[i] = (int)(n % 10);
            n /= 10;
        }
        return result;
    }

    int doFragment(int digit, int[] data, int z) {
        z /= data[0]; // sometimes 26, sometimes 1

        int x, y;
        if (data[1] + z % 26 == digit) {
            x = 0;
        } else {
            x = 1;
            z *= 26;
        }

        // We need x to be 0 here in order to avoid incrementing the checksum
        y = x * (digit + data[2]);
        return z + y;
    }

    boolean tryNumber(long n) {
        int[] digits = digits(n);
        for (int i = 0; i < 14; i++) {
            if (digits[i] == 0) {
                return false;
            }
        }

        int z = 0;
        for (int i = 0; i < 14; i++) {
            z = doFragment(digits[i], data.get(i), z);
        }
        return z == 0;
    }

    int solveDigit(int[] data) {
        for (int d = 9; d >= 1; d--) {
            if (doFragment(d, data, 0) == 0) {
                return d;
            }
        }
        throw new IllegalStateException();
    }

    String solve() {
        String result = "";
        int z = 0;

        for (int i = 0; i < 14; i++) {
            result += solveDigit(data.get(i));
        }
        return result;
    }
}

public class Day24 {

    public static String doIt(List<String> input) {
        Program program = new Program(input);

        String part1 = program.solve();

        long part2 = 0;

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }
}