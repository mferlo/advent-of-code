const getInput = (test) => {
  const fs = require('fs');
  const lines = fs.readFileSync(test ? 'test' : 'input', 'utf8').split("\n");
  const parse = /position=<\s*([-\d]+),\s*([-\d]+)> velocity=<\s*([-\d]+),\s*([-\d]+)>/;
  return lines
    .map(line => parse.exec(line).slice(1).map(m => +m))
    .map(([ x, y, xDelta, yDelta ]) => ({ x, y, xDelta, yDelta }));
};

const getPositionAt = (p, t) => [ p.x + t*p.xDelta, p.y + t*p.yDelta ];

const getDimensions = (points) => {
  const xMax = Math.max(...points.map(p => p[0]));
  const xMin = Math.min(...points.map(p => p[0]));
  const yMax = Math.max(...points.map(p => p[1]));
  const yMin = Math.min(...points.map(p => p[1]));
  return { xMin, xMax, yMin, yMax, size: (xMax - xMin) * (yMax - yMin) };
};

const print = (points) => {
  const d = getDimensions(points);
  for (let y = d.yMin; y <= d.yMax; y++) {
    const line = [];
    for (let x = d.xMin; x <= d.xMax; x++) {
      line.push('.');
    }
    for (const p of points) {
      if (p[1] === y) {
        line[p[0]-d.xMin] = '#';
      }
    }
    console.log(line.join(''));
  }
};

const points = getInput();
let lastSize = Number.MAX_VALUE;
let t = 0;

while (true) {
  const curSize = getDimensions(points.map(p => getPositionAt(p, t))).size;
  if (curSize > lastSize) {
    t--;
    break;
  }
  lastSize = curSize;
  t++;
}

print(points.map(p => getPositionAt(p, t)));
console.log(t);

