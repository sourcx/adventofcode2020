using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day06
    {
        public static void start()
        {
            part1();
            part2();
        }

        // What is the sum of those counts?
        static void part1()
        {
            var answers = new HashSet<char>();
            int sum = 0;

            foreach (var line in File.ReadLines("input/6_example"))
            {
                if (line == String.Empty)
                {
                    // Console.WriteLine($"Last set had {answers.Count} answers.");
                    sum += answers.Count;
                    answers.Clear();
                }

                foreach (var c in line.ToArray())
                {
                    answers.Add(c);
                }
            }

            // Console.WriteLine($"Last set had {answers.Count} answers.");
            sum += answers.Count;

            Console.WriteLine($"Day 06: Questions to which anyone answered yes: {sum}."); // 11
        }

        static void part2()
        {
            HashSet<char> answers = null;
            int sum = 0;

            foreach (var line in File.ReadLines("input/6"))
            {
                if (line == String.Empty)
                {
                    // Console.WriteLine($"Last set had {answers.Count} answers.");
                    sum += answers.Count;
                    answers = null;
                }
                else
                {
                    if (answers == null)
                    {
                        answers = new HashSet<char>();
                        foreach (var c in line.ToArray())
                        {
                            answers.Add(c);
                        }
                    }
                    else
                    {
                        answers.IntersectWith(line.ToArray());
                    }
                }
            }

            // Console.WriteLine($"Last set had {answers.Count} answers.");
            sum += answers.Count;

            Console.WriteLine($"Day 06: Questions to which everyone answered yes {sum}."); // 3243
        }
    }
}
