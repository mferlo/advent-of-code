using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _04
{
    struct Sleep {
        public int From;
        public int To;
        public int Duration => To - From;
        public bool Asleep(int minute) => From <= minute && minute < To;
        public override string ToString() => $"{From}-{To}";
    }

    class Guard {
        public int Id;
        public string Date;
        public IList<Sleep> Sleeps;
        public int SleepyTime => Sleeps.Sum(s => s.Duration);
        public bool Asleep(int minute) => Sleeps.Any(s => s.Asleep(minute));
        public override string ToString() => $"{Id} {Date} {String.Join(", ", Sleeps)}";
    }

    class Program
    {
        static bool IsGuard(string line) => line.Contains("Guard");

        static Guard ParseGuard(string line) {
            var id = int.Parse(line.Split(' ')[3].Substring(1));
            var date = line.Substring(line.IndexOf('-') + 1, length: 5);
            return new Guard { Id = id, Date = date, Sleeps = new List<Sleep>() };
        }

        static IEnumerable<Guard> ParseFile() {
            var lines = File.ReadLines("input.txt");
            var x = lines.ToList();
            var i = 0;
            Guard g = new Guard();
            while (i < x.Count) {
                if (IsGuard(x[i])) {
                    g = ParseGuard(x[i]);
                    i += 1;
                }
                while (i < x.Count && !IsGuard(x[i])) {
                    var sleepStart = x[i].Substring(15, length: 2);
                    var sleepEnd = x[i+1].Substring(15, length: 2);
                    i += 2;
                    var sleep = new Sleep { From = int.Parse(sleepStart), To = int.Parse(sleepEnd) };
                    g.Sleeps.Add(sleep);
                }
                yield return g;
            }
        }

        static IList<Guard> MostMinutesAsleep(IList<Guard> guards) {
            var slept = new Dictionary<int, int>();
            foreach (var g in guards) {
                slept.TryAdd(g.Id, 0);
                slept[g.Id] += g.SleepyTime;
            }
            var maxValue = slept.Max(kvp => kvp.Value);
            var id = slept.Single(kvp => kvp.Value == maxValue).Key;
            return guards.Where(g => g.Id == id).ToList();
        }

        struct SleepiestMinute {
            public int Minute;
            public int NumSleeps;
        }

        static SleepiestMinute MinuteMostSlept(IList<Guard> guards) {
            int minute = 0;
            int value = -1;
            for (int i = 0; i <= 59; i++) {
                var x = guards.Count(g => g.Asleep(i));
                if (x > value) {
                    value = x;
                    minute = i;
                }
            }
            return new SleepiestMinute { Minute = minute, NumSleeps = value };
        }

        static string Part1(IList<Guard> guards) {
            var sleepGuardLogs = MostMinutesAsleep(guards);
            var id = sleepGuardLogs.First().Id;
            var sleepyMinute = MinuteMostSlept(sleepGuardLogs).Minute;
            return (id * sleepyMinute).ToString();
        }

        static string Part2(IList<Guard> guards) {
            var groupedGuards = guards.GroupBy(g => g.Id);
            string answer = null;
            var bestValue = 0;
            foreach (var group in groupedGuards) {
                var id = group.Key;
                var value = MinuteMostSlept(group.ToList());
                if (value.NumSleeps > bestValue) {
                    bestValue = value.NumSleeps;
                    answer = (value.Minute * id).ToString();
                }
            }
            return answer;
        }

        static void Main(string[] args)
        {
            var guards = ParseFile().ToList();
            Console.WriteLine(Part1(guards));
            Console.WriteLine(Part2(guards));
        }
    }
}
