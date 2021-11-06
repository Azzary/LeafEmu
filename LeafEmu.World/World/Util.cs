using System;
using System.Collections.Generic;
using System.Threading;

namespace LeafEmu.World
{
    class Util
    {

        public static Random rng = new Random();

        public static void PrintLogoLeafColor()
        {
            for (int i = 0; i < 15; i++)
            {
                Console.Clear();
                PrintLeafEmu((ConsoleColor)i);
                Thread.Sleep(80);
            }
            Console.Clear();
            PrintLeafEmu(ConsoleColor.Magenta);
            Logger.Logger.color = ConsoleColor.White;
        }

        public static void PrintLeafEmu(ConsoleColor color = ConsoleColor.Magenta)
        {
            Logger.Logger.color = color;
            Logger.Logger.Log(@" __                             ______         ________             ");
            Logger.Logger.Log(@"/  |                           /      \       /        |                        ");
            Logger.Logger.Log(@"$$ |        ______    ______  /$$$$$$  |      $$$$$$$$/  _____  ____   __    __ ");
            Logger.Logger.Log(@"$$ |       /      \  /      \ $$ |_ $$/       $$ |__    /     \/    \ /  |  /  |");
            Logger.Logger.Log(@"$$ |      /$$$$$$  | $$$$$$  |$$   |          $$    |   $$$$$$ $$$$  |$$ |  $$ |");
            Logger.Logger.Log(@"$$ |      $$    $$ | /    $$ |$$$$/           $$$$$/    $$ | $$ | $$ |$$ |  $$ |");
            Logger.Logger.Log(@"$$ |_____ $$$$$$$$/ /$$$$$$$ |$$ |            $$ |_____ $$ | $$ | $$ |$$ \__$$ |");
            Logger.Logger.Log(@"$$       |$$       |$$    $$ |$$ |            $$       |$$ | $$ | $$ |$$    $$/ ");
            Logger.Logger.Log(@"$$$$$$$$/  $$$$$$$/  $$$$$$$/ $$/             $$$$$$$$/ $$/  $$/  $$/  $$$$$$/  ");
            Logger.Logger.Log($"Rev {World.WorldConfig.Version} Azzary\0");
            Logger.Logger.Log(string.Empty);
        }
        public static void RemoveFirst<T1, T2>(LinkedList<T1> list, Predicate<T1> predicate)
        {
            var node = list.First;
            while (node != null)
            {
                if (predicate(node.Value))
                {
                    list.Remove(node);
                    return;
                }
                node = node.Next;
            }
        }

        public static int GetPriceOfZaap(Game.Map.Map startMap, Game.Map.Map targetMap)
        {
             return (int)Util.GetDistance2Points(startMap.X, startMap.Y, targetMap.X, targetMap.Y) * 10;
        }

        public static float GetDistance2Points(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow((x2 - x1),2) + Math.Pow((y2 - y1), 2));
        }

        public static int getIntByHashedValue(char c)
        {
            for (int a = 0; a < HASH.Count; a++)
                if (HASH[a] == c)
                    return a;
            return -1;
        }
        public static long GetUnixTime => ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
  

        public static int getStarAlea()
        {
            int rand = rng.Next(1, 100);
            if (rand > 54 && rand < 90)
            {
                int i = rng.Next(1, 15);
                switch (i)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        return 15;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        return 30;
                    case 10:
                    case 11:
                    case 12:
                        return 45;
                    case 13:
                    case 14:
                        return 60;
                    case 15:
                        return 75;
                }
            }
            if (rand >= 90)
            {
                int i = rng.Next(1, 15);
                switch (i)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        return 90;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        return 105;
                    case 10:
                    case 11:
                    case 12:
                        return 120;
                    case 13:
                    case 14:
                        return 135;
                    case 15:
                        return 150;
                }
            }
            return 0;
        }

        public static void Turn(int i, int max)
        {
            Console.Write(i + "/" + max);
            Console.SetCursorPosition(Console.CursorLeft - (i.ToString() + "/" + max).Length, Console.CursorTop);
        }

        public static float[] normalized(float[] pos)
        {
            float distance = (float)Math.Sqrt(Math.Abs(pos[0]) + Math.Abs(pos[1]));
            return new float[] { pos[0] / distance, pos[1] / distance };
        }

        public static List<char> HASH = new List<char>() {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
                't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'};

        public static string GetDirChar(int dirNum)
        {
            var hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

            if (dirNum >= hash.Length)
                return string.Empty;

            return hash[dirNum].ToString();
        }
        public static int GetDirNum(string dirChar)
        {
            var hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            return hash.IndexOf(dirChar);
        }

        public static int CharToCell(string cellCode)
        {
            char char1 = cellCode.ToCharArray()[0];
            char char2 = cellCode.ToCharArray()[1];
            short code1 = 0;
            short code2 = 0;
            short a = 0;
            while (a < HASH.Count)
            {
                if (HASH[a] == char1)
                {
                    code1 = (short)(a * 64);
                }
                if (HASH[a] == char2)
                {
                    code2 = a;
                }
                a = (short)(a + 1);
            }
            return (short)(code1 + code2);
        }

        public static string CellToChar(int cellId)
        {
            int char1 = cellId / 64;
            int char2 = cellId % 64;
            return HASH[char1] + string.Empty + HASH[char2];

        }

    }
}
