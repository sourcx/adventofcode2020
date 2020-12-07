using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day07
    {
        class Bag
        {
            public string _name;
            public HashSet<Tuple<int, Bag>> _bags;

            public Bag(string name, HashSet<Tuple<int, Bag>> bags)
            {
                this._name = name;
                this._bags = bags;
            }

            public Bag(string name)
            {
                this._name = name;
            }

            public bool Contains(Bag toFind)
            {
                if (_bags == null)
                {
                    return false;
                }

                foreach (var bag in _bags)
                {
                    if (bag.Item2.Equals(toFind) || bag.Item2.Contains(toFind))
                    {
                        return true;
                    }
                }

                return false;
            }

            public int NrOfContainingBags()
            {
                int total = 1;

                foreach (var bag in _bags)
                {
                    total += bag.Item1 * bag.Item2.NrOfContainingBags();
                }

                return total;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                else
                {
                    Bag other = (Bag) obj;
                    return this._name == other._name;
                }
            }

            public override int GetHashCode()
            {
                return this._name.GetHashCode();
            }
        }

        public static void start()
        {
            var bags = GetBags();
            part1(bags);
            part2(bags);
        }

        // How many bag colors can eventually contain at least one shiny gold bag?
        static void part1(HashSet<Bag> bags)
        {
            var shinyGoldBag = new Bag("shiny gold");
            var nrContainingShinyGold = bags.Where(bag => bag.Contains(shinyGoldBag)).Count();

            Console.WriteLine($"Day 07: {nrContainingShinyGold} can contain shiny gold."); // 177
        }

        // How many individual bags are required inside your single shiny gold bag?
        static void part2(HashSet<Bag> bags)
        {
            Bag shinyGold = null;

            if (bags.TryGetValue(new Bag("shiny gold"), out shinyGold))
            {
                Console.WriteLine($"Day 07: Shiny gold bag contains {shinyGold.NrOfContainingBags() - 1} bags."); // 34988
            }
        }

        private static HashSet<Bag> GetBags()
        {
            var bags = new HashSet<Bag>();

            foreach (var line in File.ReadLines("input/7"))
            {
                var subBags = new HashSet<Tuple<int, Bag>>();

                foreach (var subBagStr in line.Split(" bags contain ")[1].Split(","))
                {
                    if (subBagStr == "no other bags.")
                    {
                        continue;
                    }

                    var match = Regex.Match(subBagStr, @"(\d) ([\w\s]+) bag.*");
                    var amount = Int32.Parse(match.Groups[1].Value);
                    var subBagName = match.Groups[2].Value;
                    Bag subBag = null;

                    if (!bags.TryGetValue(new Bag(subBagName), out subBag))
                    {
                        subBag = new Bag(subBagName);
                        bags.Add(subBag);
                    }

                    subBags.Add(new Tuple<int, Bag>(amount, subBag));
                }

                Bag bag = null;
                var name = line.Split(" bags contain ")[0];

                if (bags.TryGetValue(new Bag(name), out bag))
                {
                    bag._bags = subBags;
                }
                else
                {
                    bag = new Bag(name, subBags);
                    bags.Add(bag);
                }
            }

            return bags;
        }
    }
}
