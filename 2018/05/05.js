const input = "";

const iterate = s => {
  let match = undefined;
  for (i = 0; i < s.length - 1; i += 1) {
    if (s.charAt(i) !== s.charAt(i + 1) &&
        s.charAt(i).toLowerCase() === s.charAt(i + 1).toLowerCase()) {
      match = i;
      break;
    }
  }
  if (match !== undefined) {
    return s.slice(0, match) + s.slice(match + 2);
  } else {
    return s;
  }
};

const react = s => {
  let result = s;
  let prev;
  do {
    prev = result;
    result = iterate(result);
  } while (prev != result);
  return result;
};

const part2 = s => {
  let result = s;
  for (let i = 'A'.charCodeAt(0); i <= 'Z'.charCodeAt(0); i += 1) {
    const upper = String.fromCharCode(i);
    const lower = upper.toLowerCase();
    const toTest = s.replace(new RegExp(`[${upper}${lower}]`, 'g'), '');
    const testResult = react(toTest);
    if (testResult.length < result.length) {
      result = testResult;
    }
  }
  return result;
};

console.log(react(input).length);
console.log(part2(input).length);
