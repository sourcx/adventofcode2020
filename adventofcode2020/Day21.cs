using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace adventofcode
{
    class Day21
    {
        public static void Start()
        {
            var foods = new Dictionary<List<string>, List<string>>();
            var allergens = new HashSet<string>();
            var ingredients = new HashSet<string>();

            foreach (var line in File.ReadLines("input/21"))
            {
                var foodIngredients = line.Split(" (contains ")[0].Split(" ");
                ingredients.UnionWith(foodIngredients);
                var foodAllergens = line.Split(" (contains ")[1][..^1].Split(", ");
                allergens.UnionWith(foodAllergens);
                foods[foodAllergens.ToList()] = foodIngredients.ToList();
            }

            var allergensWithIngredients = Correspond(allergens, foods);
            var unmatchedIngredients = UnmatchedIngredients(ingredients, allergensWithIngredients);

            var nrAppearances = unmatchedIngredients.Select(
                (ingredient) => foods.Values.Where(
                    (ingredients) => ingredients.Contains(ingredient)
                ).Count()).Aggregate(0, (i, j) => i + j);

            // Part1: How many times do any of those ingredients appear?
            Console.WriteLine($"Unused appearance count: {nrAppearances}"); // 2287

            var sortedAllergens = allergensWithIngredients.Keys.ToList().OrderBy(a => a);
            var danger = sortedAllergens.Select(allergen => allergensWithIngredients[allergen].First()).Aggregate("", (i, j) => i + "," + j)[1..];

            // Part 2: What is your canonical dangerous ingredient list?
            Console.WriteLine($"Canonical dangerous ingredient list: {danger}"); // fntg,gtqfrp,xlvrggj,rlsr,xpbxbv,jtjtrd,fvjkp,zhszc
        }

        static Dictionary<string, HashSet<string>> Correspond(HashSet<string> allergens, Dictionary<List<string>, List<string>> foods)
        {
            var allergensWithIngredients = new Dictionary<string, HashSet<string>>();

            // Intersect all sets of ingredients that can contain this allergen.
            foreach (var allergen in allergens)
            {
                foreach (var food in foods)
                {
                    if (food.Key.Contains(allergen))
                    {
                        HashSet<string> ingredients;

                        if (allergensWithIngredients.TryGetValue(allergen, out ingredients))
                        {
                            ingredients.IntersectWith(food.Value);
                        }
                        else
                        {
                            allergensWithIngredients[allergen] = new HashSet<string>(food.Value);
                        }
                    }
                }
            }

            // Remove ingredients that are matched 1 on 1 to an allergen from the others.
            // This can take multiple iterations.
            while (allergensWithIngredients.Where(kv => kv.Value.Count() != 1).Any())
            {
                foreach (var one in allergensWithIngredients.Where(kv => kv.Value.Count() == 1))
                {
                    foreach (var other in allergensWithIngredients.Where(other => one.Key != other.Key))
                    {
                        other.Value.Remove(one.Value.First());
                    }
                }
            }

            return allergensWithIngredients;
        }

        static HashSet<string> UnmatchedIngredients(HashSet<string> ingredients, Dictionary<string, HashSet<string>> allergensWithIngredients)
        {
            var unmatched = new HashSet<string>();

            foreach (var ingredient in ingredients)
            {
                if (!allergensWithIngredients.Values.Where((ingredients) => ingredients.Contains(ingredient)).Any())
                {
                    unmatched.Add(ingredient);
                }
            }

            return unmatched;
        }
    }
}
