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
      this.immunities = g.specials.match(/immune to ([^;)]*)/).slice(1);
    } else {
      this.immunities = [];
    }

    if (g.specials && g.specials.includes('weak')) {
      this.weaknesses = g.specials.match(/weak to ([^;)]*)/).slice(1);
    } else {
      this.weaknesses = [];
    }
  }

  get effectivePower() {
    return this.units * this.attack;
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

const g = new Group(immuneTestInput[0]);
console.log(g);
