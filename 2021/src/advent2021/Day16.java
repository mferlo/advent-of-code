package advent2021;

import java.util.ArrayList;
import java.util.List;

class BitHelper {
    static int[] hexToBits(String s) {
        int[] result = new int[4*s.length()];
        for (int i = 0; i < s.length(); i++) {
            int hexValue = Integer.parseInt(s.substring(i, i+1), 16);
            result[4*i] = (hexValue & 0b1000) == 0b1000 ? 1 : 0;
            result[4*i + 1] = (hexValue & 0b0100) == 0b0100 ? 1 : 0;
            result[4*i + 2] = (hexValue & 0b0010) == 0b0010 ? 1 : 0;
            result[4*i + 3] = (hexValue & 0b0001) == 0b0001 ? 1 : 0;
        }
        return result;
    }

    static long fromBits(int[] bits, int start, int length) {
        long result = 0;
        for (int i = start; i < start + length; i++) {
            result <<= 1;
            result |= bits[i];
        }
        return result;
    }
}

class ParseResult<T> {
    public T result;
    public int nextIndex;

    public ParseResult(T result, int nextIndex) {
        this.result = result;
        this.nextIndex = nextIndex;
    }
}

class PacketParser {
    static Packet parse(String s) {
        int[] bits = BitHelper.hexToBits(s);

        int i = 0;
        var packet = parse(bits, i);
        if (moreToParse(bits, packet.nextIndex)) {
            throw new IllegalStateException();
        }

        return packet.result;
    }

    static boolean moreToParse(int[] bits, int start) {
        for (int i = start; i < bits.length; i++) {
            if (bits[i] == 1) {
                return true;
            }
        }
        return false;
    }

    static ParseResult<Long> parseLiteralValue(int[] bits, int startIndex) {
        long value = 0;
        boolean readNextGroup = true;
        int i = startIndex;

        while (readNextGroup) {
            readNextGroup = bits[i] == 1;
            value <<= 4;
            value |= BitHelper.fromBits(bits, i + 1, 4);
            i += 5;
        }

        return new ParseResult<>(value, i);
    }

    static ParseResult<Packet> parse(int[] bits, int startIndex) {
        int i = startIndex;
        int version = (int)BitHelper.fromBits(bits, i, 3);
        i += 3;
        int typeId = (int)BitHelper.fromBits(bits, i, 3);
        i += 3;

        Packet packet = new Packet(version, typeId);

        if (typeId == 4) {
            var literalResult = parseLiteralValue(bits, i);
            packet.value = literalResult.result;
            i = literalResult.nextIndex;
        } else {
            int lengthTypeId = bits[i];
            i++;

            List<Packet> subpackets = new ArrayList<>();

            if (lengthTypeId == 0) {
                int lengthOfSubpackets = (int)BitHelper.fromBits(bits, i, 15);
                i += 15;
                int iStop = i + lengthOfSubpackets;
                while (i < iStop) {
                    var subpacket = parse(bits, i);
                    subpackets.add(subpacket.result);
                    i = subpacket.nextIndex;
                }
            } else {
                int numberOfSubpackets = (int)BitHelper.fromBits(bits, i, 11);
                i += 11;
                for (int c = 0; c < numberOfSubpackets; c++) {
                    var subpacket = parse(bits, i);
                    subpackets.add(subpacket.result);
                    i = subpacket.nextIndex;
                }
            }

            packet.packets = subpackets;
        }

        return new ParseResult<>(packet, i);
    }
}

class Packet {
    public int version;
    public int typeId;
    public long value;
    public List<Packet> packets;

    public Packet(int version, int typeId) {
        this.version = version;
        this.typeId = typeId;
        this.packets = new ArrayList<>();
    }

    public long versionSum() {
        return version + packets.stream().mapToLong(Packet::versionSum).sum();
    }

    public long calculateValue() {
        value = switch (typeId) {
            case 0 -> packets.stream().mapToLong(Packet::calculateValue).sum();
            case 1 -> packets.stream().mapToLong(Packet::calculateValue).reduce(1L, (x, y) -> x * y);
            case 2 -> packets.stream().mapToLong(Packet::calculateValue).min().getAsLong();
            case 3 -> packets.stream().mapToLong(Packet::calculateValue).max().getAsLong();
            case 4 -> value;
            case 5 -> packets.get(0).calculateValue() > packets.get(1).calculateValue() ? 1L : 0L;
            case 6 -> packets.get(0).calculateValue() < packets.get(1).calculateValue() ? 1L : 0L;
            case 7 -> packets.get(0).calculateValue() == packets.get(1).calculateValue() ? 1L : 0L;
            default -> throw new IllegalStateException("Unknown " + typeId);
        };
        return value;
    }

    public String toString() {
        return toString(0);
    }

    String toStringFlat() {
        return version + " (" + typeId + ") [" + value + "] ";
    }

    String toString(int depth) {
        String result = "";
        for (int i = 0; i < depth; i++) {
            result += "  ";
        }
        result += toStringFlat();
        if (packets.size() > 0) {
            result += "\n";
            result += String.join("\n", packets.stream().map(p -> p.toString(depth + 1)).toList());
        }
        return result;
    }
}

public class Day16 {

    public static String doIt(List<String> input) {
        String realInput = input.get(0);
        Packet p = PacketParser.parse(realInput);

        long part1 = p.versionSum();
        long part2 = p.calculateValue();

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }
}
