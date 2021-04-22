'use strict';

const input = // "1234";
const digits = input.split('').map(x => Number.parseInt(x, 10));

let sum = 0;

// Part 1
if (digits[0] === digits[digits.length - 1]) {
  sum += digits[0];
}

for (let i = 0; i < digits.length - 1; i++) {
  if (digits[i] === digits[i+1]) {
    sum += digits[i];
  }
}

console.log(sum);

// Part 2
sum = 0;
const half = digits.length / 2;
for (let i = 0; i < half; i++) {
  if (digits[i] === digits[i + half]) {
    sum += 2*digits[i];
  }
}

console.log(sum);
