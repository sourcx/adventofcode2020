using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Program
    {
        static void Main(string[] args)
        {
            // Day1();
            // Day1_part2();
            // Day2();
            // Day2_part2();
            // Day3();
            Day3_part2();
        }

        // Find the two entries that sum to 2020; what do you get if you multiply them together?
        static void Day1()
        {
            Int16 YEAR = 2020;

            var entries = new List<Int16>();

            foreach (var line in File.ReadLines("input/1"))
            {
                entries.Add(Int16.Parse(line));
            }

            foreach (var one in entries)
            {
                foreach (var other in entries)
                {
                    if (one + other == YEAR)
                    {
                        Console.WriteLine($"Day1: {one} + {other} sums up to {YEAR} and their product is {one * other}."); // 969024
                        return;
                    }
                }
            }
        }

         static void Day1_part2()
        {
            Int16 YEAR = 2020;

            var entries = new List<Int16>();

            foreach (var line in File.ReadLines("input/1"))
            {
                entries.Add(Int16.Parse(line));
            }

            foreach (var one in entries)
            {
                foreach (var two in entries)
                {
                    foreach (var three in entries)
                    {
                        if (one + two + three == YEAR)
                        {
                            Console.WriteLine($"Day1: {one} + {two} + {three} sums up to {YEAR} and their product is {one * two * three}."); // 230057040.
                            return;
                        }
                    }
                }
            }
        }

        static void Day2()
        {
            Int16 correctPasswords = 0;

            foreach (var line in File.ReadLines("input/2"))
            {
                var groups = Regex.Matches(line, @"(\d+)-(\d+)\s(\w):\s(\w+)")[0].Groups;
                var min = Int16.Parse(groups[1].Value);
                var max = Int16.Parse(groups[2].Value);
                var letter = char.Parse(groups[3].Value);
                var password = groups[4].Value;
                int count = password.Count(f => f == letter);

                if (count >= min && count <= max)
                {
                    correctPasswords += 1;
                }
            }

            Console.WriteLine($"Day2: {correctPasswords} passwords are correct."); // 625
        }

         static void Day2_part2()
        {
            Int16 correctPasswords = 0;

            foreach (var line in File.ReadLines("input/2"))
            {
                var groups = Regex.Matches(line, @"(\d+)-(\d+)\s(\w):\s(\w+)")[0].Groups;
                var pos1 = Int16.Parse(groups[1].Value);
                var pos2 = Int16.Parse(groups[2].Value);
                var letter = char.Parse(groups[3].Value);
                var password = groups[4].Value;

                var letterOccurrances = 0;

                if (password[pos1 - 1] == letter)
                {
                    letterOccurrances += 1;
                }

                if (password[pos2 - 1] == letter)
                {
                    letterOccurrances += 1;
                }

                if (letterOccurrances == 1)
                {
                    correctPasswords += 1;
                }
            }

            Console.WriteLine($"Day2: {correctPasswords} passwords are correct."); // 391
        }

        // How many trees would you encounter?
        static void Day3()
        {
            var mapList = new List<char[]>();

            foreach (var line in File.ReadLines("input/3"))
            {
                mapList.Add(line.ToArray<char>());
            }

            var map = mapList.ToArray();

            Console.WriteLine($"I hit {treesHit(map, 3, 1)} trees."); // 280
        }

        static int treesHit(char[][] map, int x_delta, int y_delta)
        {
            int x = 0;
            int y = 0;
            int treesHit = 0;

            while (y < map.Length)
            {
                if (map[y][x % map[0].Length] == '#')
                {
                    treesHit += 1;
                }

                x += x_delta;
                y += y_delta;
            }

            return treesHit;
        }

        static void Day3_part2()
        {
            var mapList = new List<char[]>();

            foreach (var line in File.ReadLines("input/3"))
            {
                mapList.Add(line.ToArray<char>());
            }

            var map = mapList.ToArray();

            Int64 totalTreesHit = 1;

            totalTreesHit *= treesHit(map, 1, 1);
            totalTreesHit *= treesHit(map, 3, 1);
            totalTreesHit *= treesHit(map, 5, 1);
            totalTreesHit *= treesHit(map, 7, 1);
            totalTreesHit *= treesHit(map, 1, 2);

            Console.WriteLine($"I hit {totalTreesHit} trees in total."); // 4355551200
        }
    }
}
