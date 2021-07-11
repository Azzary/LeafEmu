using System;

namespace LeafEmu.Logger
{
    public static class Logger
    {
        public static ConsoleColor color = ConsoleColor.White;
        public static byte lvlOfWarning = 9;//https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line
        public static void Log(object text, int PadLeft = 0)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(new String(' ', PadLeft) + text.ToString());
            Console.ResetColor();
        }

        public static void Debug(string text, int PadLeft = 0)
        {
            if (lvlOfWarning >= 1)
            {
                color = ConsoleColor.White;
                Log(text, PadLeft);
            }
        }

        public static void Warning(string text, int PadLeft = 0)
        {
            if (lvlOfWarning >= 3)
            {
                color = ConsoleColor.Yellow;
                Log(text, PadLeft);
                color = ConsoleColor.White;
            }
        }

        public static void Error(string text, int PadLeft = 0)
        {
            if (lvlOfWarning >= 5)
            {
                color = ConsoleColor.Red;
                Log(text, PadLeft);
                color = ConsoleColor.White;
            }
        }

    }
}

