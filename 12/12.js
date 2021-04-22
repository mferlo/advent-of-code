const step = {
  '#####': '#',
  '####.': '#',
  '###.#': '.',
  '###..': '#',
  '##.##': '#',
  '##.#.': '#',
  '##..#': '.',
  '##...': '#',
  '#.###': '#',
  '#.##.': '.',
  '#.#.#': '.',
  '#.#..': '.',
  '#..##': '#',
  '#..#.': '.',
  '#...#': '#',
  '#....': '.',
  '.####': '#',
  '.###.': '#',
  '.##.#': '#',
  '.##..': '#',
  '.#.##': '.',
  '.#.#.': '#',
  '.#..#': '.',
  '.#...': '.',
  '..###': '.',
  '..##.': '.',
  '..#.#': '#',
  '..#..': '#',
  '...##': '.',
  '...#.': '#',
  '....#': '.',
  '.....': '.'
};

let state = "#.#####.##.###...#...#.####..#..#.#....##.###.##...#####.#..##.#..##..#..#.#.#.#....#.####....#..#".split('');

const slack = 200;
for (let i = 0; i < slack; i++) {
  state.push('.');
}

for (let i = 0; i < 10; i++) {
  state.unshift('.');
}

const go = () => {
  const next = [ '.', '.' ];
  for (let i = 2; i < state.length - 2; i++) {
    const s = state.slice(i - 2, i + 3).join('');
    next[i] = step[s];
  }
  next.push('.');
  next.push('.');
  state = next;
};

/* Part 1
for (let count = 0; count < 20; count++) {
  go();
}

let score = 0;
for (let i = 0; i < state.length; i++) {
  if (state[i] === '#') {
    score += i - slack;
  }
}
console.log('Part 1:' + score);
*/

for (let count = 0; count < 183; count++) {
  go();
}

console.log(state.join(''));
go();
console.log(state.join(''));
go();
console.log(state.join('')); // 185
go();
console.log(state.join(''));
go();
console.log(state.join(''));

// at count === 185, we stabilize into a pattern with one solid ### block that advances to the right one space each turn. this is the string with the code above at line 185:
/*
...............................................................................................##################################################################################################################################################################################################...................
*/
// it begins at space 95 and ends at space 288, inclusive
// but we put in some slop at the beginning, so subtract 10
// so let start=85 and end=278
// the final block will go from start+delta to end+delta

const delta = 50000000000 - 185;
let score = 0;
for (let x = 85 + delta; x <= 278 + delta; x++) {
  score += x;
}
console.log(score);
