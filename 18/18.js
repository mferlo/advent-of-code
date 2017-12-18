'use strict';

const instructions = [ "set i 31", "set a 1", "mul p 17", "jgz p p", "mul a 2", "add i -1", "jgz i -2", "add a -1", "set i 127", "set p 622", "mul p 8505", "mod p a", "mul p 129749", "add p 12345", "mod p a", "set b p", "mod b 10000", "snd b", "add i -1", "jgz i -9", "jgz a 3", "rcv b", "jgz b -1", "set f 0", "set i 126", "rcv a", "rcv b", "set p a", "mul p -1", "add p b", "jgz p 4", "snd a", "set a b", "jgz 1 3", "snd b", "set f 1", "add i -1", "jgz i -11", "snd a", "jgz f -16", "jgz a -19" ];

// Part 1 & Part 2 state
let lastSound = undefined;
let lastRecoveredSound = undefined;
let part2Answer = 0;
const queues = [ [], [] ];
const blocked = [ false, false ];

const program = (progNum, isPart1, sndAction) => {
  const reg = { a: 0, b: 0, f: 0, i: 0, p: progNum };
  const receiveQueue = queues[1 - progNum];
  const valueOf = (x) => {
    const n = Number.parseInt(x);
    return Number.isNaN(n) ? reg[x] : n;
  };

  let i = 0;
  return () => {
    blocked[progNum] = false;
    const [instr, r, arg] = instructions[i].split(' ');

    if (instr !== 'jgz') {
      i++;
    }

    switch (instr) {
    case 'set': reg[r] = valueOf(arg);  break;
    case 'add': reg[r] += valueOf(arg); break;
    case 'mul': reg[r] *= valueOf(arg); break;
    case 'mod': reg[r] %= valueOf(arg); break;
    case 'jgz': i += (valueOf(r) > 0) ? valueOf(arg) : 1; break;

    case 'snd': sndAction(valueOf(r)); break;
    case 'rcv':
      if (isPart1) {
        if (valueOf(r)) {
          lastRecoveredSound = lastSound;
        }
      } else {
        if (receiveQueue.length) {
          reg[r] = receiveQueue.shift();
        } else {
          i--;
          blocked[progNum] = true;
        }
      }
    }
  };
};

const part1 = program(0, true, (val) => lastSound = val);
while (lastRecoveredSound === undefined) {
  part1();
}
console.log(lastRecoveredSound);

const p0 = program(0, false, (val) => queues[0].push(val) );
const p1 = program(1, false, (val) => { queues[1].push(val); part2Answer++; });
while (!blocked.every(b => b)) {
  p0();
  p1();
}
console.log(part2Answer);
