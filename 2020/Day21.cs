using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    class Allergen
    {
        public string Name { get; }
        public string IngredientName { get; private set; }
        List<List<string>> PossibleIngredients;

        public Allergen(string name)
        {
            Name = name;
            PossibleIngredients = new List<List<string>>();
        }

        public void AddIngredients(List<string> ingredients) => PossibleIngredients.Add(ingredients);

        public void RemovePossibleIngredient(string wrongIngredient)
        {
            foreach (var ingredients in PossibleIngredients)
            {
                ingredients.Remove(wrongIngredient);
            }
        }

        public bool AttemptIdentification()
        {
            var ingredients = new HashSet<string>(PossibleIngredients.First());
            foreach (var i in PossibleIngredients.Skip(1))
            {
                ingredients.IntersectWith(i);
            }
            Debug.Assert(ingredients.Count > 0);

            if (ingredients.Count == 1)
            {
                IngredientName = ingredients.Single();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class Day21
    {
        static List<(List<string> Ingredients, List<string> Allergens)> Recipes;
        static List<Allergen> IdentifiedAllergens;

        static (List<string> Ingredients, List<string> Allergens) ParseLine(string line)
        {
            var parts = line.Split(" (contains ");
            var ingredients = parts[0].Split(" ").ToList();
            List<string> allergens;
            if (parts.Length == 1)
            {
                allergens = new List<string>();
            }
            else
            {
                allergens = parts[1].TrimEnd(')').Split(", ").ToList();
            }
            return (ingredients, allergens);
        }

        public static void Parse() => Recipes = File.ReadAllLines("21.txt").Select(ParseLine).ToList();

        public static object Part1()
        {
            var allergens = Recipes.SelectMany(i => i.Allergens).Distinct().Select(a => new Allergen(a)).ToDictionary(a => a.Name);
            foreach (var recipe in Recipes)
            {
                foreach (var allergen in recipe.Allergens)
                {
                    allergens[allergen].AddIngredients(recipe.Ingredients);
                }
            }

            IdentifiedAllergens = new List<Allergen>();
            while (IdentifiedAllergens.Count < allergens.Count)
            {
                foreach (var allergen in allergens.Values.Except(IdentifiedAllergens))
                {
                    if (allergen.AttemptIdentification())
                    {
                        IdentifiedAllergens.Add(allergen);
                        foreach (var a in allergens.Values.Except(IdentifiedAllergens))
                        {
                            a.RemovePossibleIngredient(allergen.IngredientName);
                        }
                        break;
                    }
                }
            }

            var allergenIngredients = IdentifiedAllergens.Select(a => a.IngredientName).ToHashSet();
            var allIngredients = Recipes.SelectMany(i => i.Ingredients).ToList();
            return allIngredients.Count(i => !allergenIngredients.Contains(i));
        }

        public static object Part2() => 
            string.Join(",", IdentifiedAllergens.OrderBy(a => a.Name).Select(a => a.IngredientName));
    }
}