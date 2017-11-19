const testWeights = [ 1,2,3,4,5,7,8,9,10,11 ].sort((a, b) => b - a);
const realWeights = [ 1, 2, 3, 7, 11, 13, 17, 19, 23, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113 ].sort((a, b) => b - a);

const inputWeights = realWeights;

//const balancedWeight = inputWeights.reduce((s, w) => s + w, 0) / 3;
const balancedWeight = inputWeights.reduce((s, w) => s + w, 0) / 4;

// Assumptions:
// 1. weights is sorted large to small
// 2. Each present has a unique weight
// 3. When we find the smallest quantity of weights that make it to balancedWeight,
//    the unused weights can balance between themselves.
const greedyFindAll = (weights, targetWeight) => {
  if (weights.length === 0 || targetWeight === 0) {
    return []; // Either we already found it, or we never will. Either way, we're done.
  }

  const first = weights[0];
  const rest = weights.slice(1);

  if (first === targetWeight) {
    return [ [ first ] ]; // We can do no better. Any other solution would use 2 weights (per #1 & #2)
  }

  const didntUseIt = greedyFindAll(rest, targetWeight);
  if (first > targetWeight) {
    return didntUseIt;
  } else {
    const usedIt = greedyFindAll(rest, targetWeight - first);
    usedIt.forEach(x => x.push(first));
    return usedIt.concat(didntUseIt);
  }
};

const qe = arr => arr.reduce((p, x) => p * x, 1);
const ordering = (a, b) => a.length - b.length || qe(a) - qe(b);

const balances = greedyFindAll(inputWeights, balancedWeight);
balances.sort(ordering);
console.log(balances[0]);
console.log(qe(balances[0]));
