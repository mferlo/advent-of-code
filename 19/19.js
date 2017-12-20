'use strict';

let height, width;
const grid = [];
const solution = [];

const scale = 10;
const paint = (img, x, y) => img.fillRect(x*scale, y*scale, scale, scale);

const initImage = () => {
  const img = document.getElementById('c').getContext('2d');
  img.fillStyle = 'gray';

  for (let y = 0; y < height; y++) {
    for (let x = 0; x < width; x++) {
      if (grid[y][x] !== ' ') {
        paint(img, x, y); // Coordinates are weird
      }
    }
  }
};

const animateSolution = () => {
  document.getElementById('go').disabled = true;
  // FIXME: Add delay, animate letters
  const img = document.getElementById('c').getContext('2d');
  const part1 = document.getElementById('part1');
  let i = 0;

  img.fillStyle = 'black';
  for (const step of solution) {
    i++;
    paint(img, step.x, step.y);

    if (step.letter) {
      part1.innerHTML = part1.innerHTML + step.letter;
    }

    part2.innerHTML = i;
  }
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

const solve = () => {
  // FIXME, find path through maze
  for (let i = 0; i < 15; i++) {
    solution.push({ x: i, y: i });
  }

  solution[0].letter = 'H';
  solution[5].letter = 'W';

  animateSolution();
}

const init = () => {
  populateGrid();
  initImage();
};
