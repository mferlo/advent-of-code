'use strict';

const history = new Set();
const input = [ 0, 5, 10, 0, 11, 14, 13, 4, 11, 8, 8, 7, 1, 4, 12, 11 ];

const alreadySeen = (state) => {
  const s = JSON.stringify(state);
  const result = history.has(s);
  history.add(s);
  return result;
}

const redistribute = (state) => {
  const len = state.length;
  const v = Math.max(...state);
  const maxBucket = state.findIndex(x => x === v);
  state[maxBucket] = 0;

  const addAllBy = Math.floor(v / len);
  for (let i = 0; i < len; i++) {
    state[i] += addAllBy;
  }

  const leftOver = v % len;
  for (let i = 1; i <= leftOver; i++) {
    state[(maxBucket + i) % len] += 1;
  }
  return state;
}

let count = 0;
let state = input;

while (!alreadySeen(state)) {
  count += 1;
  state = redistribute(state);
}

console.log(count);

const part2State = JSON.stringify(state);
count = 0;
do {
  count += 1;
  state = redistribute(state);
} while (JSON.stringify(state) !== part2State);

console.log(count);
