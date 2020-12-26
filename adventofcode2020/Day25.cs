using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day25
    {

        static List<(int, int)> _directions = new List<(int, int)>()
        {
            (-1, 1), (0, 1), (1, 0), (0, -1), (1, -1), (-1, 0)
        };

        public static void Start()
        {
            var tiles = ReadInput("input/24");

            var blackTiles = new HashSet<(int, int)>();

            foreach (var tile in tiles)
            {
                if (blackTiles.Contains(tile))
                {
                    blackTiles.Remove(tile);
                }
                else
                {
                    blackTiles.Add(tile);
                }
            }

            // Part1: how many tiles are left with the black side up?
            Console.WriteLine($"Tiles with black side up: {blackTiles.Count()}"); // 473

            for (int i = 0; i < 100; i++)
            {
                blackTiles = NextDay(blackTiles);
            }

            // Part2: How many tiles will be black after 100 days?
            Console.WriteLine($"Tiles with black side up: {blackTiles.Count()}"); // 4070
        }

        static IEnumerable<(int, int)> ReadInput(string file)
        {
            return File.ReadAllLines(file).Select(line => ReadTile(line));
        }

        static (int, int) ReadTile(string line)
        {
            var pos = (0, 0);

            int i = 0;
            while (i < line.Length)
            {
                switch (line[i])
                {
                    case 'n': // nw (-1, 1), ne (0, 1)
                        pos.Item2 += 1;
                        if (line[i + 1] == 'w')
                        {
                            pos.Item1 -= 1;
                        }
                        i += 2;
                        break;
                    case 'e': // e (1, 0)
                        pos.Item1 += 1;
                        i += 1;
                        break;
                    case 's': // sw (0, -1), se (1, -1)
                        pos.Item2 -= 1;
                        if (line[i + 1] == 'e')
                        {
                            pos.Item1 += 1;
                        }
                        i += 2;
                        break;
                    case 'w': // w (-1, 0)
                        pos.Item1 -= 1;
                        i += 1;
                        break;
                }
            }

            return pos;
        }

        static HashSet<(int, int)> NextDay(HashSet<(int, int)> blackTiles)
        {
            var tilesToCheck = new HashSet<(int, int)>();
            var newBlackTiles = new HashSet<(int, int)>();

            foreach (var tile in blackTiles)
            {
                tilesToCheck.Add(tile);

                foreach (var direction in _directions)
                {
                    tilesToCheck.Add((tile.Item1 + direction.Item1, tile.Item2 + direction.Item2));
                }
            }

            foreach (var tile in tilesToCheck)
            {
                var blackNeighbours = NrBlackNeighbours(blackTiles, tile);

                if (blackTiles.Contains(tile))
                {
                    if (!(blackNeighbours == 0 || blackNeighbours > 2))
                    {
                        newBlackTiles.Add(tile);
                    }
                }
                else
                {
                    if (blackNeighbours == 2)
                    {
                        newBlackTiles.Add(tile);
                    }
                }
            }

            return newBlackTiles;
        }

        static int NrBlackNeighbours(HashSet<(int, int)> blackTiles, (int, int) tile)
        {
            return _directions.Where(d => (blackTiles.Contains((tile.Item1 + d.Item1, tile.Item2 + d.Item2)))).Count();
        }
    }
}
