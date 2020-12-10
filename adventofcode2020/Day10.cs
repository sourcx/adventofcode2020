using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;

namespace adventofcode
{
    class Day10
    {
        static int[] _Tribonacci = new []{ 1, 1, 2, 4, 7, 13 };

        public static void Start()
        {
            Part1();
            Part2();
        }

        //What is the number of 1-jolt differences multiplied by the number of 3-jolt differences?
        static void Part1()
        {
            var nrs = File.ReadLines("input/10").Select(Int64.Parse).ToList();;
            nrs.Sort();

            var latestAdapter = 0L;

            var diffs1 = 0;
            var diffs3 = 0;

            foreach (var nr in nrs)
            {
                var diff = nr - latestAdapter;

                if (diff == 1)
                {
                    diffs1 += 1;
                }
                else if (diff == 3)
                {
                    diffs3 += 1;
                }
                else
                {
                    Console.WriteLine($"Cannot connect adapter with {nr} jolts.");
                    return;
                }

                latestAdapter = nr;
            }

            Console.WriteLine($"Day 10: 1-jolt difference ({diffs1}) * 1-jolt difference ({diffs3}) = {diffs1 * diffs3}."); // 1890
        }

        // What is the total number of distinct ways you can arrange the adapters to connect the charging outlet to your device?
        static void Part2()
        {
            var nrs = File.ReadLines("input/10").Select(Int64.Parse).Append(0).ToList();
            nrs.Sort();
            nrs.Add(nrs.Max() + 3);

            var sequentialOnes = 0;
            long arrangements = 1;

            for (int i = 1; i < nrs.Count(); i++)
            {
                long diff = nrs[i] - nrs[i-1];

                if (diff == 1)
                {
                    sequentialOnes += 1;
                }
                else // diff == 3
                {
                    arrangements *= _Tribonacci[sequentialOnes];
                    sequentialOnes = 0;
                }
            }

            Console.WriteLine($"Arrangements: {arrangements}"); // 49607173328384
        }
    }
}
