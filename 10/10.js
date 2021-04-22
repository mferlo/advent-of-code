const reverse = (list, cur, len) => {
  const result = Array.from(list);
  for (let i = 0; i < len / 2; i++) {
    const s = (cur + i) % list.length;
    const t = (cur + len - i - 1) % list.length;
    [result[s], result[t]] = [list[t], list[s]];
  }
  return result;
}

const init = (len) => new Array(len).fill().map((_, i) => i);

let cur = 0;
let skip = 0;

const algorithm = (list, inputList) => {
  for (var input of inputList) {
    list = reverse(list, cur, input);
    cur = (cur + input + skip) % list.length;
    skip += 1;
  }
  return list;
}

// Part 1
const input = [102,255,99,252,200,24,219,57,103,2,226,254,1,0,69,216];
const result = algorithm(init(256), input);
console.log(result[0] * result[1]);

// Part 2
const toStr = (s) => s.reduce((x, y) => x^y).toString(16).padStart(2, "0");

const makeHash = (list) => {
  let result = "";
  for (let i = 0; i < list.length; i += 16) {
    result += toStr(list.slice(i, i + 16));
  }
  return result;
}

const stringInput = "102,255,99,252,200,24,219,57,103,2,226,254,1,0,69,216";
const asciiInput = stringInput.split('').map(ch => ch.charCodeAt());
const inputSuffix = [ 17, 31, 73, 47, 23 ];
const part2Input = asciiInput.concat(inputSuffix);

cur = 0;
skip = 0;
let list = init(256);
for (let i = 0; i < 64; i++) {
  list = algorithm(list, part2Input);
}

console.log(makeHash(list));
