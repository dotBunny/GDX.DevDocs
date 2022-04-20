// Copyright (c) 2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System;

namespace Dox.Utils
{
    public static class Output
    {
        public const ConsoleColor ExternalForegroundColor = ConsoleColor.DarkGray;
        public const ConsoleColor ExternalBackgroundColor = ConsoleColor.Black;

        static ConsoleColor s_StashedForegroundColor;
        static ConsoleColor s_StashedBackgroundColor;


        public static void Error(string message, int errorCode, bool isFatal = false)
        {
            StashState();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($"ERROR {errorCode}: {message}");
            RestoreState();
            if (isFatal)
            {
                Environment.Exit(errorCode);
            }
        }

        public static void Value(string key, string value)
        {
            StashState();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(key);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("=");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(value);
            Console.WriteLine();
            RestoreState();
        }

        public static void Log(string message, ConsoleColor foregroundColor = ConsoleColor.Gray,
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            StashState();
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(message);
            RestoreState();
        }

        public static void LogLine(string message,
            ConsoleColor foregroundColor = ConsoleColor.Gray,
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            StashState();
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(message);
            RestoreState();
        }

        public static void NextLine()
        {
            Console.WriteLine();
        }

        public static void SectionHeader(string message)
        {
            StashState();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("]\n");
            RestoreState();
        }


        public static void Warning(string message)
        {
            StashState();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($"WARNING: {message}");
            RestoreState();
        }

        static void RestoreState()
        {
            Console.BackgroundColor = s_StashedBackgroundColor;
            Console.ForegroundColor = s_StashedForegroundColor;
        }

        static void StashState()
        {
            s_StashedBackgroundColor = Console.BackgroundColor;
            s_StashedForegroundColor = Console.ForegroundColor;
        }
    }
}