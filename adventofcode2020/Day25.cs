using System;

namespace adventofcode
{
    class Day25
    {
        public static void Start()
        {
            long pubkeyCard = 17115212;
            long pubkeyDoor = 3667832;

            long subject = 7;

            var res = 1L;
            long loopSizeCard = 0;

            while (res != pubkeyCard)
            {
                res *= subject;
                res = res % 20201227;
                loopSizeCard += 1;
            }

            Console.WriteLine($"Loop size card is {loopSizeCard}");

            // res = 1L;
            // long loopSizeDoor = 0;

            // while (res != pubkeyDoor)
            // {
            //     res *= subject;
            //     res = res % 20201227;
            //     loopSizeDoor += 1;
            // }

            // Console.WriteLine($"Loop size door is {loopSizeDoor}");

            subject = pubkeyDoor;
            res = 1L;
            for (int i = 0; i < loopSizeCard; i++)
            {
                res *= subject;
                res = res % 20201227;
            }

            Console.WriteLine($"Encryption key is {res}");
        }
    }
}
