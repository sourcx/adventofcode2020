using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day11
    {
        const char EMPTY = 'L';
        const char OCCUPIED = '#';
        const char FLOOR = '.';

        public static void Start()
        {
            Part1();
            Part2();
        }

        static void Part1()
        {
            CalculateOccupiedSeats(AssignSeatsForPart1); // 2324
        }

        static void Part2()
        {
            CalculateOccupiedSeats(AssignSeatsForPart2); // 2068
        }

        static void CalculateOccupiedSeats(Func<List<List<char>>, List<List<char>>> AssignSeats)
        {
            var seats = File.ReadLines("input/11").Select(line => new List<char>(line)).ToList();
            var newSeats = AssignSeats(seats);

            while (!Same(seats, newSeats))
            {
                seats = newSeats;
                newSeats = AssignSeats(seats);
            }

            Console.WriteLine($"Day 11: There are {NrOccupiedSeats(newSeats)} occupied seats after stabilization.");
        }

        static List<List<char>> AssignSeatsForPart1(List<List<char>> seats)
        {
            return AssignSeats(seats, ReplaceEmpty1, ReplaceOccupied1);
        }

        static List<List<char>> AssignSeatsForPart2(List<List<char>> seats)
        {
            return AssignSeats(seats, ReplaceEmpty2, ReplaceOccupied2 );
        }

        static List<List<char>> AssignSeats(List<List<char>> seats, Func<List<List<char>>, int, int, char> ReplaceEmpty, Func<List<List<char>>, int, int, char> ReplaceOccupied)
        {
            var newSeats = new List<List<char>>();

            for (int row = 0; row < seats.Count(); row++)
            {
                var newRow = new List<char>();

                for (int col = 0; col < seats[row].Count(); col++)
                {
                    switch (seats[row][col])
                    {
                        case EMPTY:
                            newRow.Add(ReplaceEmpty(seats, row, col));
                            break;
                        case OCCUPIED:
                            newRow.Add(ReplaceOccupied(seats, row, col));
                            break;
                        case FLOOR:
                            newRow.Add('.');
                            break;
                        default:
                            break;
                    }
                }

                newSeats.Add(newRow);
            }

            return newSeats;
        }

        // If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
        private static char ReplaceEmpty1(List<List<char>> seats, int row, int col)
        {
            return NrOccupiedAdjacent(seats, row, col) > 0 ? EMPTY : OCCUPIED;
        }

        // If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
        private static char ReplaceOccupied1(List<List<char>> seats, int row, int col)
        {
            return NrOccupiedAdjacent(seats, row, col) >= 4 ? EMPTY : OCCUPIED;
        }

        // If a seat is empty (L) and there are no occupied seats visible from it, the seat becomes occupied.
        private static char ReplaceEmpty2(List<List<char>> seats, int row, int col)
        {
            return NrOccupiedVisible(seats, row, col) > 0 ? EMPTY : OCCUPIED;
        }

        // If a seat is occupied (#) and five or more seats visible from it are also occupied, the seat becomes empty.
        private static char ReplaceOccupied2(List<List<char>> seats, int row, int col)
        {
            return NrOccupiedVisible(seats, row, col) >= 5 ? EMPTY : OCCUPIED;
        }

        private static int NrOccupiedAdjacent(List<List<char>> seats, int row, int col)
        {
            int adjacentOccupied = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (!ValidSeat(seats, row, col, x, y))
                    {
                        continue;
                    }

                    if (seats[row + x][col + y] == OCCUPIED)
                    {
                        adjacentOccupied += 1;
                    }
                }
            }

            return adjacentOccupied;
        }

        private static int NrOccupiedVisible(List<List<char>> seats, int row, int col)
        {
            int visibleOccupied = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (FirstSeatInDirection(seats, row, col, x, y) == OCCUPIED)
                    {
                        visibleOccupied += 1;
                    }
                }
            }

            return visibleOccupied;
        }

        private static char FirstSeatInDirection(List<List<char>> seats, int row, int col, int dX, int dY)
        {
            var offsetX = dX;
            var offsetY = dY;

            while (ValidSeat(seats, row, col, offsetX, offsetY))
            {
                if (seats[row + offsetX][col + offsetY] == EMPTY)
                {
                    return EMPTY;
                }

                if (seats[row + offsetX][col + offsetY] == OCCUPIED)
                {
                    return OCCUPIED;
                }

                offsetX += dX;
                offsetY += dY;
            }

            return FLOOR;
        }

        private static bool ValidSeat(List<List<char>> seats, int row, int col, int x, int y)
        {
            return !((x == 0 && y == 0) ||
                     (row + x < 0) ||
                     (col + y < 0) ||
                     (row + x >= seats.Count()) ||
                     (col + y >= seats[0].Count()));
        }

        private static bool Same(List<List<char>> one, List<List<char>> other)
        {
            for (int row = 0; row < one.Count(); row++)
            {
                for (int col = 0; col < one[0].Count(); col++)
                {
                    if (one[row][col] != other[row][col])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static int NrOccupiedSeats(List<List<char>> seats)
        {
            return seats.Select(row => row.Where(seat => (seat == OCCUPIED)).Count()).Sum();
        }
    }
}
