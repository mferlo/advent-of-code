package advent2021;

import java.util.List;
/*
interface Element {
    int getDepth();
    void setDepth(int d);
    boolean isNumber();
}

class Value implements Element {
    public long value;
    int depth;

    public Value(long n) {
        value = n;
    }

    public long getValue() {
        return value;
    }

    public int getDepth() {
        return depth;
    }

    public void setDepth(int d) {
        depth = d;
    }

    public boolean isNumber() {
        return true;
    }

    public String toString() {
        return String.valueOf(value);
    }
}

class Pair implements Element {

    Element e1, e2;
    int depth;

    public Pair(Element e1, Element e2) {
        this.e1 = e1;
        this.e2 = e2;
    }

    public boolean Reduce() {
        boolean reductionHappened = false;
        do {
            if (!e1.isNumber() && e1.getDepth() == 4) {
                ((Pair) e1).explode();
                reductionHappened = true;
            } else if (e1.reduce()) {
            } else if (!e2.isNumber() && e2.getDepth() == 4) {
                ((Pair)e2).explode();
                reductionHappened = true;
            } else {

            }

        } while (reductionHappened);

    }

    void explode() {

    }

    public int getDepth() {
        return depth;
    }

    public void setDepth(int d) {
        depth = d;
        e1.setDepth(d + 1);
        e2.setDepth(d + 1);
    }

    public boolean isNumber() {
        return false;
    }

    public String toString() {
        return "[" + e1.toString() + "," + e2.toString() + "]";
    }
}
*/
public class Day18 {

    public static String doIt(List<String> input) {
        long part1 = 0;
        long part2 = 0;

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }
}
