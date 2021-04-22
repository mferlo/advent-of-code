const input = "n,n,s";

const delta = { 'n': [ 0, 1 ], 'ne': [ 1, 0 ], 'se': [ 1, -1 ], 's': [ 0, -1 ], 'sw': [ -1, 0 ], 'nw': [ -1, 1 ],  };

const dist = (pos) => [ ...pos, -pos[0]-pos[1] ].map(c => Math.abs(c)).reduce((x, y) => x+y) / 2;

let farthest = 0;

const stepAndCheckMax = (pos, dir) => {
  const newPos = [ pos[0] + delta[dir][0], pos[1] + delta[dir][1] ];
  const d = dist(newPos);
  if (d > farthest) {
    farthest = d;
  }
  return newPos;
}

const endPoint = input.split(',').reduce((pos, dir) => stepAndCheckMax(pos, dir), [0, 0]);

console.log(dist(endPoint));
console.log(farthest);
