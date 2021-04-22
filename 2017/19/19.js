'use strict';

let height, width;
const grid = [];
const solution = [];

const scale = 2;
const paint = (img, x, y) => img.fillRect(x*scale, y*scale, scale, scale);

const initImage = () => {
  const canvas = document.getElementById('c');
  canvas.width = scale * width;
  canvas.height = scale * height;

  const img = canvas.getContext('2d');
  img.fillStyle = 'gray';

  for (let y = 0; y < height; y++) {
    for (let x = 0; x < width; x++) {
      if (grid[y][x] !== ' ') {
        paint(img, x, y);
      }
    }
  }
};

const solutionIterator = () => {
  const part1 = document.getElementById('part1');
  const part2 = document.getElementById('part2');
  const img = document.getElementById('c').getContext('2d');
  img.fillStyle = 'black';

  let i = 0;
  let done = false;
  return () => {
    if (done) {
      return;
    }
    const cur = solution[i++];
    if (cur === undefined) {
      done = true;
      return;
    }
    paint(img, cur.x, cur.y);
    if (cur.letter !== '+') {
      part1.innerHTML = part1.innerHTML + cur.letter;
    }
    part2.innerHTML = i;
  };
};

const animateSolution = () => {
  // FIXME: animate letters, make speed variable
  const iter = solutionIterator();
  setInterval(iter, 1);
};


const populateGrid = () => {
  const lines = input.split('\n');
  height = lines.length;
  width = lines[0].length;

  for (let y = 0; y < height; y++) {
    const line = lines[y];
    if (!line) {
      continue;
    }
    grid[y] = new Array(width);
    for (let x = 0; x < width; x++) {
      const ch = line[x];
      if (ch === '|' || ch === '-') {  // '+' is line, ' ' is nothing, plus letters
        grid[y][x] = '+';
      } else {
        grid[y][x] = ch;
      }
    }
  }
};

const findStart = () => {
  for (let x = 0; x < width; x++) {
    if (grid[0][x] !== ' ') {
      return { x, y: 0, dir: 'Down' };
    }
  }
};

// In this coord system, y=0 is at the top, and gets larger going down
const d = { 'Up': [ 0, -1 ], 'Down': [ 0, 1 ], 'Left': [ -1, 0 ], 'Right': [ 1, 0 ] };

const onPath = (p) => p.x >= 0 && p.x < width && p.y >= 0 && p.y < height && grid[p.y][p.x] !== ' ';

const turnLeft = (p) => {
  const newDir = p.dir === 'Up' ? 'Left'
        : p.dir === 'Left' ? 'Down'
        : p.dir === 'Down' ? 'Right'
        : 'Up';

  return { x: p.x + d[newDir][0], y: p.y + d[newDir][1], dir: newDir };
};

const turnRight = (p) => {
  const newDir = p.dir === 'Up' ? 'Right'
        : p.dir === 'Right' ? 'Down'
        : p.dir === 'Down' ? 'Left'
        : 'Up';

  return { x: p.x + d[newDir][0], y: p.y + d[newDir][1], dir: newDir };
};

const makeSolution = () => {
  if (solution.length) {
    return;
  }

  let p = findStart();

  while (p !== undefined) {
    solution.push({ ...p, letter: grid[p.y][p.x] });

    let moveStraight = { x: p.x + d[p.dir][0], y: p.y + d[p.dir][1], dir: p.dir };
    if (onPath(moveStraight)) {
      p = moveStraight;
    } else {
      // We can't go straight. We must be able to turn...
      const left = turnLeft(p);
      const right = turnRight(p);

      if (onPath(left)) {
        p = left;
      } else if (onPath(right)) {
        p = right;
      } else {
        p = undefined;
      }
    }
  }
};

const solve = () => {
  document.getElementById('solve').disabled = true;
  makeSolution();
  animateSolution();
};

const display = () => {
  document.getElementById('display').disabled = true;
  makeSolution();

  document.getElementById('part1').innerHTML = solution.map(s => s.letter).filter(l => l !== '+').join('');
  document.getElementById('part2').innerHTML = solution.length;
};

const init = () => {
  populateGrid();
  initImage();
};
