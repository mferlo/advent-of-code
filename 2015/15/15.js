'use strict';

const properties = [ 'capacity', 'durability', 'flavor', 'texture' ];
const ingredients = {
  // Butterscotch: { capacity: -1, durability: -2, flavor: 6, texture: 3, calories: 8 },
  // Cinnamon: { capacity: 2, durability: 3, flavor: -2, texture: -1, calories: 3 },
  Sprinkles: { capacity: 5, durability: -1, flavor: 0, texture: 0, calories: 5 },
  PeanutButter: { capacity: -1, durability: 3, flavor: 0, texture: 0, calories: 1 },
  Frosting: { capacity: 0, durability: -1, flavor: 4, texture: 0, calories: 6 },
  Sugar: { capacity: -1, durability: 0, flavor: 0, texture: 2, calories: 8 }
};

const scoreProperty = (quantities, property) => {
  const result = Object.entries(quantities)
    .map(([ingredient, quantity]) => quantity * ingredients[ingredient][property])
    .reduce((s, x) => s + x, 0);

  return result > 0 ? result : 0;
};

const score = (quantities) =>
  properties.map(p => scoreProperty(quantities, p)).reduce((p, x) => p * x, 1);

const chooseIngredients = (list, target) => {
  const first = list[0];
  const rest = list.slice(1);
  if (rest.length === 0) {
    return [ { [first]: target } ];
  } else {
    let results = [];
    for (let i = 0; i <= target; i++) {
      const result = chooseIngredients(rest, target - i);
      result.forEach(r => r[first] = i);
      results = results.concat(result);
    }
    return results;
  }
};

const findBestScore = (choices) => {
  let bestScore = 0;
  let bestChoice = null;

  for (const choice of choices) {
    const s = score(choice);
    if (s > bestScore) {
       bestScore = s;
      bestChoice = choice;
    }
  }
  return { bestScore, bestChoice };
}

const choices = chooseIngredients(Object.keys(ingredients), 100);
console.log(findBestScore(choices));

const part2Choices = choices.filter(c => scoreProperty(c, 'calories') === 500);
console.log(findBestScore(part2Choices));
