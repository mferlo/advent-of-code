'use strict';

const getParticles = () => {
  const fs = require('fs');
  const lines = fs.readFileSync('input', 'utf8').split('\n');
  lines.pop();

  const sToI = (s) => Number.parseInt(s, 10);
  const triple = '<(-?\\d+),(-?\\d+),(-?\\d+)>';
  const re = new RegExp(`p=${triple}, v=${triple}, a=${triple}`);

  const parseLine = (line) => {
    const m = re.exec(line).slice(1).map(sToI);
    return { p: m.slice(0, 3), v: m.slice(3, 6), a: m.slice(6) };
  }
  return lines.map(parseLine);
};

const step = (p) => {
  const vNew = [ p.v[0] + p.a[0], p.v[1] + p.a[1], p.v[2] + p.a[2] ];
  const pNew = [ p.p[0] + vNew[0], p.p[1] + vNew[1], p.p[2] + vNew[2] ];
  return { p: pNew, v: vNew, a: p.a, dead: p.dead };
}

const distance = (particle) => particle.p.map(Math.abs).reduce((x, y) => x+y);

const closest = (particles) => {
  let c = { i: -1, d: Number.MAX_VALUE };
  for (let i = 0; i < particles.length; i++) {
    const d = distance(particles[i]);
    if (d < c.d) {
      c = { i, d };
    }
  }
  return c;
}

const initialParticles = getParticles();
let particles = initialParticles;

// Let's say "long term" is 1000 iterations
for (let i = 0; i < 1000; i++) {
  particles = particles.map(step);
}
console.log('Part 1: ' + closest(particles).i);


// Part 2:
particles = initialParticles.map(p => ({ dead: false, ...p }));

const killCollided = (particles) => {
  let positions = new Map();
  for (let i = 0; i < particles.length; i++) {
    const key = JSON.stringify(particles[i].p);
    if (positions.has(key)) {
      particles[positions.get(key)].dead = true;
      particles[i].dead = true;
    } else {
      positions.set(key, i);
    }
  }
}

for (let i = 0; i < 1000; i++) {
  particles = particles.map(step);
  killCollided(particles);
}
console.log('Part 2: ' + particles.filter(p => !p.dead).length);
