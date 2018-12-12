const serialNumber = 7347;

const hundredsDigit = n => Math.trunc(n / 100) % 10;

const powerLevel = (x, y) => {
  const rackId = x + 10;
  let result = rackId * y;
  result += serialNumber;
  result *= rackId;
  result = hundredsDigit(result);
  return result - 5;
};

const calcSum = (grid, xInit, yInit, size) => {
  let sum = 0;
  for (let x = xInit; x < xInit + size; x++) {
    for (let y = yInit; y < yInit + size; y++) {
      sum += grid[x][y];
    }
  }
  return sum;
};

const findHighestGrid = (grid, size) => {
  let max = Number.MIN_VALUE;
  let result;
  for (let x = 0; x < 301 - size; x++) {
    for (let y = 0; y < 301 - size; y++) {
      const cur = calcSum(grid, x, y, size);
      if (cur > max) {
        max = cur;
        result = `${x},${y}`;
      }
    }
  }
  return { max, result };
};

const findHighestGrid2 = grid => {
  let max = Number.MIN_VALUE;
  let result;
  for (let size = 1; size <= 300; size++) {
    const resultForSize = findHighestGrid(grid, size);
    if (resultForSize.max > max) {
      max = resultForSize.max;
      result = `${resultForSize.result},${size}`;
    }
  }
  return result;
}

const result = [];
for (let x = 0; x < 300; x++) {
  result[x] = [];
  for (let y = 0; y < 300; y++) {
    result[x][y] = powerLevel(x, y);
  }
}

console.log(findHighestGrid(result, 3).result);
console.log(findHighestGrid2(result));
