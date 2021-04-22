'use strict';

const MissileCost = 53;
const MissileDamage = 4;

const DrainCost = 73;
const DrainDamage = 2;

const ShieldCost = 113;
const ShieldDuration = 6;
const ShieldBonusArmor = 7;

const PoisonCost = 173;
const PoisonDamage = 3;
const PoisonDuration = 6;

const RechargeCost = 229;
const RechargeValue = 101;
const RechargeDuration = 5;

const BossDamage = 9;
const ShieldedBossDamage = BossDamage - ShieldBonusArmor;

const initialState = {
  HP: 50,
  Mana: 500,
  ManaSpent: 0,
  BossHP: 51,
  PoisonDuration: 0,
  ShieldDuration: 0,
  RechargeDuration: 0
};

const castSpell = (state, cost, durationProperty, durationLength) => {
  if (state.Mana < cost || state[durationProperty] > 0) {
    return null;
  } else {
    const result = { ...state, Mana: state.Mana - cost, ManaSpent: state.ManaSpent + cost };
    if (durationProperty) {
      result[durationProperty] = durationLength;
    }
    return result;
  }
};

const castMissile = state => {
  const result = castSpell(state, MissileCost);
  return result && ({ ...result, BossHP: result.BossHP - MissileDamage });
};

const castDrain = state => {
  const result = castSpell(state, DrainCost);
  return result && ({ ...result, HP: result.HP + DrainDamage, BossHP: result.BossHP - DrainDamage });
};

const castShield = state => castSpell(state, ShieldCost, "ShieldDuration", ShieldDuration);
const castPoison = state => castSpell(state, PoisonCost, "PoisonDuration", PoisonDuration);
const castRecharge = state => castSpell(state, RechargeCost, "RechargeDuration", RechargeDuration);

const castEverySpellYouCan = state => [
  castMissile(state),
  castDrain(state),
  castShield(state),
  castPoison(state),
  castRecharge(state)
].filter(newState => newState !== null);

const tick = state => {
  const result = Object.assign({}, state);
  if (result.PoisonDuration) {
    result.PoisonDuration -= 1;
    result.BossHP -= PoisonDamage;
  }
  if (result.ShieldDuration) {
    result.ShieldDuration -= 1;
  }
  if (result.RechargeDuration) {
    result.RechargeDuration -= 1;
    result.Mana += RechargeValue;
  }
  return result;
};

const bossAttack = state => state.BossHP > 0
  ? ({ ...state, HP: state.HP - (state.ShieldDuration ? ShieldedBossDamage : BossDamage) })
  : state;

const playersTurn = state => castEverySpellYouCan(tick(state));
const bossTurn = state => bossAttack(tick(state));

const flatten = states => [].concat.apply([], states);

// Part 1 vs 2
const hardMode = true;

const run = () => {
  let states = [ initialState ];
  let leastMana = Number.MAX_VALUE;
  let turn = "player";

  const checkForWin = endOfTurnStates => {
    endOfTurnStates.forEach(s => { if (s.ManaSpent < leastMana) { leastMana = s.ManaSpent; } });
  };

  while (states.length > 0) {
    if (turn === "player") {
      if (hardMode) {
        states = states.map(s => ({ ...s, HP: s.HP - 1}))
          .filter(s => s.HP > 0);
      }

      states = flatten(states.map(s => playersTurn(s)))
        .filter(s => s.ManaSpent < leastMana);

      turn = "boss";
    } else {
      states = states.map(s => bossTurn(s)).filter(s => s.HP > 0);
      turn = "player";
    }

    checkForWin(states.filter(s => s.BossHP <= 0));
    states = states.filter(s => s.BossHP > 0);
  }
  console.log(leastMana);
};

run();
