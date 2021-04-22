using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    class Passport
    {
        Dictionary<string, string> Fields;

        static Regex keyValue = new Regex(@"(...):(\S+)");
        public static Passport Parse(string lines)
        {
            var fields = new Dictionary<string, string>();
            foreach (var groups in keyValue.Matches(lines).Select(m => m.Groups))
            {
                fields[groups[1].Value] = groups[2].Value;
            }

            return new Passport { Fields = fields };
        }

        static HashSet<string> AllFields = new HashSet<string>{ "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" };
        string ValueOf(string field) => Fields.ContainsKey(field) ? Fields[field] : "null";
        public override string ToString() => string.Join(" ", AllFields.Select(field => $"{field}: {ValueOf(field)}"));

        static HashSet<string> AllPart1Fields = new HashSet<string>{ "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
        public bool ValidPart1() => AllPart1Fields.Intersect(Fields.Keys).Count() == AllPart1Fields.Count;

        static Regex year = new Regex(@"^\d{4}$");
        static Regex height = new Regex(@"^(\d{2,3})(cm|in)$");
        static Regex hcl = new Regex(@"^#[0-9a-f]{6}$");
        static List<string> validEcl = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
        static Regex pid = new Regex(@"^\d{9}$");

        public bool ValidPart2()
        {
            if (!ValidPart1())
                return false;

            // byr (Birth Year) - four digits; at least 1920 and at most 2002.
            // iyr (Issue Year) - four digits; at least 2010 and at most 2020.
            // eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
            if (!(year.IsMatch(Fields["byr"]) && year.IsMatch(Fields["iyr"]) && year.IsMatch(Fields["eyr"])))
                return false;
            var byr = int.Parse(Fields["byr"]);
            var iyr = int.Parse(Fields["iyr"]);
            var eyr = int.Parse(Fields["eyr"]);
            if (!(1920 <= byr && byr <= 2002)) return false;
            if (!(2010 <= iyr && iyr <= 2020)) return false;
            if (!(2020 <= eyr && eyr <= 2030)) return false;

            // hgt (Height) - a number followed by either cm or in:
            //    If cm, the number must be at least 150 and at most 193.
            //    If in, the number must be at least 59 and at most 76.
            var match = height.Match(Fields["hgt"]);
            if (!match.Success) return false;
            var h = int.Parse(match.Groups[1].Value);
            if (match.Groups[2].Value == "cm" && !(150 <= h && h <= 193)) return false;
            if (match.Groups[2].Value == "in" && !(59 <= h && h <= 76)) return false;

            // hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
            if (!hcl.IsMatch(Fields["hcl"])) return false;

            // ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
            if (!validEcl.Contains(Fields["ecl"])) return false;

            // pid (Passport ID) - a nine-digit number, including leading zeroes.
            if (!pid.IsMatch(Fields["pid"])) return false;

            return true;
        }
    }

    class Day04
    {
        static List<Passport> Input;

        public static void Parse() => 
            Input = File.ReadAllText("04.txt").Split("\n\n").Select(lines => Passport.Parse(lines)).ToList();

        public static object Part1() => Input.Count(passport => passport.ValidPart1());
        public static object Part2() => Input.Count(passport => passport.ValidPart2());
    }
}