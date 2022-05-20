using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncNet
{
    public partial class Utility
    {

        private static ILogHelper logHelper;


        public static void SetHelper(ILogHelper helper)
        {
            logHelper = helper;
        }
        public static void Log(string str, params object[] args)
        {
            if (logHelper != null)
            {
                logHelper.Log(str, args);
            }
            else
            {
                Console.WriteLine(string.Format(str, args));
            }
        }
        public static void ColorLog(LogColor color, string str, params object[] args)
        {
            if (logHelper != null)
            {
                logHelper.ColorLog(color, str, args);
            }
            else
            {
                ConsoleLog(string.Format(str, args), color);
            }
        }
        public static void LogWarn( string str, params object[] args)
        {
            if (logHelper != null)
            {
                logHelper.ColorLog(LogColor.Yellow, str, args);
            }
            else
            {
                ConsoleLog(string.Format(str, args), LogColor.Yellow);
            }
        }

        public static void LogError(string str, params object[] args)
        {
            if (logHelper != null)
            {
                logHelper.ColorLog(LogColor.Yellow, str, args);
            }
            else
            {
                ConsoleLog(string.Format(str, args), LogColor.Yellow);
            }
        }
        static void ConsoleLog(string logStr, LogColor color)
        {
            switch (color)
            {
                case LogColor.None:
                    Console.WriteLine(logStr);
                    break;
                case LogColor.Gray:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(logStr);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogColor.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(logStr);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogColor.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(logStr);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogColor.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(logStr);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogColor.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(logStr);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
        }
    }
    public enum LogColor : byte
    {
        None,
        Gray,
        Green,
        Yellow,
        Red,
        Blue,
    }
}