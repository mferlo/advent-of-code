'use strict';

// Part 1 is basically a copy/paste + tweak from day 18

const instructions = [ "set b 84", "set c b", "jnz a 2", "jnz 1 5", "mul b 100", "sub b -100000", "set c b", "sub c -17000", "set f 1", "set d 2", "set e 2", "set g d", "mul g e", "sub g b", "jnz g 2", "set f 0", "sub e -1", "set g e", "sub g b", "jnz g -8", "sub d -1", "set g d", "sub g b", "jnz g -13", "jnz f 2", "sub h -1", "set g b", "sub g c", "jnz g 2", "jnz 1 3", "sub b -17", "jnz 1 -23" ];

let part1 = 0;
const reg = {};

const valueOf = (x) => {
  const n = Number.parseInt(x, 10);
  return Number.isNaN(n) ? reg[x] : n;
};

const runProgram = () => {
  let i = 0;

  while (i >= 0 && i < instructions.length) {
    const [instr, r, arg] = instructions[i].split(' ');

    if (instr !== 'jnz') {
      i++;
    }

    switch (instr) {
    case 'set': reg[r] = valueOf(arg);  break;
    case 'sub': reg[r] -= valueOf(arg); break;
    case 'mul': reg[r] *= valueOf(arg); part1++; break;
    case 'jnz': i += (valueOf(r) !== 0) ? valueOf(arg) : 1; break;
    }
  }
};

for (const r of "abcdefgh") {
  reg[r] = 0;
}
runProgram();
console.log(part1);

// Eh, why not give it a shot:
for (const r of "abcdefgh") {
  reg[r] = 0;
}
reg['a'] = 1;
runProgram();
console.log(reg['h']);
