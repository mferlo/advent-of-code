const target = 150;
const containers = [ 43, 3, 4, 10, 21, 44, 4, 6, 47, 41, 34, 17, 17, 44, 36, 31, 46, 9, 27, 38 ];

function* generateAllCombos() {
  for (let i = 0; i < Math.pow(2, containers.length); i++) {
    yield i.toString(2).padStart(containers.length, '0').split('').map(x => x === '1' ? 1 : 0);
  }
}

function* filterValid() {
  for (const combo of generateAllCombos()) {
    if (target === containers.reduce((sum, cur, i) => sum + (combo[i]*cur), 0)) {
      yield combo;
    }
  }
}

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
