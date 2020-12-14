using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day14
    {
        public static void Start()
        {
            Part1();
            Part2();
        }

        // What is the sum of all values left in memory after it completes?
        static void Part1()
        {
            var memory = new Dictionary<long, long>();
            string mask = "";

            foreach (var line in File.ReadLines("input/14"))
            {
                if (line.StartsWith("mask"))
                {
                    mask = line.Split(" ")[2];
                }
                else
                {
                    var match = Regex.Match(line, @"mem\[(\d+)\] = (\d+)");
                    var addr = long.Parse(match.Groups[1].Value);
                    var val = match.Groups[2].Value;

                    memory[addr] = Mask(mask, val);
                }
            }

            long sum = memory.Aggregate(0L, (sum, kv) => sum + kv.Value);

            Console.WriteLine($"Day 14: Sum of all values is {sum}."); // 14553106347726
        }

        private static long Mask(string mask, string val)
        {
            var binaryVal = Convert.ToString(long.Parse(val), 2);
            binaryVal = binaryVal.PadLeft(mask.Count(), '0');

            var maskedVal = new List<char>();

            for (int i = 0; i < binaryVal.Count(); i++)
            {
                maskedVal.Add(mask[i] == 'X' ? binaryVal[i] : mask[i]);
            }

            return Convert.ToInt64(new string(maskedVal.ToArray()), 2);
        }

        // What is the sum of all values left in memory after it completes?
        static void Part2()
        {
            var memory = new Dictionary<long, long>();
            string mask = "";

            foreach (var line in File.ReadLines("input/14"))
            {
                if (line.StartsWith("mask"))
                {
                    mask = line.Split(" ")[2];
                }
                else
                {
                    var match = Regex.Match(line, @"mem\[(\d+)\] = (\d+)");
                    var addressStr = match.Groups[1].Value;

                    foreach (var address in DecodeAddress(addressStr, mask))
                    {
                        memory[address] = long.Parse(match.Groups[2].Value);
                    }
                }
            }

            long sum = memory.Aggregate(0L, (sum, kv) => sum + kv.Value);

            Console.WriteLine($"Day 14: Sum of all values is {sum}."); // 2737766154126
        }

        private static List<long> DecodeAddress(string address, string mask)
        {
            var addresses = GetPermutations(Mask2(mask, address));
            return addresses.Select(addr => Convert.ToInt64(addr, 2)).ToList();
        }

        private static List<string> GetPermutations(string address)
        {
            if (!address.Contains('X'))
            {
                return new List<string>() { address };
            }

            var xPos = address.IndexOf('X');

            if (xPos > -1)
            {
                var arr = new string(address).ToCharArray();
                arr[xPos] = '0';
                var replacedWith0 = new string(arr);
                arr[xPos] = '1';
                var replacedWith1 = new string(arr);

                return GetPermutations(replacedWith0).Concat(GetPermutations(replacedWith1)).ToList();
            }

            return null;
        }

        private static string Mask2(string mask, string val)
        {
            var binaryVal = Convert.ToString(long.Parse(val), 2);
            binaryVal = binaryVal.PadLeft(mask.Count(), '0');

            var maskedVal = new List<char>();

            for (int i = 0; i < binaryVal.Count(); i++)
            {
                switch (mask[i])
                {
                    case '0':
                        maskedVal.Add(binaryVal[i]);
                        break;
                    case '1':
                        maskedVal.Add('1');
                        break;
                    case 'X':
                        maskedVal.Add('X');
                        break;
                }
            }

            return new string(maskedVal.ToArray());
        }
    }
}
