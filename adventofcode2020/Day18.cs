using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;

namespace adventofcode
{
    class Day18
    {
        public static void Start()
        {
            Part1();
            Part2();
        }

        // Evaluate the expression on each line of the homework; what is the sum of the resulting values?
        static void Part1()
        {
            var sum = File.ReadLines("input/18").Select(Solve).Sum();
            Console.WriteLine($"Day 18: Homework sum: {sum}."); // 4297397455886
        }

        private static long Solve(string line)
        {
            long lhs = NextNr(line, out line);

            while (line.Count() > 0)
            {
                var op = NextOp(line, out line);
                long rhs = NextNr(line, out line);

                lhs = op switch
                {
                    '*' => lhs * rhs,
                    '+' => lhs + rhs,
                    _ => 0
                };
            }

            return lhs;
        }

        private static long NextNr(string line, out string lineOut)
        {
            if (char.IsDigit(line, 0))
            {
                lineOut = line[1..];
                return (long) char.GetNumericValue(line[0]);
            }
            else // if (line[0] == '(')
            {
                var pos = MatchBracket(line[1..]);
                lineOut = line[(pos + 2)..];
                return Solve(line.Substring(1, pos));
            }
        }

        private static int MatchBracket(string line)
        {
            int depth = 0;

            for (int i = 0; i < line.Count(); i++)
            {
                switch (line[i])
                {
                    case '(':
                        depth += 1;
                        break;
                    case ')':
                        if (depth == 0)
                        {
                            return i;
                        }
                        else
                        {
                            depth -= 1;
                        }
                        break;
                }
            }

            Console.WriteLine($"Error, cannot find closing brace in {line}");
            return -1;
        }

        private static char NextOp(string line, out string lineOut)
        {
            lineOut = line[3..];
            return line[1];
        }

        static void Part2()
        {
            var sum = File.ReadLines("input/18").Select(Solve2).Sum();
            Console.WriteLine($"Day 18: Homework sum new rules      : {sum}."); // 93000656194428
        }

        // What do you get if you add up the results of evaluating the homework problems using these new rules?
        // Completely different approach from Part1...
        private static long Solve2(string line)
        {
            // Solve all between braces first
            for (int i = 0; i < line.Count(); i++)
            {
                if (line[i] == '(')
                {
                    int len = MatchBracket(line[(i + 1)..]);
                    line = line[0..i] + Solve2(line[(i + 1)..(i + len + 1)]) + line[(i + len + 2)..];
                }
            }

            // We have a list of numbers, '+' and '*'.
            int pos = line.IndexOf('+');

            while (pos != -1)
            {
                var left = GetNrFromLeft(line, pos - 1);
                var right = GetNrFromRight(line, pos + 1);

                line = line[0..(pos - 1 - left.ToString().Count())]
                    + (long.Parse(left) + long.Parse(right))
                    + line[(pos + 2 + right.ToString().Count())..];

                pos = line.IndexOf('+');
            }

            // We have a list of numbers and '*'.
            pos = line.IndexOf('*');

            while (pos != -1)
            {
                var left = GetNrFromLeft(line, pos - 1);
                var right = GetNrFromRight(line, pos + 1);

                line = line[0..(pos - 1 - left.ToString().Count())]
                    + (long.Parse(left) * long.Parse(right))
                    + line[(pos + 2 + right.ToString().Count())..];

                pos = line.IndexOf('*');
            }

            var res = long.Parse(line);

            return res;
        }

        // Scans to the left and gets the number, removes it from the line.
        private static string GetNrFromLeft(string line, int pos)
        {
            var nr = "";

            int offset = 1;
            while (pos - offset >= 0 && char.IsDigit(line[pos - offset]))
            {
                nr = line[pos - offset] + nr;
                offset += 1;
            }

            return nr;
        }

        // Scans to the right and gets the number, removes it from the line.
        private static string GetNrFromRight(string line, int pos)
        {
            var nr = "";

            int offset = 1;
            while (pos + offset < line.Count() && char.IsDigit(line[pos + offset]))
            {
                nr += line[pos + offset];
                offset += 1;
            }

            return nr;
        }
    }
}
