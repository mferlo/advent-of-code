'use strict';

let grid, count;
const initialGrid = ['.#.', '..#', '###' ];

const byId = (id) => document.getElementById(id);
const display = () => {
  byId('grid').innerHTML = grid.join('<br />');
  byId('count').innerHTML = count;
};

const init = () => {
  grid = initialGrid;
  count = 0;
  display();
};

const getSubGrid = (i, j, n) => {
  const result = [];
  for (let row = 0; row < n; row++) {
    result[row] = grid[i+row].slice(j, j+n);
  }
  return result;
};

const populateNewGrid = (i, n, subGrid, newGrid) => {
  for (let row = 0; row < n; row++) {
    newGrid[i+row] += subGrid[row];
  }
};

const step = () => {
  const n = grid[0].length;
  const oldSize = n % 2 === 0 ? 2 : 3;
  const newSize = n % 2 === 0 ? 3 : 4;
  const numCells = n / oldSize;

  const newGrid = Array(n * newSize / oldSize).fill("");
  for (let i = 0; i < numCells; i++) {
    for (let j = 0; j < numCells; j++) {
      const oldSubGrid = getSubGrid(i*oldSize, j*oldSize, oldSize);
      const newSubGrid = transform(oldSubGrid);
      populateNewGrid(i*newSize, newSize, newSubGrid, newGrid);
    }
  }
  grid = newGrid;
  count++;
  display();
};

const runN = (n) => {
  init();
  for (let i = 0; i < n; i++) {
    step();
  }
  byId('answer').innerHTML = grid.join('').match(/#/g).length;
};

const part1 = () => runN(5);
const part2 = () => runN(18);
