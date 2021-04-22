'use strict';

const ifA = (r, a, action) => r === 'a' ? action(a) : a;
const ifB = (r, b, action) => r === 'b' ? action(b) : b;
const halve = x => Math.floor(x / 2);
const triple = x => x * 3;
const incr = x => x + 1;
const atoi = s => Number.parseInt(s);

const hlf = ({p, a, b}, r) => ({ p: p + 1, a: ifA(r, a, halve), b: ifB(r, b, halve) });
const tpl = ({p, a, b}, r) => ({ p: p + 1, a: ifA(r, a, triple), b: ifB(r, b, triple) });
const inc = ({p, a, b}, r) => ({ p: p + 1, a: ifA(r, a, incr), b: ifB(r, b, incr) });

const jmp = ({p, a, b}, v) => ({ p: p + atoi(v), a, b});
const jie = ({p, ...state}, r, v) => ({ p: state[r] % 2 === 0 ? p + atoi(v) : p + 1, ...state});
const jio = ({p, ...state}, r, v) => ({ p: state[r] === 1 ? p + atoi(v) : p + 1, ...state});

const instructions = { hlf, tpl, inc, jmp, jie, jio };

const execute = (program, state) => {
  const op = program[state.p];
  const x = op.split(/,? /);
  return instructions[x[0]](state, x[1], x[2]);
}

const initialState1 = { p: 0, a: 0, b: 0 };
const initialState2 = { p: 0, a: 1, b: 0 };
const program = [ 'jio a, +22', 'inc a', 'tpl a', 'tpl a', 'tpl a', 'inc a', 'tpl a', 'inc a', 'tpl a', 'inc a', 'inc a', 'tpl a', 'inc a', 'inc a', 'tpl a', 'inc a', 'inc a', 'tpl a', 'inc a', 'inc a', 'tpl a', 'jmp +19', 'tpl a', 'tpl a', 'tpl a', 'tpl a', 'inc a', 'inc a', 'tpl a', 'inc a', 'tpl a', 'inc a', 'inc a', 'tpl a', 'inc a', 'inc a', 'tpl a', 'inc a', 'tpl a', 'tpl a', 'jio a, +8', 'inc b', 'jie a, +4', 'tpl a', 'inc a', 'jmp +2', 'hlf a', 'jmp -7' ];

let state = initialState2;
while (state.p >= 0 && state.p < program.length) {
  state = execute(program, state);
}
console.log(JSON.stringify(state));
