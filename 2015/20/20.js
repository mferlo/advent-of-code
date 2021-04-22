'use strict';

const part1 = false;
const input = 33100000;
const divisors = [ null, [1] ];

const getDivisors = n => {
  let result;
  if (n % 2 === 0) {
    const vals = divisors[n / 2];
    result = new Set(vals.concat(vals.map(v => v * 2)));
  } else {
    result = new Set([1, n]);
    const max = Math.sqrt(n);
    for (let i = 3; i < max; i += 2) {
      if (n % i === 0) {
        result.add(i);
        result.add(n / i);
      }
    }
    if (Number.isInteger(max)) {
      result.add(max);
    }
  }
  divisors[n] = Array.from(result);
  return part1 ? divisors[n] : divisors[n].filter(i => (n / i) <= 50);
}

const run = () => {
  let i = 0;
  let s = 0;
  let max = 0;
  while (s < input) {
    i += 1;
    if (part1) {
      s = getDivisors(i).reduce((sum, cur) => sum + 10*cur, 0);
    } else {
      s = getDivisors(i).reduce((sum, cur) => sum + 11*cur, 0);
    }
  }
  console.log(i);
}

run();
