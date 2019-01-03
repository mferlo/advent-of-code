const open = '.';
const tree = '|';
const yard = '#';
const testInput = `.#.#...|#.
.....#|##|
.|..|...#.
..|#.....#
#.#|||#|#|
...#.||...
.|....|...
||...#|.#|
|.||||..|.
...#.|..|.`;

const input = `...`;

const grid = input.split('\n').map(i => i.split(''));
const p = grid => grid.forEach(line => console.log(line.join('')));

/*
  y -->
x 0,0 0,1 0,2 0,3
| 1,0 1,1 1,2 1,3
| 2,0 2,1 2,2 2,3
v 3,0 3,1 3,2 3,3
*/

const getAllAdjacent = (grid, x, y) => {
  const xHasUp = x > 0;
  const xHasDown = x < grid.length - 1;
  const yHasLeft = y > 0;
  const yHasRight = y < grid[0].length - 1;

  const results = [];
  if (xHasUp) {
    if (yHasLeft) {
      results.push(grid[x-1][y-1]);
    }
    results.push(grid[x-1][y]);
    if (yHasRight) {
      results.push(grid[x-1][y+1]);
    }
  }
  if (xHasDown) {
    if (yHasLeft) {
      results.push(grid[x+1][y-1]);
    }
    results.push(grid[x+1][y]);
    if (yHasRight) {
      results.push(grid[x+1][y+1]);
    }
  }
  if (yHasLeft) {
    results.push(grid[x][y-1]);
  }
  if (yHasRight) {
    results.push(grid[x][y+1]);
  }
  return results;
};

const hasThreeOrMoreAdjacentTrees = (grid, x, y) =>
  getAllAdjacent(grid, x, y).filter(n => n === tree).length >= 3;

const hasThreeOrMoreAdjacentYards = (grid, x, y) =>
  getAllAdjacent(grid, x, y).filter(n => n === yard).length >= 3;

const bordersYardAndTree = (grid, x, y) => {
  const ns = getAllAdjacent(grid, x, y);
  return ns.some(n => n === yard) && ns.some(n => n === tree);
};

const iterate = grid => {
  const result = [];
  for (let x = 0; x < grid.length; x += 1) {
    result[x] = [];
    for (let y = 0; y < grid[0].length; y += 1) {
      if (grid[x][y] === open) {
    result[x][y] = hasThreeOrMoreAdjacentTrees(grid, x, y) ? tree : open;
      } else if (grid[x][y] === tree) {
    result[x][y] = hasThreeOrMoreAdjacentYards(grid, x, y) ? yard : tree;
      } else if (grid[x][y] === yard) {
    result[x][y] = bordersYardAndTree(grid, x, y) ? yard : open;
      } else {
    p(grid);
    throw `Unknown item '${grid[x][y]}'`;
      }
    }
  }
  return result;
};

const resourceValue = grid => {
  let treeCount = 0;
  let yardCount = 0;
  for (let x = 0; x < grid.length; x += 1) {
    treeCount += grid[x].filter(n => n === tree).length;
    yardCount += grid[x].filter(n => n === yard).length;
  }
  return treeCount * yardCount;
};

// Part 1
let state = grid;
for (let i = 0; i < 10; i += 1) {
  state = iterate(state);
}
console.log(resourceValue(state));

// Part 2
const str = grid => grid.map(line => line.join('')).join('');

state = grid;
let duplicate;
const cache = {};
for (let i = 0; i < 1000000000; i += 1) {
  const s = str(state);
  if (cache[s]) {
    duplicate = { first: cache[s], next: i };
    console.log(duplicate);
    break;
  }
  cache[s] = i;
  state = iterate(state);
}

// Now we have state as it is at i === duplicate.next, and it will repeat
// every duplicate.next - duplicate.first iterations.
// Let's just brute force this so we don't have to do math.

let period = duplicate.next - duplicate.first;
let foo;
for (foo = duplicate.next; foo < 1000000000; foo += period) { /* empty! */ }
foo -= period;
for (; foo < 1000000000; foo += 1) {
  state = iterate(state);
}
console.log(resourceValue(state));
