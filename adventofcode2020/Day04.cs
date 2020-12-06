using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day04
    {
        public static void start()
        {
            part1();
            part2();
        }

        static void part1()
        {
            CheckPassports(PassportHasRequiredFields); // 256

        }

        static void part2()
        {
            CheckPassports(PassportHasValidFields); // 198
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

            Console.WriteLine($"Day 04: There are {validPassports} valid passports.");
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
    }
}
