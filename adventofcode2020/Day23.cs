using System;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode
{
    class Day23
    {
        class Game
        {
            private LinkedList<int> _list;
            private Dictionary<int, LinkedListNode<int>> _dict;
            private List<int> _sorted;
            private int _rounds;

            public Game(int[] input, int rounds, bool v2 = false)
            {
                _rounds = rounds;
                _list = new LinkedList<int>(input);

                if (v2)
                {
                    for (int i = 10; i <= 1000000; i++)
                    {
                        _list.AddLast(i);
                    }
                }

                _dict = new Dictionary<int, LinkedListNode<int>>();

                var cur = _list.First;
                while (cur != null)
                {
                    _dict[cur.Value] = cur;
                    cur = cur.Next;
                }

                _sorted = _dict.Keys.OrderBy(a => a).ToList();
            }

            public void Play()
            {
                var cur = _list.First;

                for (int i = 0; i < _rounds; i++)
                {
                    var one = cur.Next ?? cur.List.First;
                    var two = one.Next ?? one.List.First;
                    var thr = two.Next ?? two.List.First;
                    _list.Remove(one);
                    _list.Remove(two);
                    _list.Remove(thr);
                    var dest = DestinationCup(cur, one.Value, two.Value, thr.Value);
                    _list.AddAfter(dest, one);
                    _list.AddAfter(one, two);
                    _list.AddAfter(two, thr);
                    cur = cur.Next ?? cur.List.First;
                }
            }

            LinkedListNode<int> DestinationCup(LinkedListNode<int> cur, int one, int two, int thr)
            {
                var destValue = cur.Value - 1;

                while (true)
                {
                    if (destValue < _sorted.First())
                    {
                        destValue = _sorted.Last();
                    }

                    if (destValue != one && destValue != two && destValue != thr)
                    {
                        return _dict[destValue];
                    }

                    destValue -= 1;
                }
            }
            public void Answer1()
            {
                var node = _dict[1].Next;

                Console.Write("All labels: ");

                while (node.Value != 1)
                {
                    Console.Write(node.Value);
                    node = node.Next ?? node.List.First;
                }

                Console.WriteLine("");
            }

            public void Answer2()
            {
                var one = _dict[1].Next ?? _dict[1].List.First;
                var two = one.Next ?? one.List.First;

                Console.WriteLine($"Following two labels: {one.Value} * {two.Value} = {(long)one.Value * (long)two.Value}");
            }
        }

        public static void Start()
        {
            Part1();
            Part2();
        }

        // What are the labels on the cups after cup 1?
        static void Part1()
        {
            var game = new Game(new int[] { 8, 5, 3, 1, 9, 2, 6, 4 , 7 }, 100);
            game.Play();
            game.Answer1(); // 97624853
        }

        // What do you get if you multiply their labels together?
        static void Part2()
        {
            var game = new Game(new int[] { 8, 5, 3, 1, 9, 2, 6, 4 , 7 }, 10000000, v2: true);
            game.Play();
            game.Answer2(); // 664642452305
        }
    }
}
