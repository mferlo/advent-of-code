const input = [
  // Macro:
  // "{ 'id': " C-d M-f C-d C-d ", 'x':" C-f M-f C-f " 'y': " M-f C-d
  // ", 'width': C-f M-b C-s x C-b C-d ", 'height': " C-e " }," C-n C-a
];

const getMax = () => {
  let xMax = 0;
  let yMax = 0;
  for (const plan of input) {
    const x = plan.x + plan.width;
    const y = plan.y + plan.height;
    if (x > xMax) { xMax = x; }
    if (y > yMax) { yMax = y; }
  }
  return { xMax, yMax };
};

const init = ( { xMax, yMax } ) => {
  const result = [];
  for (let x = 0; x < xMax; x++) {
    result[x] = [];
    for (let y = 0; y < yMax; y++) {
      result[x][y] = [];
    }
  }
  return result;
};

const run = (grid) => {
  for (const plan of input) {
    for (let x = plan.x; x < plan.x + plan.width; x++) {
      for (let y = plan.y; y < plan.y + plan.height; y++) {
	grid[x][y].push(plan.id);
      }
    }
  }
};

const grid = init(getMax());
run(grid);

let answer1 = 0;
let answer2 = new Set(input.map(i => i.id));
for (const row of grid) {
  for (const cell of row) {
    if (cell.length >= 2) {
      answer1 += 1;
    }
    if (cell.length > 1) {
      cell.forEach(id => answer2.delete(id));
    }
  }
}

console.log(answer1);
console.log(answer2.values());
