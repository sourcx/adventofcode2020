using System;
using System.Collections.Generic;
using System.IO;

namespace adventofcode
{
    class Day01
    {
        static Int16 YEAR = 2020;

        public static void Start()
        {
            Part1();
            Part2();
        }

        static void Part1()
        {
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
                        Console.WriteLine($"Day01: {one} + {other} sums up to {YEAR} and their product is {one * other}."); // 969024
                        return;
                    }
                }
            }
        }

        static void Part2()
        {
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
                            Console.WriteLine($"Day01: {one} + {two} + {three} sums up to {YEAR} and their product is {one * two * three}."); // 230057040
                            return;
                        }
                    }
                }
            }
        }
    }
}
