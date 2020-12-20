using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day20
    {
        public static void Start()
        {
            Part1();
            Part2();
        }

        class Tile
        {
            public Tile Left;
            public Tile Top;
            public Tile Right;
            public Tile Bottom;
            public int Id;
            public char[,] Fields;
            public bool HasBeenMatched = false;
            public bool InCorrectOrientation = false;

            public Tile(string input)
            {
                Fields = new char[10, 10];

                var lines = input.Split("\n");

                for (int y = 0; y < lines.Length; y++)
                {
                    if (lines[y].StartsWith("Tile"))
                    {
                        Id = int.Parse(lines[y][5..9]);
                    }
                    else
                    {
                        for (int x = 0; x < lines[y].Length; x++)
                        {
                            Fields[y - 1, x] = lines[y][x];
                        }
                    }
                }
            }

            public int NrOfHashes()
            {
                int size = (int) Math.Sqrt(Fields.Length);
                int nr = 0;

                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        if (Fields[y, x] == '#')
                        {
                            nr += 1;
                        }
                    }
                }

                return nr;
            }

            public int NrNeighbours()
            {
                var nr = 0;

                if (Left != null) nr += 1;
                if (Top != null) nr += 1;
                if (Right != null) nr += 1;
                if (Bottom != null) nr += 1;

                return nr;
            }

            // Goes through all other tiles, rotates them and tries to match sides.
            // Sets the Left, Top, Right and Bottom properties if matched.
            public void MatchAllNeighbours(List<Tile> tiles)
            {
                foreach (var tile in tiles)
                {
                    if (Id == tile.Id || this.HasBeenMatched || !tile.HasBeenMatched) continue;

                    if (this.Right == null && tile.Left == null)
                    {
                        var fits = false;

                        if (this.InCorrectOrientation)
                        {
                            fits = FitsLeftOf(this, tile);
                        }
                        else
                        {
                            fits = this.TransformAndFit(tile, FitsLeftOf);
                        }

                        if (fits)
                        {
                            this.InCorrectOrientation = true;
                            tile.Left = this;
                            this.Right = tile;
                        }
                    }

                    if (this.Bottom == null && tile.Top == null)
                    {
                        var fits = false;

                        if (this.InCorrectOrientation)
                        {
                            fits = FitsOnTopOf(this, tile);
                        }
                        else
                        {
                            fits = this.TransformAndFit(tile, FitsOnTopOf);
                        }

                        if (fits)
                        {
                            this.InCorrectOrientation = true;
                            Bottom = tile;
                            tile.Top = this;
                        }
                    }

                    if (this.Left == null && tile.Right == null)
                    {
                        var fits = false;

                        if (this.InCorrectOrientation)
                        {
                            fits = FitsRightOf(this, tile);
                        }
                        else
                        {
                            fits = this.TransformAndFit(tile, FitsRightOf);
                        }

                        if (fits)
                        {
                            this.InCorrectOrientation = true;
                            Left = tile;
                            tile.Right = this;
                        }
                    }

                    if (this.Top == null && tile.Bottom == null)
                    {
                        var fits = false;

                        if (this.InCorrectOrientation)
                        {
                            fits = FitsBelow(this, tile);
                        }
                        else
                        {
                            fits = this.TransformAndFit(tile, FitsBelow);
                        }

                        if (fits)
                        {
                            this.InCorrectOrientation = true;
                            Top = tile;
                            tile.Bottom = this;
                        }
                    }
                }

                if (this.InCorrectOrientation)
                {
                    this.HasBeenMatched = true;
                }
            }

            private bool TransformAndFit(Tile tile, Func<Tile, Tile, bool> Fits)
            {
                for (int rotations = 0; rotations < 4; rotations++)
                {
                    if (Fits(this, tile))
                    {
                        return true;
                    }

                    this.FlipVertical();

                    if (Fits(this, tile))
                    {
                        return true;
                    }

                    this.FlipVertical();
                    this.FlipHorizontal();

                    if (Fits(this, tile))
                    {
                        return true;
                    }

                    this.FlipHorizontal();
                    this.Rotate90();
                }

                return false;
            }

            private static bool FitsLeftOf(Tile one, Tile other)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (one.Fields[x, 9] != other.Fields[x, 0])
                    {
                        return false;
                    }
                }

                return true;
            }

            private static bool FitsOnTopOf(Tile one, Tile other)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (one.Fields[9, y] != other.Fields[0, y])
                    {
                        return false;
                    }
                }

                return true;
            }

            private static bool FitsRightOf(Tile one, Tile other)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (one.Fields[x, 0] != other.Fields[x, 9])
                    {
                        return false;
                    }
                }

                return true;
            }

            private static bool FitsBelow(Tile one, Tile other)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (one.Fields[0, y] != other.Fields[9, y])
                    {
                        return false;
                    }
                }

                return true;
            }

            public void Rotate90()
            {
                int size = (int) Math.Sqrt(Fields.Length);
                var newFields = new char[size, size];

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        newFields[x, y] = Fields[y, size - 1 - x];
                    }
                }

                Fields = newFields;
            }

            public void FlipVertical()
            {
                int size = (int) Math.Sqrt(Fields.Length);
                var newFields = new char[size, size];

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        newFields[x, y] = Fields[size - 1 - x, y];
                    }
                }

                Fields = newFields;
            }

            public void FlipHorizontal()
            {
                int size = (int) Math.Sqrt(Fields.Length);
                var newFields = new char[size, size];

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        newFields[x, y] = Fields[x, size - 1 - y];
                    }
                }

                Fields = newFields;
            }

            public IEnumerable<(int, int)> MonsterLocations(List<string> monster)
            {
                var size = Math.Sqrt(Fields.Length);

                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        if (MonsterAtLocation(x, y, monster))
                        {
                            yield return (x, y);
                        }
                    }
                }
            }

            private bool MonsterAtLocation(int x, int y, List<string> monster)
            {
                var size = Math.Sqrt(Fields.Length);

                for (int offset = 0; offset < monster[0].Length; offset++)
                {
                    // vergelijk 3 strings tegelijkertijd onder elkaar
                    if ((x + offset >= size) || y + 2 >= size)
                    {
                        return false;
                    }

                    // We need to match with its a #
                    if (monster[0][offset] == '#') //  "                  # ";
                    {
                        if (Fields[y, x + offset] != '#')
                        {
                            return false;
                        }
                    }

                    if (monster[1][offset] == '#') //  "#    ##    ##    ###";
                    {
                        if (Fields[y + 1, x + offset] != '#')
                        {
                            return false;
                        }
                    }

                    if (monster[2][offset] == '#') // " #  #  #  #  #  #   "  ;
                    {
                        if (Fields[y + 2, x + offset] != '#')
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        // What do you get if you multiply together the IDs of the four corner tiles?
        static void Part1()
        {
            var tiles = ReadTiles("input/20");
            tiles[0].HasBeenMatched = true;

            while (tiles.Where(tile => !tile.HasBeenMatched).Count() > 0)
            {
                foreach (var tile in tiles)
                {
                    tile.MatchAllNeighbours(tiles);
                }
            }


            var res = tiles.Where(tile => tile.NrNeighbours() == 2).Aggregate(1L, (sum, tile) => sum * tile.Id);
            Console.WriteLine($"Day 20: Tile corner product {res}."); // 11788777383197
        }

        static void DebugPrint(List<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                if (tile.NrNeighbours() < 1) continue;
                Console.WriteLine($"Tile {tile.Id} has {tile.NrNeighbours()} neighbours.");
                if (tile.Left != null) Console.WriteLine($"  Left {tile.Left.Id}");
                if (tile.Top != null) Console.WriteLine($"  Top {tile.Top.Id}");
                if (tile.Right != null) Console.WriteLine($"  Right {tile.Right.Id}");
                if (tile.Bottom != null) Console.WriteLine($"  Bottom {tile.Bottom.Id}");
            }

            Console.WriteLine();
        }

        // How many # are not part of a sea monster?
        static void Part2()
        {
            var tiles = ReadTiles("input/20");
            tiles[0].HasBeenMatched = true;

            while (tiles.Where(tile => !tile.HasBeenMatched).Count() > 0)
            {
                foreach (var tile in tiles)
                {
                    tile.MatchAllNeighbours(tiles);
                }
            }

            var sea = TilesToSea(tiles);

            var monster = new List<string>()
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "
            };

            int nrMonsters = 0;

            for (int rotations = 0; rotations < 4; rotations++)
            {
                foreach (var (x, y) in sea.MonsterLocations(monster))
                {
                    nrMonsters += 1;
                }

                if (nrMonsters > 0)
                {
                    break;
                }

                sea.FlipVertical();

                foreach (var (x, y) in sea.MonsterLocations(monster))
                {
                    nrMonsters += 1;
                }

                if (nrMonsters > 0)
                {
                    break;
                }

                sea.FlipVertical();
                sea.FlipHorizontal();

                foreach (var (x, y) in sea.MonsterLocations(monster))
                {
                    nrMonsters += 1;
                }

                if (nrMonsters > 0)
                {
                    break;
                }

                sea.FlipHorizontal();
                sea.Rotate90(); Console.WriteLine("Rotate");
            }

            var hashesInMonster = monster.Aggregate(0, (sum, line) => sum + line.Where(c => c == '#').Count());
            Console.WriteLine($"Monsters found {nrMonsters}.");
            var res = sea.NrOfHashes() - (nrMonsters * hashesInMonster);
            Console.WriteLine($"Day 20: Nr # not in monster {res}."); // 2242
        }

        static Tile TilesToSea(List<Tile> tiles)
        {
            Tile topLeft = new Tile("");

            foreach (var tile in tiles)
            {
                if (tile.Left == null && tile.Top == null)
                {
                    topLeft = tile;
                    Console.WriteLine($"Top left is {topLeft.Id}");
                    break;
                }
            }

            var strings = new List<string>();

            var firstOfRow = topLeft;

            var size = Math.Sqrt(tiles.Count());

            for (int i = 0; i < size; i++)
            {
                for (int x = 1; x < 9; x++)
                {
                    var line = "";
                    var tile = firstOfRow;

                    for (int j = 0; j < size; j++)
                    {
                        for (int y = 1; y < 9; y++)
                        {
                            line += tile.Fields[x, y];
                        }
                        tile = tile.Right;
                    }

                    strings.Add(line);
                }
                firstOfRow = firstOfRow.Bottom;
            }

            // This is clumsy.
            var res = new Tile("")
            {
                Fields = new char[strings.Count(), strings.Count()]
            };

            for (int y = 0; y < strings.Count(); y++)
            {
                for (int x = 0; x < strings.Count(); x++)
                {
                    res.Fields[y, x] = strings[x][y];
                }
            }

            return res;
        }

        static List<Tile> ReadTiles(string file)
        {
            var tiles = new List<Tile>();

            foreach (var block in File.ReadAllText(file).Split("\n\n"))
            {
                tiles.Add(new Tile(block));
            }

            return tiles;
        }
    }
}
