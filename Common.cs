using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    public static class Common
    {
        public static IList<IList<int>> Ints(string input) =>
            File.ReadAllLines(input).Select(IntLine).ToList();


        static Regex IntRegex = new Regex(@"-?\d+");
        static IList<int> IntLine(string line) =>
            IntRegex.Matches(line).Select(match => int.Parse(match.Value)).ToList();
    }
}