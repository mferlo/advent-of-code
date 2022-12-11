package advent2021;

import java.util.*;
import java.util.stream.Collectors;

class SegmentDisplay {

    List<String> inputs;
    List<String> outputs;

    public SegmentDisplay(String s) {
        var parts = s.split(" \\| ");
        inputs = Arrays.stream(parts[0].split(" ")).map(SegmentDisplay::normalize).toList();
        outputs = Arrays.stream(parts[1].split(" ")).map(SegmentDisplay::normalize).toList();
    }

    public long part1() {
        return outputs.stream().mapToInt(String::length).filter(l -> l == 2 || l == 3 || l == 4 || l == 7).count();
    }

    static Set<Character> toSet(String s) {
        return s.chars().mapToObj(i -> (char)i).collect(Collectors.toSet());
    }

    Map<String, Integer> makeLookup() {
        String one = inputs.stream().filter(i -> i.length() == 2).findFirst().get();
        String seven = inputs.stream().filter(i -> i.length() == 3).findFirst().get();
        String four = inputs.stream().filter(i -> i.length() == 4).findFirst().get();
        String eight = inputs.stream().filter(i -> i.length() == 7).findFirst().get();

        Map<String, Integer> lookup = new HashMap<>();
        lookup.put(one, 1);
        lookup.put(seven, 7);
        lookup.put(four, 4);
        lookup.put(eight, 8);

        // Remaining: 0, 2, 3, 5, 6, 9
        // fiveSegments: 2, 3, 5 ; sixSegments: 0, 6, 9

        Map<String, Set<Character>> fiveSegmentSets = inputs.stream().filter(i -> i.length() == 5)
                .collect(Collectors.toMap(s -> s, SegmentDisplay::toSet));
        Map<String, Set<Character>> sixSegmentSets = inputs.stream().filter(i -> i.length() == 6)
                .collect(Collectors.toMap(s -> s, SegmentDisplay::toSet));

        // 4's segments are lit for 9 only; unlit for remainder
        Set<Character> fourSet = toSet(four);
        String nine = "";
        for (var foo : sixSegmentSets.entrySet()) {
            if (foo.getValue().containsAll((fourSet))) {
                nine = foo.getKey();
            }
        }
        if (nine.length() == 0) throw new IllegalStateException("9 not found");
        sixSegmentSets.remove(nine);
        lookup.put(nine, 9);

        Set<Character> tmp = toSet(seven);
        tmp.removeAll(toSet(one));
        if (tmp.size() != 1) throw new IllegalStateException("top segment not found");
        Character topSegment = tmp.stream().findFirst().get();

        tmp = toSet(nine);
        tmp.remove(topSegment);
        tmp.removeAll(fourSet);
        if (tmp.size() != 1) throw new IllegalStateException("bottom segment not found");
        Character bottomSegment = tmp.stream().findFirst().get();

        // Remaining: 0, 2, 3, 5, 6
        // fiveSegments: 2, 3, 5 ; sixSegments: 0, 6
        var tmpSixSegSets = sixSegmentSets.entrySet().stream().toList();
        var sixSegEntryA = tmpSixSegSets.get(0);
        var sixSegEntryB = tmpSixSegSets.get(1);
        var sixSegA = sixSegEntryA.getValue();
        var sixSegB = sixSegEntryB.getValue();

        // One of these is 0, the other is 6. If we take away "7" and the bottom,
        // the 6 will have 3 left and the 0 will have 2 left (with the middle segment being the diff)
        Set<Character> sevenSet = toSet(seven);
        sixSegA.removeAll(sevenSet);
        sixSegB.removeAll(sevenSet);
        sixSegA.remove(bottomSegment);
        sixSegB.remove(bottomSegment);

        String zero, six;
        Character middleSegment;
        if (sixSegA.size() == 3) {
            six = sixSegEntryA.getKey();
            zero = sixSegEntryB.getKey();
            sixSegA.removeAll(sixSegB);
            middleSegment = sixSegA.stream().findFirst().get();
        } else {
            six = sixSegEntryB.getKey();
            zero = sixSegEntryA.getKey();
            sixSegB.removeAll(sixSegA);
            middleSegment = sixSegB.stream().findFirst().get();
        }

        lookup.put(zero, 0);
        lookup.put(six, 6);

        // Remaining: 2, 3, 5 (all 5 segments)
        // 5 is the only one that has the top-left segment
        tmp = toSet(four);
        tmp.remove(middleSegment);
        tmp.removeAll(toSet(one));
        if (tmp.size() != 1) throw new IllegalStateException("Couldn't find top-left segment");
        Character topLeftSegment = tmp.stream().findFirst().get();

        String five = "";
        for (var foo : fiveSegmentSets.entrySet()) {
            if (foo.getValue().contains(topLeftSegment)) {
                five = foo.getKey();
            }
        }
        fiveSegmentSets.remove(five);
        lookup.put(five, 5);

        // Remaining: 2, 3
        tmp = toSet(one);
        tmp.retainAll(toSet(six));
        if (tmp.size() != 1) throw new IllegalStateException("Couldn't find bottom-right segment");
        Character bottomRightSegment = tmp.stream().findFirst().get();

        String three = "";
        for (var foo : fiveSegmentSets.entrySet()) {
            if (foo.getValue().contains(bottomRightSegment)) {
                three = foo.getKey();
            }
        }
        fiveSegmentSets.remove(three);
        lookup.put(three, 3);

        String two = fiveSegmentSets.keySet().stream().findFirst().get();
        lookup.put(two, 2);

        return lookup;
    }

    public int part2() {
        Map<String, Integer> lookup = makeLookup();
        int result = 0;
        for (String s : outputs) {
            int value = lookup.get(s);
            result *= 10;
            result += value;
        }
        return result;
    }

    static String normalize(String s) {
        char[] chars = s.toCharArray();
        Arrays.sort(chars);
        return new String(chars);
    }
}

public class Day08 {

    public static String doIt(List<String> input) {
        List<SegmentDisplay> displays = input.stream().map(SegmentDisplay::new).toList();

        long part1 = displays.stream().mapToLong(SegmentDisplay::part1).sum();
        long part2 = displays.stream().mapToLong(SegmentDisplay::part2).sum();

        return "Part 1: " + part1 + ". Part 2: " + part2;
    }
}