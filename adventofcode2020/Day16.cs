using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day16
    {
        public static void Start()
        {
            Part1();
            Part2();
        }

        class TicketField
        {
            public int Position
            {
                get
                {
                    return PossiblePositions.First();
                }
            }
            public HashSet<int> PossiblePositions { get ; set; }
            public string Name { get; }

            private int _range1Low;
            private int _range1High;
            private int _range2Low;
            private int _range2High;

            public TicketField(string input)
            {
                var match = Regex.Match(input, @"([\w\s]*): (\d+)-(\d+) or (\d+)-(\d+)");
                Name = match.Groups[1].Value;
                _range1Low = int.Parse(match.Groups[2].Value);
                _range1High = int.Parse(match.Groups[3].Value);
                _range2Low = int.Parse(match.Groups[4].Value);
                _range2High = int.Parse(match.Groups[5].Value);
            }

            public bool FitsConditions(int n)
            {
                return (_range1Low <= n && n <= _range1High) ||
                       (_range2Low <= n && n <= _range2High);
            }

            public bool FitConditions(List<int> nrs)
            {
                foreach (var nr in nrs)
                {
                    if (!FitsConditions(nr))
                    {
                        return false;
                    }
                }

                return true;
            }

            // Sets possible positions for this field (all valid options).
            public void MatchWithTickets(List<List<int>> tickets)
            {
                PossiblePositions = new HashSet<int>();

                for (int i = 0; i < tickets[0].Count(); i++)
                {
                    var valid = true;

                    foreach (var ticket in tickets)
                    {
                        if (!FitsConditions(ticket[i]))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (valid)
                    {
                        PossiblePositions.Add(i);
                    }
                }
            }
        }

        class TicketFormat
        {
            public List<TicketField> fields;

            public TicketFormat(string input)
            {
                fields = new List<TicketField>();

                foreach (var line in input.Split("\n"))
                {
                    fields.Add(new TicketField(line));
                }
            }

            public bool FitsConditions(int n)
            {
                foreach (var field in fields)
                {
                    if (field.FitsConditions(n))
                    {
                        return true;
                    }
                }

                return false;
            }

            public void MatchWithTickets(List<List<int>> tickets)
            {
                // Iterate over all ticket fields and match them to the provided tickets.
                foreach (var field in fields)
                {
                    field.MatchWithTickets(tickets);
                }

                // Make it a tight fit, removing duplicates.
                while (fields.Max(field => field.PossiblePositions.Count()) > 1)
                {
                    RemoveDuplicateOptions();
                }
            }

            private void RemoveDuplicateOptions()
            {
                foreach (var field in fields)
                {
                    if (field.PossiblePositions.Count() == 1)
                    {
                        var nr = field.PossiblePositions.First();

                        foreach (var otherField in fields)
                        {
                            if (otherField.PossiblePositions.Count() == 1)
                            {
                                // We have ourselves, or a bigger a problem...
                            }
                            else if (otherField.PossiblePositions.Contains(nr))
                            {
                                otherField.PossiblePositions.Remove(nr);
                            }
                        }
                    }
                }
            }
        }

        // What is your ticket scanning error rate?
        private static void Part1()
        {
            var input = File.ReadAllText("input/16").Split("\n\n");
            var ticketFormat = new TicketFormat(input[0]);

            var myTicket = input[1].Split("\n")[1].Split(",").Select(int.Parse).ToList();

            var nearbyTickets = new List<List<int>>();
            foreach (var line in input[2].Trim().Split("\n")[1..])
            {
                nearbyTickets.Add(line.Split(",").Select(int.Parse).ToList());
            }

            var errorRate = 0;

            foreach (var ticket in nearbyTickets)
            {
                foreach (var n in ticket)
                {
                    if (!ticketFormat.FitsConditions(n))
                    {
                        errorRate += n;
                    }
                }
            }

            Console.WriteLine($"Day 16: Error rate: {errorRate}."); // 29878
        }

        // What do you get if you multiply those six values together?
        private static void Part2()
        {
            var input = File.ReadAllText("input/16").Split("\n\n");
            var ticketFormat = new TicketFormat(input[0]);

            var nearbyTickets = new List<List<int>>();
            foreach (var line in input[2].Trim().Split("\n")[1..])
            {
                nearbyTickets.Add(line.Split(",").Select(int.Parse).ToList());
            }

            var validTickets = FilterValidTickets(ticketFormat, nearbyTickets);
            ticketFormat.MatchWithTickets(validTickets);

            var myNumbers = input[1].Split("\n")[1].Split(",").Select(int.Parse).ToList();

            var product = ticketFormat.fields.Where(field => field.Name.StartsWith("departure")).Aggregate(1L, (product, field) => product *= myNumbers[field.Position]);
            Console.WriteLine($"Error rate: {product}."); // 855438643439
        }

        private static List<List<int>> FilterValidTickets(TicketFormat format, List<List<int>> nearbyTickets)
        {
            var validTickets = new List<List<int>>();

            foreach (var ticket in nearbyTickets)
            {
                bool valid = true;

                foreach (var n in ticket)
                {
                    if (!format.FitsConditions(n))
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    validTickets.Add(ticket);
                }
            }

            return validTickets;
        }
    }
}
