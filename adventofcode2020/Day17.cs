using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day17
    {
        public static void Start()
        {
            Part1();
            Part2();
        }

        static void Part1()
        {
            var pocketDimension = ReadInput3D();

            // Keeps track of active neighbours per iteration because we have to update all fields at once.
            var nrNeighbours = new Dictionary<(int x, int y, int z), int>();

            // [-1, 0, -1] in 3 dimensions.
            var offsets = Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                        .Select(z => (x, y, z))))
                        .Where(xzy => xzy != (0, 0, 0));

            for (int i = 0; i < 6; i++)
            {
                foreach (var xyz in pocketDimension.Keys)
                {
                    nrNeighbours[xyz] = 0;
                }

                foreach (var ((x, y, z), _) in pocketDimension.Where(kv => kv.Value))
                {
                    foreach (var (dx, dy, dz) in offsets)
                    {
                        nrNeighbours[(x + dx, y + dy, z + dz)] = nrNeighbours.GetValueOrDefault((x + dx, y + dy, z + dz)) + 1;
                    }
                }

                foreach (var (xyz, nr) in nrNeighbours)
                {
                    pocketDimension[xyz] = pocketDimension.GetValueOrDefault(xyz) switch
                    {
                        true => (nr == 2 || nr == 3),
                        false => (nr == 3)
                    };
                }
            }

            var nrActive = pocketDimension.Where(kv => kv.Value).Count();
            Console.WriteLine($"Day 17: Number of active cubes 3D {nrActive}."); // 322
        }

        private static Dictionary<(int x, int y, int z), bool> ReadInput3D()
        {
            var lines = File.ReadLines("input/17").ToList();
            var matrix = new Dictionary<(int x, int y, int z), bool>();

            for (int y = 0; y < lines.Count(); y++)
            {
                for (int x = 0; x < lines[y].Count(); x++)
                {
                    matrix[(x, y, 0)] = (lines[y][x] == '#');
                }
            }

            return matrix;
        }

        static void Part2()
        {
            var pocketDimension = ReadInput4D();

            var nrNeighbours = new Dictionary<(int x, int y, int z, int w), int>();

            var offsets = Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                        .SelectMany(z => Enumerable.Range(-1, 3)
                            .Select(w => (x, y, z, w))))
                            .Where(xzyw => xzyw != (0, 0, 0, 0)));

            for (int i = 0; i < 6; i++)
            {
                foreach (var xyzw in pocketDimension.Keys)
                {
                    nrNeighbours[xyzw] = 0;
                }

                foreach (var ((x, y, z, w), _) in pocketDimension.Where(kv => kv.Value))
                {
                    foreach (var (dx, dy, dz, dw) in offsets)
                    {
                        nrNeighbours[(x + dx, y + dy, z + dz, w + dw)] = nrNeighbours.GetValueOrDefault((x + dx, y + dy, z + dz, w + dw)) + 1;
                    }
                }

                foreach (var (xyzw, nr) in nrNeighbours)
                {
                    pocketDimension[xyzw] = pocketDimension.GetValueOrDefault(xyzw) switch
                    {
                        true => (nr == 2 || nr == 3),
                        false => (nr == 3)
                    };
                }
            }

            var nrActive = pocketDimension.Where(kv => kv.Value).Count();
            Console.WriteLine($"Day 17: Number of active cubes 4D {nrActive}."); // 2000
        }

        private static Dictionary<(int x, int y, int z, int w), bool> ReadInput4D()
        {
            var lines = File.ReadLines("input/17").ToList();
            var matrix = new Dictionary<(int x, int y, int z, int w), bool>();

            for (int y = 0; y < lines.Count(); y++)
            {
                for (int x = 0; x < lines[y].Count(); x++)
                {
                    matrix[(0, 0, y, x)] = (lines[y][x] == '#');
                }
            }

            return matrix;
        }
    }
}
