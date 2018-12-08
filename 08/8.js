const fs = require('fs');
const input = fs.readFileSync('input', 'utf8').split(' ');

const sum = arr => arr.reduce((x, y) => x + y, 0);

const parse = (entries, results) => {
  const numChildren = +entries.shift();
  const numMetadata = +entries.shift();

  const children = [];
  for (let c = 0; c < numChildren; c++) {
    parse(entries, results);
    children[c] = results[results.length - 1];
  }

  const metadata = [];
  for (let m = 0; m < numMetadata; m++) {
    metadata[m] = +entries.shift();
  }

  // Part 2
  let value;
  if (numChildren === 0) {
    value = sum(metadata);
  } else {
    value = 0;
    for (const index of metadata) {
      const child = children[index - 1]; // One-indexed!
      if (child !== undefined) {
        value += child.value;
      }
    }
  }

  results.push( { children, metadata, value } );
}


const results = [];
parse(input, results);
console.log(sum(results.map(node => sum(node.metadata))));
console.log(results[results.length - 1].value);
