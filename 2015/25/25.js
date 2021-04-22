const initialCode = 20151125;
const target = [ 2981, 3075 ];

const foundTarget = cur => cur[0] === target[0] && cur[1] === target[1];
const nextPos = ([r, c]) => r === 1 ? [ c + 1, 1 ] : [ r - 1, c + 1 ];
const nextCode = prev => (252533 * prev) % 33554393;

let pos = [1, 1];
let code = initialCode;
while (!foundTarget(pos)) {
  code = nextCode(code);
  pos = nextPos(pos);
}
console.log(code);
