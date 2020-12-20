using System;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode
{
    class Day15
    {
        public static void Start()
        {
            NaiveElfMemoryGame("16,1,0,18,12,14,19", 2020);

            // NaiveElfMemoryGame("16,1,0,18,12,14,19", 30000000); // too slow
            WittyElfMemoryGame("16,1,0,18,12,14,19", 30000000);
        }

        static void NaiveElfMemoryGame(string input, int nrIterations)
        {
            var nrs = input.Split(",").Select(int.Parse).ToList();

            while (nrs.Count() < nrIterations)
            {
                var found = false;
                var lastSpoken = nrs.Last();

                for (int i = nrs.Count() - 2; i >= 0; i--)
                {
                    if (nrs[i] == lastSpoken)
                    {
                        nrs.Add(nrs.Count() - 1 - i);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    nrs.Add(0);
                }
            }

            Console.WriteLine($"Day 15: The {nrIterations}-th spoken number is {nrs.Last()}."); // 929
        }

        static void WittyElfMemoryGame(string input, int nrIterations)
        {
            var nrs = input.Split(",").Select(int.Parse).ToList();
            var lastSpokenIndices = new Dictionary<int, int>();

            var addNow = nrs.Last();
            nrs.Remove(addNow);

            for (int i = 0; i < nrs.Count(); i++)
            {
                lastSpokenIndices[nrs[i]] = i;
            }

            while (nrs.Count() < nrIterations)
            {
                var lastIndex = 0;
                var addNext = 0;

                if (lastSpokenIndices.TryGetValue(addNow, out lastIndex))
                {
                    addNext = nrs.Count() - lastIndex;
                }

                lastSpokenIndices[addNow] = nrs.Count();
                nrs.Add(addNow);

                addNow = addNext;
            }

            Console.WriteLine($"Day 15: The {nrIterations}-th spoken number is {nrs.Last()}."); // 16671510
        }
    }
}
