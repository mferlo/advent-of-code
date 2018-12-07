const getGraph = (test) => {
  const fs = require('fs');
  const lines = fs.readFileSync(test ? 'test' : 'input', 'utf8').split("\n");

  const allNodes = new Set();
  const sinks = {};
  const parse = /Step (.) must be finished before step (.) can begin\./;

  const parseLine = line => {
    const [_, source, sink] = parse.exec(line);
    allNodes.add(source);
    allNodes.add(sink);
    if (sinks[sink]) {
      sinks[sink].push(source);
    } else {
      sinks[sink] = [ source ];
    }
  };


  lines.forEach(parseLine);
  return { sinks, allNodes: [...allNodes] };
};

const part1 = (test) => {
  const { sinks, allNodes } = getGraph(test);
  const readyNodes = allNodes.filter(node => !sinks[node]);
  const doneNodes = new Set();

  let result = "";
  while (readyNodes.length) {
    readyNodes.sort();
    const cur = readyNodes.shift();
    result += cur;

    doneNodes.add(cur);
    for (const [n, srcs] of Object.entries(sinks)) {
      if (!doneNodes.has(n) &&
          !readyNodes.includes(n) &&
          srcs.every(src => doneNodes.has(src))) {
        readyNodes.push(n);
      }
    }
  }
  console.log(result);
};


const part2 = (test) => {
  const duration = ch => (test ? 1 : 61) + "ABCDEFGHIJKLMNOPQRSTUVWXYZ".indexOf(ch);

  const { sinks, allNodes } = getGraph(test);
  const readyNodes = allNodes.filter(node => !sinks[node]);
  const doneNodes = new Set();

  const workers = test
        ? [ { task: "", finishTime: 0 }, { task: "", finishTime: 0 } ]
        : [ { task: "", finishTime: 0 }, { task: "", finishTime: 0 },
            { task: "", finishTime: 0 }, { task: "", finishTime: 0 },
            { task: "", finishTime: 0 } ];

  let result = "";
  let t = 0;
  while (true) {
    for (const w of workers) {
      if (w.finishTime === t) {
        result += w.task;
        if (result.length === allNodes.length) {
          console.log(t);
          return;
        }
        doneNodes.add(w.task);
        w.available = 1;
        for (const [n, srcs] of Object.entries(sinks)) {
          if (!doneNodes.has(n) &&
              !readyNodes.includes(n) &&
              !workers.some(w => w.task === n) &&
              srcs.every(src => doneNodes.has(src))) {
            readyNodes.push(n);
          }
        }
      }
    }

    readyNodes.sort();

    for (const w of workers) {
      if (w.finishTime <= t && readyNodes.length) {
        w.task = readyNodes.shift();
        w.finishTime = t + duration(w.task)
      }
    }

    t += 1;
  }
};

part1(true);
part2(true);
part1();
part2();
