using System;
using System.IO;
using System.Collections.Generic;

namespace adventofcode
{
    class Program
    {
        static void Main(string[] args)
        {
            Day1();
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
                        Console.WriteLine($"{one} + {other} sums up to {YEAR}");
                        Console.WriteLine($"Their product is {one * other}");
                        return;
                    }
                }
            }
        }
    }
}
