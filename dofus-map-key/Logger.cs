using System;


public static class Logger
{

    public static int PadLeft = 0;
    public static byte lvlOfWarning = 9;//https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line
    public static void Log(object text, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text.ToString().PadRight(PadLeft, ' '));
        Console.ResetColor();
    }

    public static void Debug(string text)
    {
        if (lvlOfWarning <= 1)
        {
            Log(text);
        }
    }

    public static void Warning(string text)
    {
        if (lvlOfWarning <= 3)
        {
            Log(text, ConsoleColor.Yellow);
        }
    }

    public static void Error(string text)
    {
        if (lvlOfWarning <= 5)
        {
            Log(text, ConsoleColor.Red);
        }
    }

}
