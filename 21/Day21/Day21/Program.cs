using System;
using System.Collections.Generic;
using System.Linq;


namespace Day21
{
    struct C
    {
        public int HP;
        public int Damage;
        public int Armor;
        public int Budget;

        public C WithItem(Item i) => new C
        {
            HP = this.HP,
            Damage = this.Damage + i.Damage,
            Armor = this.Armor + i.Armor,
            Budget = this.Budget + i.Cost
        };
    }

    struct Item
    {
        public int Cost;
        public int Damage;
        public int Armor;
    }

    static class Program
    {
        static List<Item> Weapons = new List<Item>
        {
            new Item { Cost = 8, Damage = 4 },
            new Item { Cost = 10, Damage = 5 },
            new Item { Cost = 25, Damage = 6 },
            new Item { Cost = 40, Damage = 7 },
            new Item { Cost = 74, Damage = 8 }
        };

        static List<Item> Armors = new List<Item>
        {
            new Item { Cost = 13, Armor = 1 },
            new Item { Cost = 31, Armor = 2 },
            new Item { Cost = 53, Armor = 3 },
            new Item { Cost = 75, Armor = 4 },
            new Item { Cost = 102, Armor = 5 }
        };

        static List<Item> Rings = new List<Item>
        {
            new Item { Cost = 25, Damage = 1 },
            new Item { Cost = 50, Damage = 2 },
            new Item { Cost = 100, Damage = 3 },
            new Item { Cost = 20, Armor = 1 },
            new Item { Cost = 40, Armor = 2 },
            new Item { Cost = 80, Armor = 3 },
        };

        static int CalcDamage(int damage, int armor)
        {
            var result = damage - armor;
            return result >= 1 ? result : 1;
        }

        static bool WinsFight(C pc, C boss)
        {
            var playerHP = pc.HP;
            var bossHP = boss.HP;

            var playerDamage = CalcDamage(pc.Damage, boss.Armor);
            var bossDamage = CalcDamage(boss.Damage, pc.Armor);
            var playersTurn = true;

            while (playerHP > 0 && bossHP > 0)
            {
                if (playersTurn)
                {
                    bossHP -= playerDamage;
                } else
                {
                    playerHP -= bossDamage;
                }

                playersTurn = !playersTurn;
            }

            return playerHP > 0;
        }

        // You must buy exactly one weapon
        static IEnumerable<C> EquipWeapon(this C player)
        {
            foreach (var weapon in Weapons)
            {
                yield return player.WithItem(weapon);
            }
        }

        // Armor is optional, but you can't use more than one
        static IEnumerable<C> EquipArmor(this IEnumerable<C> players)
        {
            foreach (var p in players)
            {
                // Maybe no armor!
                yield return p;
                foreach (var armor in Armors)
                {
                    yield return p.WithItem(armor);
                }
            }
        }

        static IEnumerable<Tuple<Item, Item>> GenerateRingPairs()
        {
            for (var i = 0; i < Rings.Count - 1; i++)
            {
                for (var j = i + 1; j < Rings.Count; j++)
                {
                    yield return Tuple.Create(Rings[i], Rings[j]);
                }
            }
        }

        // You can buy 0-2 rings
        static IEnumerable<C> EquipRings(this IEnumerable<C> players)
        {
            var allPairsOfRings = GenerateRingPairs().ToList();

            foreach (var p in players)
            {
                // No ring
                yield return p;

                // One ring
                foreach (var ring in Rings)
                {
                    yield return p.WithItem(ring);
                }

                // Two rings
                foreach (var ringPair in allPairsOfRings)
                {
                    yield return p.WithItem(ringPair.Item1).WithItem(ringPair.Item2);
                }
            }
        }

        static C SamplePlayer = new C { HP = 8, Damage = 5, Armor = 5 };
        static C SampleBoss = new C { HP = 12, Damage = 7, Armor = 2 };

        static C RealPlayer = new C { HP = 100 };
        static C RealBoss = new C { HP = 100, Damage = 8, Armor = 2 };

        static void Main(string[] args)
        {
            Func<C, bool> winsBossFight = p => WinsFight(p, RealBoss);
            Func<C, bool> losesBossFight = p => !WinsFight(p, RealBoss);

            Console.WriteLine("Part 1: " + RealPlayer.EquipWeapon().EquipArmor().EquipRings().Where(winsBossFight).Min(p => p.Budget));
            Console.WriteLine("Part 2: " + RealPlayer.EquipWeapon().EquipArmor().EquipRings().Where(losesBossFight).Max(p => p.Budget)); 

            Console.ReadLine();
        }
    }
}
