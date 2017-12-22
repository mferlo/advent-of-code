'use strict';

const transforms = makeTransforms(lines);

const initialGrid = '.#./..#/###';
let grid = initialGrid;

const display = () =>
  document.getElementById('grid').innerHTML =
    grid.replace(new RegExp('/', 'g'), '<br />');

const init = () => display();

const asString = (g) => g.map(r => r.join('')).join('/');

const to2dArray = (grid, n) => {
  const g = new Array(n);
  const rows = grid.split('/');
  for (let i = 0; i < n; i++) {
    g[i] = rows[i].split('');
  }
  return g;
};

const gridStringN = (g, n, iIn, jIn) => {
  console.log('-> gridStringN', g, n, iIn, jIn);
  let result = "";
  for (let i = iIn; i < iIn + n; i++) {
    for (let j = jIn; j < jIn + n; j++) {
      console.log('-> gridStringN', i, j, g[i]);
      result += g[i].join('');
    }
    result += '/';
  }
  return result.slice(0, -1);
}

const gridString2 = (g, i, j) => gridStringN(g, 2, i, j);
const gridString3 = (g, i, j) => gridStringN(g, 3, i, j);

const splitSubGrids = (grid, n, subGridSize, subGridToString) => {
  const g = to2dArray(grid, n);
  const results = [];
  for (let i = 0; i < n; i += subGridSize) {
    for (let j = 0; j < n; j += subGridSize) {
      results.push(subGridToString(g, i, j));
    }
  }
  return results;
};


const joinSubGrids4 = (subGrids, oldN, n) => {
  console.log('joinSubGrids4', subGrids[0]);
  if (subGrids.length === 1) {
    return subGrids[0];
  } else {
    throw "FIXME joinSubGrids4 n > 1";
  }
};
/*
  const g = new Array(n);
  for (let i = 0; i < n; i++) {
    g[i] = new Array(n);
  }

  let r = 0;
  let c = 0;
  for (let subGrid of subGrids) {
    const lines = subGrid.split('/');

    c += oldN;
    if (c >= oldN) {
      c = 0;
      r += oldN;
    }
  }
*/

/* joinSubGrids3:
   * create 2d array `g`
   * for (i = 0; i < n; i += 3)
   *   for (j = 0; j < n; j += 3)
   *     g[i + 0,1,2][j + 0,1,2] = subgrid[i][j]
   * return asString(g)
   */
const joinSubGridRow = (subGrids, row) => {
  subGrids.map(sg => sg[row].replace('/', ''));
};

const joinSubGrids3 = (subGrids, n) => {
  const subGridsPerRow = n / 3;
  console.log('joinSubGrids3', n, subGrids, subGridsPerRow);
  if (subGridsPerRow === 2) {
    let row = subGrids.slice(0, 2).map(sg => to2dArray(sg, 3));
    console.log('slice(0,2)', row);
    row = subGrids.slice(2, 2).map(sg => to2dArray(sg, 3));
    console.log('slice(2,2)', row);
  } else {
    throw "FIXME";
  }

  let result = "";
  return result;
//  for (let i = 0; i < subGridsPerRow; i++) {
//    const subGridRow = subGrids.slice(3*i, 3*(i+1));
//
//    const r0 = subGrids.map(sg => sg[0].replace(new RegExp('/', 'g'), ''))
//  }
};

const step = () => {
  console.log(grid);
  const n = len(grid);

  let subGridSize, subGridToString, nextN;
  if (n % 2 === 0) {
    subGridSize = 2;
    subGridToString = gridString2;
    nextN = n * 3/2;
  } else {
    subGridSize = 3;
    subGridToString = gridString3;
    nextN = n * 4/3;
  }
  const subGrids = splitSubGrids(grid, n, subGridSize, subGridToString);
  const nextSubGrids = subGrids.map(sg => transforms[subGridSize][sg]);

  if (n % 2 === 0) {
    grid = joinSubGrids3(nextSubGrids, nextN);
  } else {
    grid = joinSubGrids4(nextSubGrids, n, nextN);
  }
  console.log(grid);


  document.getElementById('count').innerHTML++;
  display();
};
