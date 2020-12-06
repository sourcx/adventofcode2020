using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day05
    {
        public static void start()
        {
            part1();
            part2();
        }

        static void part1()
        {
            Console.WriteLine($"Day 05: Highest seat ID: {GetAllSeatIDs().Max()}.");
        }

        static void part2()
        {
            var bookedSeats = GetAllSeatIDs();
            bookedSeats.Sort();

            var allSeats = Enumerable.Range(bookedSeats.First(), bookedSeats.Last()).ToList();

            // First one should be missing one
            foreach (var seat in allSeats.Except(bookedSeats))
            {
                Console.WriteLine($"Day 05: {seat} is my seat."); // 517
                return;
            }
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
    }
}
