using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day19
    {
        static Dictionary<int, Rule> rules;
        static List<string> messages;

        public static void Start()
        {
            ReadInput("input/19");
            Part1();
            Part2();
        }

        class Rule
        {
            public char? _term;
            public List<List<int>> _sequences;

            public IEnumerable<string> Derive(string toFind)
            {
                if (_term is char term)
                {
                    yield return term.ToString();
                    yield break;
                }

                foreach (var sequence in _sequences)
                {
                    switch (sequence.Count)
                    {
                        case 1:
                        {
                            foreach (var message in DeriveOneConsecutive(sequence, toFind))
                            {
                                yield return message;
                            }
                            break;
                        }
                        case 2:
                        {
                            foreach (var message in DeriveTwoConsecutive(sequence, toFind))
                            {
                                yield return message;
                            }
                            break;
                        }
                        case 3:
                        {
                            foreach (var message in DeriveThreeConsecutive(sequence, toFind))
                            {
                                yield return message;
                            }
                            break;
                        }
                    }
                }
            }

            private IEnumerable<string> DeriveOneConsecutive(List<int> sequence, string toFind)
            {
                foreach (var s in rules[sequence[0]].Derive(toFind))
                {
                    yield return s;
                }
            }

            private IEnumerable<string> DeriveTwoConsecutive(List<int> sequence, string toFind)
            {
                var firstSentences = rules[sequence[0]].Derive(toFind);

                foreach (var first in firstSentences)
                {
                    if (!toFind.StartsWith(first))
                    {
                        continue;
                    }

                    var secondSentences = rules[sequence[1]].Derive(toFind.Substring(first.Length));

                    foreach (var second in secondSentences)
                    {
                        yield return first + second;
                    }
                }
            }

            private IEnumerable<string> DeriveThreeConsecutive(List<int> sequence, string toFind)
            {
                var firstSentences = rules[sequence[0]].Derive(toFind);

                foreach (var first in firstSentences)
                {
                    if (!toFind.StartsWith(first))
                    {
                        continue;
                    }

                    var secondSentences = rules[sequence[1]].Derive(toFind.Substring(first.Length));

                    foreach (var second in secondSentences)
                    {
                        if (!toFind.StartsWith(first + second))
                        {
                            continue;
                        }

                        var thirdSentences = rules[sequence[2]].Derive(toFind.Substring(first.Length + second.Length));

                        foreach (var third in thirdSentences)
                        {
                            yield return first + second + third;
                        }
                    }
                }
            }
        }

        // How many messages completely match rule 0?
        static void Part1()
        {
            var nrMatches = messages.Select(msg => rules[0].Derive(msg).Contains(msg)).Count(valid => valid);
            Console.WriteLine($"Day 19: There are {nrMatches} valid messages."); // 187
        }

        // After updating rules 8 and 11, how many messages completely match rule 0?
        static void Part2()
        {
            rules[8] = new Rule()
            {
                _sequences = new List<List<int>>()
                {
                    new List<int>() { 42 },
                    new List<int>() { 42, 8 }
                }
            };

            rules[11] = new Rule()
            {
                _sequences = new List<List<int>>()
                {
                    new List<int>() { 42, 31 },
                    new List<int>() { 42, 11, 31 }
                }
            };

            var nrMatches = messages.Select(msg => rules[0].Derive(msg).Contains(msg)).Count(valid => valid);

            Console.WriteLine($"Day 19: There are {nrMatches} valid messages."); // 392
        }

        static void ReadInput(string file)
        {
            rules = new Dictionary<int, Rule>();
            messages = new List<string>();

            foreach (var line in File.ReadLines(file))
            {
                if (line == "") continue;

                if (line.StartsWith("a") || line.StartsWith("b"))
                {
                    messages.Add(line);
                }
                else
                {
                    var nr = int.Parse(line.Split(": ")[0]);
                    var rest = line.Split(": ")[1];

                    if (rest.StartsWith("\""))
                    {
                        rules[nr] = new Rule() { _term = rest[1] };
                    }
                    else
                    {
                        var options = new List<List<int>>();
                        foreach (var option in line.Split(": ")[1].Split("|"))
                        {
                            options.Add(option.Trim().Split(" ").Select(int.Parse).ToList());
                        }

                        rules[nr] = new Rule() { _sequences = options };
                    }
                }
            }
        }
    }
}
