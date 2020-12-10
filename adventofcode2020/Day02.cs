using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day02
    {
        public static void Start()
        {
            Part1();
            Part2();
        }

        static void Part1()
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

            Console.WriteLine($"Day02: {correctPasswords} passwords are correct."); // 625
        }

        static void Part2()
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

            Console.WriteLine($"Day02: {correctPasswords} passwords are correct."); // 391
        }
    }
}
