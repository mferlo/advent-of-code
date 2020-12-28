using System;
using System.IO;

namespace Advent2020
{
    class Day25
    {
        static long CardPK;
        static long DoorPK;

        public static void Parse()
        {
            var data = File.ReadAllLines("25.txt");
            CardPK = long.Parse(data[0]);
            DoorPK = long.Parse(data[1]);
        }

        static int FindLoopSizeForPK(long pk)
        {
            var loopSize = 0;
            var value = 1L;

            while (pk != value)
            {
                loopSize++;
                value *= 7;
                value %= 20201227;
            }

            return loopSize;
        }

        static long CalculateEncryptionKey(long subjectNumber, int loopSize)
        {
            var value = 1L;
            for (var i = 1; i <= loopSize; i++)
            {
                value *= subjectNumber;
                value %= 20201227;
            }
            return value;
        }

        static long Part1(long cardPk, long doorPk)
        {
            var cardLoopSize = FindLoopSizeForPK(cardPk);
            var doorLoopSize = FindLoopSizeForPK(doorPk);

            var cardEncryptionKey = CalculateEncryptionKey(doorPk, cardLoopSize);
            var doorEncryptionKey = CalculateEncryptionKey(cardPk, doorLoopSize);
            
            if (cardEncryptionKey != doorEncryptionKey)
            {
                throw new Exception($"{cardEncryptionKey} != {doorEncryptionKey}");
            }

            return cardEncryptionKey;
        }

        public static void Test() => Console.WriteLine(Part1(5764801, 17807724));
        public static object Part1() => Part1(CardPK, DoorPK);
        public static object Part2() => "Hooray";
    }
}