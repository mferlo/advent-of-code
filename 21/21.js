'use strict';

/* 01 --> 20
   23     31 */
const rotate2 = (rows) =>
  rows[1][0] + rows[0][0] + '/' +
  rows[1][1] + rows[0][1];

/* 012     630
   345 --> 741
   678     852 */
const rotate3 = (rows) =>
  rows[2][0] + rows[1][0] + rows[0][0] + '/' +
  rows[2][1] + rows[1][1] + rows[0][1] + '/' +
  rows[2][2] + rows[1][2] + rows[0][2];

const rotate = (s, n) => {
  switch (n) {
    case 2: return rotate2(s.split('/'));
    case 3: return rotate3(s.split('/'));
    default: throw `WTF: ${n}`;
  }
};

/* 01 --> 10
   23     32 */
const flip2 = (rows) =>
  rows[0][1] + rows[0][0] + '/' +
  rows[1][1] + rows[1][0];

/* 012     210
   345 --> 543
   678     876 */
const flip3 = (rows) =>
  rows[0][2] + rows[0][1] + rows[0][0] + '/' +
  rows[1][2] + rows[1][1] + rows[1][0] + '/' +
  rows[2][2] + rows[2][1] + rows[2][0];

const flip = (s, n) => {
  switch (n) {
    case 2: return flip2(s.split('/'));
    case 3: return flip3(s.split('/'));
    default: throw `WTF: ${n}`;
  }
};

const parseLine = (line) => {
  const [from, to] = line.split(' => ');
  const n = from.split('/')[0].length;

  const result = { n, to };

  const permutations = [ from, flip(from, n) ];
  let c = from;
  for (let i = 0; i < 3; i++) {
    c = rotate(c, n);
    const f = flip(c, n);
    permutations.push(c, f);
  }
  return { n, to, permutations };
};

const makeTransforms = (lines) => {
  const result = [ {}, {}, {}, {} ];
  for (const line of lines) {
    const { n, to, permutations } = parseLine(line);
    const size = result[n];
    for (const p of permutations) {
      size[p] = to;
    }
  }
  return result;
};

const transforms = makeTransforms(lines);

const init = () => {
  const grid = document.getElementById('grid');
  grid.innerHTML = "Hello,<br /> world!";
  console.log(transforms[2]);
  console.log(transforms[3]);
};

const go = () => {
  alert('!');
};
