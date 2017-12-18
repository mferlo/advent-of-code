'use strict';

const instructions = [ "set i 31", "set a 1", "mul p 17", "jgz p p", "mul a 2", "add i -1", "jgz i -2", "add a -1", "set i 127", "set p 622", "mul p 8505", "mod p a", "mul p 129749", "add p 12345", "mod p a", "set b p", "mod b 10000", "snd b", "add i -1", "jgz i -9", "jgz a 3", "rcv b", "jgz b -1", "set f 0", "set i 126", "rcv a", "rcv b", "set p a", "mul p -1", "add p b", "jgz p 4", "snd a", "set a b", "jgz 1 3", "snd b", "set f 1", "add i -1", "jgz i -11", "snd a", "jgz f -16", "jgz a -19" ];

const reg = { a: 0, b: 0, f: 0, i: 0, p: 0 };

const valueOf = (x) => {
  const n = Number.parseInt(x);
  return Number.isNaN(n) ? reg[x] : n;
}

const part1 = () => {
  let i = 0;
  let lastSound = undefined;

  while (true) {
    const [instr, r, arg] = instructions[i++].split(' ');

    switch (instr) {
    case 'snd': lastSound = valueOf(r); break;
    case 'set': reg[r] = valueOf(arg);  break;
    case 'add': reg[r] += valueOf(arg); break;
    case 'mul': reg[r] *= valueOf(arg); break;
    case 'mod': reg[r] %= valueOf(arg); break;

    case 'rcv': if (reg[r]) { return lastSound; } break;
    case 'jgz': if (valueOf(r) > 0) { i += valueOf(arg); i--; }
    }
  }
}

console.log(part1());
