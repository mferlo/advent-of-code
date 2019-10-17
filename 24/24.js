const parser = /(?<units>\d+) units each with (?<hp>\d+) hit points (?<specials>\(.*\)) with an attack that does (?<attack>\d+) (?<attackType>.*) damage at initiative (?<initiative>\d+)/;

let id = 0;

class Group {
  constructor(input) {
    this.id = id++;
    const g = input.match(parser).groups;
    this.units = +g.units;
    this.hp = +g.hp;
    this.attack = +g.attack;
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
    return this.units * this.attack;
  }

  calculateDamageTo(defender) {
    if (defender.immunities.includes(this.attackType)) {
      return 0;
    } else if (defender.weaknesses.includes(this.attackType)) {
      return 2 * this.effectivePower;
    } else {
      return this.effectivePower;
    }
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

const byPowerThenInitiative = (a, b) =>
  (b.effectivePower - a.effectivePower) || (a.initiative - b.initiative);

const chooseTarget = (attacker, defenders) => {
  const damageDealt = defenders.map(
    d => ({ defenderId: d.id, damage: attacker.calculateDamageTo(d) }));

  console.log(attacker);
  console.log(damageDealt);

  const maxDamageDealt = Math.max(...damageDealt.map(d => d.damage));
  console.log(maxDamageDealt);
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
    console.log(`${attacker.id} attacking ${targetId}`);

    if (targetId !== undefined) {
      result[attackers.id] = targetId;
      remainingDefenders = remainingDefenders.filter(d => d.id !== targetId);
    }
  }
  return result;
};

const immuneGroups = immuneTestInput.map(i => new Group(i));
const infectionGroups = infectionTestInput.map(i => new Group(i));

chooseTargets(immuneGroups, infectionGroups);
chooseTargets(infectionGroups, immuneGroups);
