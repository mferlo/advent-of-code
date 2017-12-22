'use strict';

const transforms = makeTransforms(lines);

const initialGrid = '.#./..#/###';
let grid;

const display = () =>
  document.getElementById('grid').innerHTML =
    grid.replace(new RegExp('/', 'g'), '<br />');

const init = () => {
  grid = initialGrid;
  display();
};

// const asString = (g) => g.map(r => r.join('')).join('/');

/* splitSubGrids3:
   * put in 2d array `g`
   * for (i = 0; i < n; i += 3)
   *   for (j = 0; j < n; j += 3)
   *     yield return asString(g[i + 0,1,2][j + 0,1,2])
   */

const splitSubGrids2 = (grid, n) => grid;
const splitSubGrids3 = (grid, n) => grid;

/* joinSubGrids3:
   * create 2d array `g`
   * for (i = 0; i < n; i += 3)
   *   for (j = 0; j < n; j += 3)
   *     g[i + 0,1,2][j + 0,1,2] = subgrid[i][j]
   * return asString(g)
   */

const joinSubGrids2 = (subGrids, n) => subGrids;
const joinSubGrids3 = (subGrids, n) => subGrids;

const step = () => {
  document.getElementById('count').innerHTML++;

  const n = len(grid);
  let subGrids, nextN;
  if (n % 2 === 0) {
    nextN = n * 3/2;
    subGrids = splitSubGrids2(grid, n);
  } else {
    nextN = n * 4/3;
    subGrids = splitSubGrids3(grid, n);
  }

  const nextSubGrids = subGrids;
//  const nextSubGrids = subGrids.map(sg => transforms[sg][to]);

  if (n % 2 === 0) {
    grid = joinSubGrids3(nextSubGrids, nextN);
  } else {
    grid = joinSubGrids2(nextSubGrids, nextN);
  }
};
