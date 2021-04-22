using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent {

	public class Day10 {
		public static void Main() {
			new Factory(File.ReadAllLines("input.txt")).Process();
		}
	}

	public class Factory {
		public IDictionary<int, Bot> Bots;
		public IDictionary<int, int> Outputs;

		public Factory(IList<string> input) {
			Outputs = new Dictionary<int, int>();
			Bots = input
				.Where(i => i.StartsWith("bot"))
				.Select(line => new Bot(line))
				.ToDictionary(b => b.Number);

			foreach (var line in input.Where(i => i.StartsWith("value"))) {
				var match = Regex.Match(line, @"value (\d+) goes to bot (\d+)");
				var value = Int32.Parse(match.Groups[1].Value);
				var bot = Int32.Parse(match.Groups[2].Value);
				Bots[bot].AddChip(value);
			}
		}

		public void Process() {
			var bot = Bots.Values.FirstOrDefault(b => b.HasTwoChips);
			while (bot != null) {
				if (bot.LowChip == 17 && bot.HighChip == 61) {
					Console.WriteLine(bot.Number);
				}
				bot.Act(Bots, Outputs);
				bot = Bots.Values.FirstOrDefault(b => b.HasTwoChips);
			}

			Console.WriteLine(Outputs[0] * Outputs[1] * Outputs[2]);
		}
	}

	public class Bot {
		public int Number { get; set; }

		public int? LowChip;
		public int? HighChip;
		public bool HasTwoChips => HighChip != null;

		struct Target { 
			public Target(string type, string number) {
				Type = type;
				Number = Int32.Parse(number);
			}
			public string Type;
			public int Number;
		}

		private Target low;
		private Target high;

		public Bot(string definition) {
			var m = Regex.Match(
				definition,
				@"bot (\d+) gives low to (bot|output) (\d+) and high to (bot|output) (\d+)");
			this.Number = Int32.Parse(m.Groups[1].Value);
			this.low = new Target(m.Groups[2].Value, m.Groups[3].Value);
			this.high = new Target(m.Groups[4].Value, m.Groups[5].Value);
			this.LowChip = null;
			this.HighChip = null;
		}

		public void AddChip(int chip) {
			if (LowChip == null) {
				LowChip = chip;
			} else if (LowChip < chip) {
				HighChip = chip;
			} else {
				HighChip = LowChip;
				LowChip = chip;
			}
		}

		public void Act(
			IDictionary<int, Bot> bots,
			IDictionary<int, int> outputs) {

			if (low.Type == "bot") {
				bots[low.Number].AddChip(LowChip.Value);
			} else {
				outputs[low.Number] = LowChip.Value;
			}

			if (high.Type == "bot") {
				bots[high.Number].AddChip(HighChip.Value);
			} else {
				outputs[high.Number] = HighChip.Value;
			}

			LowChip = null;
			HighChip = null;
		}
	}
}
