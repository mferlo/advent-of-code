const input = [
  // Macro: C-i " C-e ", C-n C-a
];

const solveOne = () => {
  const checksum = n => input => {
    const counts = {};
    const inc = c => counts[c] ? counts[c] += 1 : counts[c] = 1;
    for (const ch of input) {
      inc(ch);
    }
    return Object.values(counts).some(x => x === n);
  };

  const is2 = checksum(2);
  const is3 = checksum(3);
  let c2 = 0;
  let c3 = 0;

  for (const i of input) {
    if (is2(i)) { c2 += 1; }
    if (is3(i)) { c3 += 1; }
  }
  return c2 * c3;
};

const solveTwo = () => {
  const diffByOne = (x, y) => {
    let foundDiff = null;
    for (let i = 0; i < x.length; i++) {
      if (x.charAt(i) !== y.charAt(i)) {
        if (foundDiff !== null) {
          return false;
        }
        foundDiff = i;
      }
    }

    if (foundDiff === null) {
      return false;
    } else {
      return x.slice(0, foundDiff) + x.slice(foundDiff + 1);
    }
  };

  let solution;
  for (const word of input) {
    for (const word2 of input) {
      if (solution = diffByOne(word, word2)) {
        return solution;
      }
    }
  }
};

console.log(solveOne());
console.log(solveTwo());
