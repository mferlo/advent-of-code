package advent2021;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;

public class Main {

    public static List<String> ReadFile(String day) {
        String path = "C:\\Users\\Matt\\Desktop\\dev\\advent-code\\2021\\" + day + ".txt";
        try {
            return Files.readAllLines(Path.of(path));
        } catch (IOException ex) {
            System.out.println("Error for " + path);
            System.out.println(ex);
            throw new IllegalStateException();
        }
    }

    public static void main(String[] args) {
        String result = Day23.doIt(); //ReadFile("20"));
        System.out.println(result);
    }
}
