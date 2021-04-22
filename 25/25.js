'use strict';

class Tape {
  constructor() {
    this.i = 0;
    this.data = [ 0 ];
  }

  write(v) {
    this.data[this.i] = v;
  }

  current() {
    return this.data[this.i];
  }

  left() {
    this.i--;
    if (this.i === -1) {
      this.data.unshift(0);
      this.i = 0;
    }
  }

  right() {
    this.i++;
    if (this.i === this.data.length) {
      this.data.push(0);
    }
  }

  checksum() {
    return this.data.filter(v => v === 1).length;
  }
}

class TuringMachine {
  static parse(lines) {
    const shiftMatch = (re) => lines.shift().match(re)[1];

    const startState = shiftMatch('Begin in state ([A-Z])');
    const iterations = shiftMatch('(\\d+) steps');
    lines.shift(); // Blank line

    const instructions = {};
    while (lines.length) {
      const state = shiftMatch('In state ([A-Z])');

      lines.shift(); // "If the current value is 0"
      const zero = {
        write: Number.parseInt(shiftMatch('Write the value ([01])'), 10),
        move: shiftMatch('Move one slot to the (\\w+)'),
        next: shiftMatch('Continue with state ([A-Z])'),
      }

      lines.shift(); // "If the current value is 1"
      const one = {
        write: Number.parseInt(shiftMatch('Write the value ([01])'), 10),
        move: shiftMatch('Move one slot to the (\\w+)'),
        next: shiftMatch('Continue with state ([A-Z])'),
      }
      instructions[state] = [ zero, one ];

      lines.shift(); // Blank line
    }

    return new TuringMachine(instructions, startState, iterations);
  }

  constructor(instructions, startState, iterations) {
    this.instructions = instructions;
    this.state = startState;
    this.iterations = iterations;
    this.tape = new Tape();
  }

  run() {
    for (let i = 0; i < this.iterations; i++) {
      this.step();
    }
  }

  step() {
    const i = this.instructions[this.state][this.tape.current()];
    this.tape.write(i.write);
    if (i.move === 'left') {
      this.tape.left();
    } else {
      this.tape.right();
    }
    this.state = i.next;
  }
}

const input = `Begin in state A.
Perform a diagnostic checksum after 6 steps.

In state A:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state B.
  If the current value is 1:
    - Write the value 0.
    - Move one slot to the left.
    - Continue with state B.

In state B:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state A.
  If the current value is 1:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state A.`;

const lines = input.split('\n');
const tm = TuringMachine.parse(lines);
tm.run();
console.log('Checksum: ' + tm.tape.checksum());
