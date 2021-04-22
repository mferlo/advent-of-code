'use strict';

const parser = /(?<units>\d+) units each with (?<hp>\d+) hit points (?<specials>\(.*\))? ?with an attack that does (?<attackDamage>\d+) (?<attackType>.*) damage at initiative (?<initiative>\d+)/;

class Group {
  constructor(input, id) {
    this.id = id;
    const g = input.match(parser).groups;
    this.units = +g.units;
    this.hp = +g.hp;
    this.attackDamage = +g.attackDamage;
    this.initiative = +g.initiative;
    this.attackType = g.attackType;

    if (g.specials && g.specials.includes('immune')) {
      this.immunities = g.specials.match(/immune to ([^;)]*)/)[1].split(', ');
    } else {
      this.immunities = [];
    }

    if (g.specials && g.specials.includes('weak')) {
      this.weaknesses = g.specials.match(/weak to ([^;)]*)/)[1].split(', ');
    } else {
      this.weaknesses = [];
    }
  }

  get effectivePower() {
    return this.units * this.attackDamage;
  }

  calculateDamageTo(defender, silent) {
    let dmg;
    if (defender.immunities.includes(this.attackType)) {
      dmg = 0;
    } else if (defender.weaknesses.includes(this.attackType)) {
      dmg = 2 * this.effectivePower;
    } else {
      dmg = this.effectivePower;
    }
    if (!silent) {
      console.log(`${this.id} would deal ${defender.id} ${dmg} damage`);
    }
    return dmg;
  }

  attack(defender) {
    const damage = this.calculateDamageTo(defender, true);
    const unitsKilled = Math.min(defender.units, Math.floor(damage / defender.hp));
    defender.units -= unitsKilled;
    console.log(`${this.id} attacks ${defender.id}, killing ${unitsKilled}`);
  }
}

const immuneTestInput = [
  "17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2",
  "989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3"
];

const infectionTestInput = [
  "801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1",
  "4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4"
];

const immuneInput = [
  "2086 units each with 11953 hit points with an attack that does 46 cold damage at initiative 13",
  "329 units each with 3402 hit points (weak to bludgeoning) with an attack that does 90 slashing damage at initiative 1",
  "414 units each with 7103 hit points (weak to bludgeoning; immune to radiation) with an attack that does 170 radiation damage at initiative 4",
  "2205 units each with 7118 hit points (immune to cold; weak to fire) with an attack that does 26 radiation damage at initiative 18",
  "234 units each with 9284 hit points (weak to slashing; immune to cold, fire) with an attack that does 287 radiation damage at initiative 12",
  "6460 units each with 10804 hit points (weak to fire) with an attack that does 15 slashing damage at initiative 11",
  "79 units each with 1935 hit points with an attack that does 244 radiation damage at initiative 8",
  "919 units each with 2403 hit points (weak to fire) with an attack that does 22 slashing damage at initiative 2",
  "172 units each with 1439 hit points (weak to slashing; immune to cold, fire) with an attack that does 69 slashing damage at initiative 3",
  "1721 units each with 2792 hit points (weak to radiation, fire) with an attack that does 13 cold damage at initiative 16",
];

const infectionInput = [
  "1721 units each with 29925 hit points (weak to cold, radiation; immune to slashing) with an attack that does 34 radiation damage at initiative 5",
  "6351 units each with 21460 hit points (weak to cold) with an attack that does 6 slashing damage at initiative 15",
  "958 units each with 48155 hit points (weak to bludgeoning) with an attack that does 93 radiation damage at initiative 7",
  "288 units each with 41029 hit points (immune to bludgeoning; weak to radiation) with an attack that does 279 cold damage at initiative 20",
  "3310 units each with 38913 hit points with an attack that does 21 radiation damage at initiative 19",
  "3886 units each with 16567 hit points (immune to bludgeoning, cold) with an attack that does 7 cold damage at initiative 9",
  "39 units each with 7078 hit points with an attack that does 300 bludgeoning damage at initiative 14",
  "241 units each with 40635 hit points (weak to cold) with an attack that does 304 fire damage at initiative 6",
  "7990 units each with 7747 hit points (immune to fire) with an attack that does 1 radiation damage at initiative 10",
  "80 units each with 30196 hit points (weak to fire) with an attack that does 702 bludgeoning damage at initiative 17",
];

const byPowerThenInitiative = (a, b) =>
  (b.effectivePower - a.effectivePower) || (a.initiative - b.initiative);

const chooseTarget = (attacker, defenders) => {
  const damageDealt = defenders.map(
    d => ({ defenderId: d.id, damage: attacker.calculateDamageTo(d) }));

  const maxDamageDealt = Math.max(...damageDealt.map(d => d.damage));
  const targetIds = damageDealt
    .filter(d => d.damage === maxDamageDealt)
    .map(d => d.defenderId);

  if (targetIds.length && maxDamageDealt > 0) {
    const targets = defenders
      .filter(d => targetIds.includes(d.id))
      .sort(byPowerThenInitiative);
    return targets[0].id;
  } else {
    return undefined;
  }
};

const chooseTargets = (attackers, defenders) => {
  const result = {};
  const sortedAttackers = [ ...attackers ].sort(byPowerThenInitiative);
  let remainingDefenders = [ ...defenders ];

  for (const attacker of sortedAttackers) {
    const targetId = chooseTarget(attacker, remainingDefenders);
    if (targetId !== undefined) {
      result[attacker.id] = targetId;
      remainingDefenders = remainingDefenders.filter(d => d.id !== targetId);
    }
  }
  return result;
};

const resolveAttacks = (groups, targets) => {
  groups.sort((a, b) => b.initiative - a.initiative);

  for (const group of groups) {
    if (group.hp <= 0) {
      continue;
    }

    const targetId = targets[group.id];

    if (!targetId) {
      continue;
    }

    const target = groups.find(g => g.id == targetId);
    group.attack(target);
  }
};

let g1 = immuneInput.map((input, i) => new Group(input, `Immune Group ${i+1}`));
let g2 = infectionInput.map((input, i) => new Group(input, `Infection Group ${i+1}`));

while (g1.length && g2.length) {
  console.log('Immune System:');
  g1.forEach(g => console.log(`${g.id} contains ${g.units} units`));
  console.log('Infection:')
  g2.forEach(g => console.log(`${g.id} contains ${g.units} units`));
  console.log();
  
  const g1Targets = chooseTargets(g1, g2);
  const g2Targets = chooseTargets(g2, g1);
  console.log();

  resolveAttacks([ ...g1, ...g2 ], { ...g1Targets, ...g2Targets });
  g1 = g1.filter(g => g.units > 0);
  g2 = g2.filter(g => g.units > 0);

  console.log();
}

console.log(`Winning army has ${[...g1, ...g2].reduce((sum, g) => sum + g.units, 0)} units total`);
