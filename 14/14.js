const input = 637061;

let recipes, length, e1, e2;

const init = () => {
  recipes = [ 3, 7 ];
  length = 2;
  e1 = 0;
  e2 = 1;
};

const addNewRecipes = (r1, r2) => {
  const sum = r1 + r2;
  if (sum < 10) {
    recipes[length] = sum;
    length += 1;
  } else {
    recipes[length] = Math.floor(sum / 10);
    recipes[length + 1] = sum % 10;
    length += 2;
  }
};

const step = () => {
  addNewRecipes(recipes[e1], recipes[e2]);
  e1 = (e1 + 1 + recipes[e1]) % length;
  e2 = (e2 + 1 + recipes[e2]) % length;
};

const p = () => console.log(e1, e2, recipes);

const testData = [ [9, "5158916779"], [5, "0124515891"], [18, "9251071085"], [2018, "5941429882"] ];
const testPart1 = () => {
  for (const t of testData) {
    console.log(`${t[1]} ${part1(t[0])}`);
  }
};

const part1 = (target) => {
  init();
  while (recipes.length < target + 10) {
    step();
  }
  return recipes.slice(target, target + 10).join('');
};

const part2 = () => {
  const target = input.toString().split('').map(ch => Number.parseInt(ch, 10));
  console.log(target);
  let i = 0;
  while (true) {
    if (i + 5 < length) {
      step();
    }
    if (recipes[i] == target[i] &&
        recipes[i + 1] == target[i + 1] &&
        recipes[i + 2] == target[i + 2] &&
        recipes[i + 3] == target[i + 3] &&
        recipes[i + 4] == target[i + 4] &&
        recipes[i + 5] == target[i + 5]) {
      return i;
    }
    i += 1;
    if (i % 1000 === 0) {
      console.log(length);
    }
  }
};

console.log(part1(input));
console.log(part2());
