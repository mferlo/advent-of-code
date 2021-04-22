const step = 366;

const buffer = [ 0 ];
let cur = 0;
for (let i = 1; i <= 2017; i++) {
  cur = 1 + (cur + step) % i;
  buffer.splice(cur, 0, i);
}
console.log(`Part 1: ${buffer[(cur + 1) % buffer.length]}`);

let value;
cur = 0;
for (let i = 1; i <= 50000000; i++) {
  cur = 1 + (cur + step) % i;
  if (cur === 1) {
    value = i;
  }
}
console.log(`Part 2: ${value}`);
