package advent2021;

import java.util.List;
import java.util.stream.Collectors;

class Coordinates {
    public int HorizontalPosition;
    public int Depth;
    public int Aim;
    public Coordinates() {
        HorizontalPosition = 0;
        Depth = 0;
        Aim = 0;
    }
}

class Command {
    private final String Direction;
    private final int Amount;

    public Command(String s) {
        var spl = s.split(" ");
        Direction = spl[0];
        Amount = Integer.parseInt(spl[1]);
    }

    public void execute1(Coordinates current) {
        switch (Direction) {
            case "forward" -> current.HorizontalPosition += Amount;
            case "down" -> current.Depth += Amount;
            case "up" -> current.Depth -= Amount;
            default -> throw new IllegalStateException("Unexpected value: " + Direction);
        }
    }

    public void execute2(Coordinates current) {
        switch (Direction) {
            case "forward" -> {
                current.HorizontalPosition += Amount;
                current.Depth += current.Aim * Amount;
            }
            case "down" -> current.Aim += Amount;
            case "up" -> current.Aim -= Amount;
            default -> throw new IllegalStateException("Unexpected value: " + Direction);
        }
    }

}

public class Day02 {
    static String doIt(List<String> input) {
        List<Command> commands = input.stream().map(Command::new).collect(Collectors.toList());

        var part1 = part1(commands);
        var part2 = part2(commands);

        return "Part 1: " + part1 + "; Part 2: " + part2;
    }

    static int part1(List<Command> commands) {
        final Coordinates c = new Coordinates();
        commands.forEach(command -> command.execute1(c));
        return c.HorizontalPosition * c.Depth;
    }

    static int part2(List<Command> commands) {
        final Coordinates c = new Coordinates();
        commands.forEach(command -> command.execute2(c));
        return c.HorizontalPosition * c.Depth;
    }
}
