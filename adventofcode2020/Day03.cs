using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode
{
    class Day03
    {
        public static void start()
        {
            part1();
            part2();
        }

        static void part1()
        {
            var mapList = new List<char[]>();

            foreach (var line in File.ReadLines("input/3"))
            {
                mapList.Add(line.ToArray<char>());
            }

            var map = mapList.ToArray();

            Console.WriteLine($"Day 03: I hit {TreesHit(map, 3, 1)} trees."); // 280
        }

        static void part2()
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

            Console.WriteLine($"Day 03: I hit {totalTreesHit} trees in total."); // 4355551200
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
    }
}
