using System;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day12
    {
        class Coords
        {
            public int x, y;

            public Coords (int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public static Coords operator+(Coords one, Coords other)
            {
                return new Coords(one.x + other.x, one.y + other.y);
            }

            public static Coords operator*(Coords one, int n)
            {
                return new Coords(one.x * n, one.y * n);
            }

            public int Manhattan()
            {
                return Math.Abs(x) + Math.Abs(y);
            }
        }

        public static void Start()
        {
            Part1();
            Part2();
        }

        // What is the Manhattan distance between that location and the ship's starting position?
        static void Part1()
        {
            var instructions = File.ReadLines("input/12").ToList();

            var pos = new Coords(0, 0);
            var shipDirection = new Coords(1, 0); // east

            foreach (var instruction in instructions)
            {
                var direction = instruction[0];
                var amount = int.Parse(instruction[1..]);

                switch (direction)
                {
                    case 'N':
                        pos += new Coords(0, -1) * amount;
                        break;
                    case 'E':
                        pos += new Coords(1, 0) * amount;
                        break;
                    case 'S':
                        pos += new Coords(0, 1) * amount;
                        break;
                    case 'W':
                        pos += new Coords(-1, 0) * amount;
                        break;
                    case 'F':
                        pos += shipDirection * amount;
                        break;
                    case 'L':
                    case 'R':
                        shipDirection = Rotate(shipDirection, direction, amount);
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine($"Day 12: Manhattan distance {pos.Manhattan()}"); // 590
        }

        private static Coords Rotate(Coords shipDirection, char direction, int degrees)
        {
            var radians = Math.Atan2(shipDirection.y, shipDirection.x);
            var radiansToAdd = degrees * Math.PI / 180;
            var radius = Math.Sqrt(Math.Pow(shipDirection.x, 2) + Math.Pow(shipDirection.y, 2));

            if (direction == 'L')
            {
                radiansToAdd *= -1;
            }

            var newRadians = (radians + radiansToAdd) % (Math.PI * 2);

            int x = (int) Math.Round(radius * Math.Cos(newRadians));
            int y = (int) Math.Round(radius * Math.Sin(newRadians));

            return new Coords(x, y);
        }

        // What is the Manhattan distance between that location and the ship's starting position?
        static void Part2()
        {
            var instructions = File.ReadLines("input/12").ToList();

            var shipPos = new Coords(0, 0);
            var waypointPos = new Coords(10, -1); // relative to ship

            foreach (var instruction in instructions)
            {
                var direction = instruction[0];
                var amount = int.Parse(instruction[1..]);

                switch (direction)
                {
                    case 'N':
                        waypointPos += new Coords(0, -1) * amount;
                        break;
                    case 'E':
                        waypointPos += new Coords(1, 0) * amount;
                        break;
                    case 'S':
                        waypointPos += new Coords(0, 1) * amount;
                        break;
                    case 'W':
                        waypointPos += new Coords(-1, 0) * amount;
                        break;
                    case 'F':
                        shipPos += waypointPos * amount;
                        break;
                    case 'L':
                    case 'R':
                        waypointPos = Rotate(waypointPos, direction, amount);
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine($"Day 12: Manhattan distance {shipPos.Manhattan()}"); // 42013
        }
    }
}
