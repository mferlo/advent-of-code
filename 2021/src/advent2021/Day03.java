package advent2021;

import java.util.ArrayList;
import java.util.List;

public class Day03 {

    static int[] parse(String s) {
        var count = s.length();
        var result = new int[count];
        for (int i = 0; i < count; i++) {
            result[i] = (s.charAt(i) == '0') ? 0 : 1;
        }
        return result;
    }

    public static String doIt(List<String> input) {
        List<int[]> bits = input.stream().map(Day03::parse).toList();

        int part1 = part1(bits);
        int part2 = part2(bits);

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }

    static int mostCommonBit(List<int[]> bits, int i) {
        int ones = 0;
        for (int[] entry : bits) {
            ones += entry[i];
        }
        int zeroes = bits.size() - ones;
        return (ones >= zeroes) ? 1 : 0;
    }

    public static int part1(List<int[]> bits) {
        int gamma = 0;
        int epsilon = 0;

        int max = bits.get(0).length;
        for (int i = 0; i < max; i++) {
            gamma <<= 1;
            epsilon <<= 1;

            if (mostCommonBit(bits, i) == 0) {
                epsilon++;
            } else {
                gamma++;
            }
        }

        return gamma * epsilon;
    }

    static int toInt(int[] bits) {
        int result = 0;
        for (int bit : bits) {
            result <<= 1;
            result += bit;
        }
        return result;
    }

    public static int part2(List<int[]> bits) {
        var oxygenBits = new ArrayList<>(bits);
        int i = 0;
        while (oxygenBits.size() > 1) {
            int mostCommonBit = mostCommonBit(oxygenBits, i);
            int finalI = i;
            oxygenBits.removeIf(b -> b[finalI] != mostCommonBit);
            i++;
        }

        var co2Bits = new ArrayList<>(bits);
        i = 0;
        while (co2Bits.size() > 1) {
            int mostCommonBit = mostCommonBit(co2Bits, i);
            int finalI = i;
            co2Bits.removeIf(b -> b[finalI] == mostCommonBit);
            i++;
        }

        return toInt(oxygenBits.get(0)) * toInt(co2Bits.get(0));
    }
}
