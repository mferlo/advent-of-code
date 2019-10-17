const parser = /(?<units>\d+) units each with (?<hp>\d+) hit points (?<specials>\(.*\)) with an attack that does (?<attack>\d+) (?<attackType>.*) damage at initiative (?<initiative>\d+)/;

let id = 0;

const makeGroup = input => {
  const group = {
    ...input.match(parser).groups,
    id: id++,
    immunities: [],
    weaknesses: []
  };

  const specials = group.specials;
  delete group.specials;
  if (specials) {
    if (specials.includes('immune')) {
      group.immunities = specials.match(/immune to ([^;)]*)/).slice(1);
    }
    if (specials.includes('weak')) {
      group.weaknesses = specials.match(/weak to ([^;)]*)/).slice(1);
    }
  }

  return group;
}

const effectivePower = group => group.units * group.attack;


const immuneTestInput = [
  "17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2",
  "989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3"
];

const infectionTestInput = [
  "801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1",
  "4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4"
];

immuneTestInput.forEach(i => console.log(makeGroup(i)));
infectionTestInput.forEach(i => console.log(makeGroup(i)));
