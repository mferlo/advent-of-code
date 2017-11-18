const weights = [ 1, 2, 3, 4, 5, 7, 8, 9, 10, 11 ];

//[ 1, 2, 3, 7, 11, 13, 17, 19, 23, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113 ];

const balancedWeight = weights.reduce((s, w) => s + w, 0) / 3;

function* generateAllCombos() {
  for (let i = 0; i < Math.pow(3, weights.length); i++) {
    yield i.toString(3).padStart(weights.length, '0').split('');
  }
}

function* filterValid() {
  for (const combo of generateAllCombos()) {
    const sums = [ 0, 0, 0 ];
    for (const [i, c] of combo.entries()) {
      sums[c] += weights[i];
    }

    if (sums.every(s => s === balancedWeight)) {
      yield {
        combo,
        packagesInFrontSeat: combo.filter(c => c === '0').length,
        quantumEntanglement: combo.reduce((qe, cur, i) => qe * (cur === '0' ? weights[i] : 1), 1)
      }
    }
  }
}

const test = [...generateAllCombos()];
console.log(test[0]);
console.log(test[test.length - 1]);

const sortBy = (a, b) => (a.packagesInFrontSeat - b.packagesInFrontSeat) || (a.quantumEntanglement - b.quantumEntanglement);

const validCombos = [...filterValid()].sort(sortBy);

console.log(JSON.stringify(validCombos[0]));
console.log(JSON.stringify(validCombos[1]));

/*
const validCombos = [...filterValid()];
console.log("Part 1: " + validCombos.length);

const answerPart2 = combos => {
  const distribution = Array(containers.length).fill(0);

  for (combo of combos) {
    const used = combo.reduce((sum, cur) => sum + cur, 0);
    distribution[used]++;
  }
  return distribution.find(x => x > 0);
}

console.log("Part 2: " + answerPart2(validCombos));
*/
