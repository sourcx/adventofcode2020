using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day09
    {
        public static void start()
        {
            var failedSum = part1();
            part2(failedSum);
        }

        static long part1()
        {
            var nrs = ReadNrs();
            var preamble = 25;

            for (var i = preamble; i < nrs.Count; ++i)
            {
                if (!SumsBefore(nrs, i - preamble, preamble).Contains(nrs[i]))
                {
                    Console.WriteLine($"Day 09: The sum {nrs[i]} can not be made."); // 731031916
                    return nrs[i];
                }
            }

            return -1;
        }

        static void part2(long sum)
        {
            var nrs = ReadNrs();
            var sequence = FindSequenceToSum(nrs, sum);
            var encryptionWeakness = sequence.Min() + sequence.Max();

            Console.WriteLine($"The XMAS encryption weakness: {encryptionWeakness}.");
        }

        static List<long> ReadNrs()
        {
            var nrs = new List<long>();

            foreach (var nr in File.ReadLines("input/9"))
            {
                nrs.Add(Int64.Parse(nr));
            }

            return nrs;
        }

        private static IEnumerable<long> SumsBefore(List<long> nrs, int start, int length)
        {
            var sums = new List<long>();

            for (var i = start; i < start + length; i++)
            {
                for (var j = start + 1; j < start + length; j++)
                {
                    sums.Add(nrs[i] + nrs[j]);
                }
            }

            return sums.Distinct();
        }

        static List<long> FindSequenceToSum(List<long> nrs, long toFind)
        {
            var sequence = new List<long>();

            for (int start = 0; start < (nrs.Count - 2); start++)
            {
                sequence.Clear();
                sequence.Add(nrs[start]);

                for (int offset = 1; offset < (nrs.Count - start); offset++)
                {
                    sequence.Add(nrs[start + offset]);

                    if (sequence.Sum() == toFind)
                    {
                        return sequence;
                    }

                    if (sequence.Sum() > toFind)
                    {
                        break;
                    }
                }
            }

            return sequence;
        }
    }
}
