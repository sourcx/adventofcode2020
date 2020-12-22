using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day22
    {
        // What is the winning player's score?
        public static void Start()
        {
            // Part1();
            Part2();
        }

        static void Part1()
        {
            // Sorry Regex-Santa...
            var player1 = File.ReadAllText("input/22")[10..].Split("\nPlayer 2:\n")[0].Trim().Split("\n").Select(int.Parse).ToList();
            var player2 = File.ReadAllText("input/22")[10..].Split("\nPlayer 2:\n")[1].Trim().Split("\n").Select(int.Parse).ToList();

            while (!(player1.Count() == 0 || player2.Count() == 0))
            {
                var card1 = player1.First();
                player1.RemoveAt(0);

                var card2 = player2.First();
                player2.RemoveAt(0);

                if (card1 > card2)
                {
                    player1.Add(card1);
                    player1.Add(card2);
                }
                else
                {
                    player2.Add(card2);
                    player2.Add(card1);
                }
            }

            Console.WriteLine("Player 1:");
            foreach (var card in player1)
            {
                Console.WriteLine(card);
            }
            Console.WriteLine("\nPlayer 2:");
            foreach (var card in player1)
            {
                Console.WriteLine(card);
            }

            var winner = player1.Count() > 0 ? player1 : player2;

            var score = 0L;
            for (int i = 0; i < winner.Count(); i++)
            {
                score += winner[i] * (winner.Count() - i);
            }

            Console.WriteLine($"Winner score: {score}"); // 34664

            // var sortedAllergens = allergensWithIngredients.Keys.ToList().OrderBy(a => a);
            // var danger = sortedAllergens.Select(allergen => allergensWithIngredients[allergen].First()).Aggregate("", (i, j) => i + "," + j)[1..];

            // // Part 2: What is your canonical dangerous ingredient list?
            // Console.WriteLine($"Canonical dangerous ingredient list: {danger}"); // fntg,gtqfrp,xlvrggj,rlsr,xpbxbv,jtjtrd,fvjkp,zhszc
        }


        static void Part2()
        {
            // Sorry Regex-Santa...
            var player1 = File.ReadAllText("input/22")[10..].Split("\nPlayer 2:\n")[0].Trim().Split("\n").Select(int.Parse).ToList();
            var player2 = File.ReadAllText("input/22")[10..].Split("\nPlayer 2:\n")[1].Trim().Split("\n").Select(int.Parse).ToList();

            List<int> winningHand = null;
            var winner = Play(new List<int>(player1), new List<int>(player2), out winningHand);

            var score = 0L;
            for (int i = 0; i < winningHand.Count(); i++)
            {
                score += winningHand[i] * (winningHand.Count() - i);
            }

            Console.WriteLine($"Winner score: {score}"); // 32018
        }

        static int Play(List<int> player1, List<int> player2, out List<int> winningHand)
        {
            var playedHands = new List<string>(); // only store for player1

            while (!(player1.Count() == 0 || player2.Count() == 0))
            {
                var handToString = player1.Aggregate("", (i, j) => i + "," + j)[1..];
                if (playedHands.Contains(handToString))
                {
                    winningHand = null;
                    return 1;
                }

                playedHands.Add(handToString);

                var card1 = player1.First();
                player1.RemoveAt(0);

                var card2 = player2.First();
                player2.RemoveAt(0);

                if (player1.Count() >= card1 && player2.Count() >= card2)
                {
                    // recursive
                    var winner = Play(player1.Take(card1).ToList(), player2.Take(card2).ToList(), out winningHand);

                    if (winner == 1)
                    {
                        player1.Add(card1);
                        player1.Add(card2);
                    }
                    else
                    {
                        player2.Add(card2);
                        player2.Add(card1);
                    }
                }
                else
                {
                    // normal
                    if (card1 > card2)
                    {
                        player1.Add(card1);
                        player1.Add(card2);
                    }
                    else
                    {
                        player2.Add(card2);
                        player2.Add(card1);
                    }
                }
            }

            if (player1.Count() > 0)
            {
                winningHand = new List<int>(player1);
                return 1;
            }
            else
            {
                winningHand = new List<int>(player2);
                return 2;
            }
        }
    }
}
