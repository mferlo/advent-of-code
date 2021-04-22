const input = [
  // Macro: C-e ", " C-d
];

// Part 1
console.log(input.reduce((a, b) => a + b));

// Part 2
let f = 0;
const freqs = new Set([0]);

while (true) {
  for (const delta of input) {
    f += delta;
    if (freqs.has(f)) {
      console.log(f);
      process.exit();
    }
    freqs.add(f);
  }
}
