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
            // Day3_part2();
            // Day4();
            // Day4_part2();
            // Day5();
            // Day5_part2();
            Day6();
            Day6_part2();
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

            Console.WriteLine($"I hit {TreesHit(map, 3, 1)} trees."); // 280
        }

        static int TreesHit(char[][] map, int x_delta, int y_delta)
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

            totalTreesHit *= TreesHit(map, 1, 1);
            totalTreesHit *= TreesHit(map, 3, 1);
            totalTreesHit *= TreesHit(map, 5, 1);
            totalTreesHit *= TreesHit(map, 7, 1);
            totalTreesHit *= TreesHit(map, 1, 2);

            Console.WriteLine($"I hit {totalTreesHit} trees in total."); // 4355551200
        }

        // In your batch file, how many passports are valid?
        static void Day4()
        {
            CheckPassports(PassportHasRequiredFields); // 256
        }

        static void Day4_part2()
        {
            CheckPassports(PassportHasValidFields); // kleiner dan not 199
        }

        static void CheckPassports(Func<Dictionary<string, string>, bool> IsValidPassport)
        {
            int validPassports = 0;

            var passport = new Dictionary<string, string>();

            foreach (var line in File.ReadLines("input/4"))
            {
                if (line == String.Empty)
                {
                    if (IsValidPassport(passport))
                    {
                        validPassports += 1;
                    }
                    passport.Clear();
                }
                else
                {
                    foreach (Match match in Regex.Matches(line, @"(\w\w\w:\S+)"))
                    {
                        var k = match.Value.Split(":")[0];
                        var v = match.Value.Split(":")[1];
                        passport.Add(k, v);
                    }
                }
            }

            if (IsValidPassport(passport))
            {
                validPassports += 1;
            }

            Console.WriteLine($"There are {validPassports} valid passports.");
        }

        static List<string> MANDATORY_FIELDS = new List<string>() {
            "byr",  "iyr",  "eyr",  "hgt",  "hcl",  "ecl",  "pid"
        };

        static bool PassportHasRequiredFields(Dictionary<string, string> passport)
        {
            foreach (var field in MANDATORY_FIELDS)
            {
                if (!passport.Keys.Contains(field))
                {
                    return false;
                }
            }

            return true;
        }

        static List<string> EYECOLORS = new List<string>() {
            "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
        };

        static bool PassportHasValidFields(Dictionary<string, string> passport)
        {
            foreach (var field in MANDATORY_FIELDS)
            {
                if (!passport.Keys.Contains(field))
                {
                    return false;
                }
            }

            // byr (Birth Year) - four digits; at least 1920 and at most 2002.
            var byr = Int32.Parse(passport["byr"]);
            if (byr < 1920 || byr > 2002)
            {
                return false;
            }

            // iyr (Issue Year) - four digits; at least 2010 and at most 2020.
            var iyr = Int32.Parse(passport["iyr"]);
            if (iyr < 2010 || iyr > 2020)
            {
                return false;
            }

            // eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
            var eyr = Int32.Parse(passport["eyr"]);
            if (eyr < 2020 || eyr > 2030)
            {
                return false;
            }

            // hgt (Height) - a number followed by either cm or in:
            //                If cm, the number must be at least 150 and at most 193.
            //                If in, the number must be at least 59 and at most 76.
            var hgt = passport["hgt"];
            if (hgt.Contains("cm"))
            {
                var cm = Int32.Parse(hgt.Split("cm")[0]);
                if (cm < 150 || cm > 193)
                {
                    return false;
                }
            }
            else if (hgt.Contains("in"))
            {
                var inch = Int32.Parse(hgt.Split("in")[0]);
                if (inch < 59 || inch > 76)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            // hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
            var hcl = passport["hcl"];
            if (!Regex.Match(hcl, @"#[0-9,a-f]{6}").Success)
            {
                return false;
            }

            // ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
            var ecl = passport["ecl"];
            if (!EYECOLORS.Contains(ecl))
            {
                return false;
            }

            // pid (Passport ID) - a nine-digit number, including leading zeroes.
            var pid = passport["pid"];
            if (!Regex.Match(pid, @"^[0-9]{9}$").Success)
            {
                return false;
            }

            return true;
        }

        // What is the highest seat ID on a boarding pass?
        static void Day5()
        {
            Console.WriteLine($"Highest seat ID: {GetAllSeatIDs().Max()}.");
        }

        static List<int> GetAllSeatIDs()
        {
            const int MAX_SEAT_ROW = 128;
            const int MAX_SEAT_COL = 8;

            var ids = new List<int>();

            foreach (var line in File.ReadLines("input/5"))
            {
                var rowInstructions = line.Substring(0, 7);
                int row = PartitionBinarySpace(rowInstructions, 'F', 'B', MAX_SEAT_ROW);

                var colInstructions = line.Substring(7, 3);
                int col = PartitionBinarySpace(colInstructions, 'L', 'R', MAX_SEAT_COL);

                var seatId = MAX_SEAT_COL * row + col;
                ids.Add(seatId);
                // Console.WriteLine($"{line}: row {row}, col {col}, seat ID {seatId}.");
            }

            return ids;
        }

        static int PartitionBinarySpace(string instructions, char lowerHalf, char upperHalf, int max)
        {
            int lo = 0;
            int hi = max;

            foreach (char instruction in instructions)
            {
                if (instruction == lowerHalf)
                {
                    hi -= ((hi - lo) / 2);
                }

                if (instruction == upperHalf)
                {
                    lo += ((hi - lo) / 2);
                }
            }

            return lo;
        }

        static void Day5_part2()
        {
            var bookedSeats = GetAllSeatIDs();
            bookedSeats.Sort();

            var allSeats = Enumerable.Range(bookedSeats.First(), bookedSeats.Last()).ToList();

            // First one should be missing one
            foreach (var seat in allSeats.Except(bookedSeats))
            {
                Console.WriteLine($"{seat} is my seat.");
                return;
            }
        }

        // What is the sum of those counts?
        static void Day6()
        {
            var answers = new HashSet<char>();
            int sum = 0;

            foreach (var line in File.ReadLines("input/6_example"))
            {
                if (line == String.Empty)
                {
                    Console.WriteLine($"Last set had {answers.Count} answers.");
                    sum += answers.Count;
                    answers.Clear();
                }

                foreach (var c in line.ToArray())
                {
                    answers.Add(c);
                }
            }

            Console.WriteLine($"Last set had {answers.Count} answers.");
            sum += answers.Count;

            Console.WriteLine($"Sum of count is {sum}.");
        }

        static void Day6_part2()
        {
            HashSet<char> answers = null;
            int sum = 0;

            foreach (var line in File.ReadLines("input/6"))
            {
                if (line == String.Empty)
                {
                    Console.WriteLine($"Last set had {answers.Count} answers.");
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

            Console.WriteLine($"Last set had {answers.Count} answers.");
            sum += answers.Count;

            Console.WriteLine($"Sum of count is {sum}.");
        }
    }
}
