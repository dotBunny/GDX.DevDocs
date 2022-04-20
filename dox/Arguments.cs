// Copyright (c) 2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Dox.Utils;

namespace Dox
{
    public class Arguments
    {
        const string k_ArgumentPrefix = "--";
        public const string HelpKey = "help";

        public const string InputKey = "input";
        public const string OutputKey = "output";

        public const string StepsKey = "steps";
        public const string DefinesKey = "defines";

        public const string SetTeamCityKey = "set-teamcity";
        public const string SetUserEnvironmentKey = "set-user-env";
        public const string PingHostKey = "ping-host";

        readonly string[] m_RawArguments;

        readonly Dictionary<string, Dictionary<string, string>> m_RegisteredHelp = new();

        /// <summary>
        ///     A list of arguments post processing.
        /// </summary>
        readonly string[] m_CleanedArguments;

        readonly int m_CleanedArgumentsLength;

        public Arguments(string[] args)
        {
            List<string> processedArguments = new(args.Length);
            List<string> rawArguments = new(args.Length);
            foreach (string s in args)
            {
                if (string.IsNullOrEmpty(s))
                {
                    continue;
                }

                string p = s.Trim().ToLower();
                if (p.StartsWith(k_ArgumentPrefix))
                {
                    p = p.Substring(k_ArgumentPrefix.Length);
                }

                processedArguments.Add(p);
                rawArguments.Add(s);
            }

            m_CleanedArguments = processedArguments.ToArray();
            m_CleanedArgumentsLength = m_CleanedArguments.Length;
            m_RawArguments = rawArguments.ToArray();

            StringBuilder argumentChain = new();
            foreach (string s in m_RawArguments)
            {
                argumentChain.Append($"{s} ");
            }

            Output.LogLine($"Using arguments {argumentChain.ToString().Trim()}");
        }

        public void RegisterHelp(string section, string arg, string message)
        {
            // Check for section
            if (!m_RegisteredHelp.ContainsKey(section))
            {
                m_RegisteredHelp.Add(section, new Dictionary<string, string>());
            }

            // Add messaging
            if (m_RegisteredHelp[section].ContainsKey(arg))
            {
                m_RegisteredHelp[section][arg] = message;
            }
            else
            {
                m_RegisteredHelp[section].Add(arg, message);
            }
        }

        public bool Has(string argument)
        {
            for (int i = 0; i < m_CleanedArgumentsLength; i++)
            {
                if (m_CleanedArguments[i] == argument)
                {
                    return true;
                }
            }

            return false;
        }

        public void Help()
        {
            Output.NextLine();
            Output.LogLine(
                "Simple configuration options.",
                ConsoleColor.DarkGray);

            Output.SectionHeader("Required");

            Output.Log($"{k_ArgumentPrefix}{InputKey} ", ConsoleColor.Cyan);
            Output.Log("<", ConsoleColor.Cyan);
            Output.Log("value");
            Output.Log(">", ConsoleColor.Cyan);
            Output.Log("\t\tPath to package repository.");
            Output.NextLine();


            Output.SectionHeader("Optional");

            Output.Log($"{k_ArgumentPrefix}{OutputKey} ", ConsoleColor.Cyan);
            Output.Log("<", ConsoleColor.Cyan);
            Output.Log("value");
            Output.Log(">", ConsoleColor.Cyan);
            Output.Log("\tPath to where the docs will be generated.");
            Output.NextLine();

            Output.Log($"{k_ArgumentPrefix}{StepsKey} ", ConsoleColor.Cyan);
            Output.Log("<", ConsoleColor.Cyan);
            Output.Log("value");
            Output.Log(">", ConsoleColor.Cyan);
            Output.Log("\t\tOverride the default steps.");
            Output.NextLine();
            Output.Log($"{k_ArgumentPrefix}{DefinesKey} ", ConsoleColor.Cyan);
            Output.Log("<", ConsoleColor.Cyan);
            Output.Log("value");
            Output.Log(">", ConsoleColor.Cyan);
            Output.Log("\t\tOverride the default defines used when building.");
            Output.NextLine();

            Output.NextLine();
            Output.LogLine(
                "The following arguments are meant to provide fine-grain control of the generation steps.",
                ConsoleColor.DarkGray);

            foreach (KeyValuePair<string, Dictionary<string, string>> section in m_RegisteredHelp)
            {
                Output.SectionHeader(section.Key);
                foreach (KeyValuePair<string, string> item in section.Value)
                {
                    if (item.Key.EndsWith("<value>"))
                    {
                        Output.Log($"{k_ArgumentPrefix}{item.Key.Substring(0, item.Key.Length - 7)}", ConsoleColor.Cyan);
                        Output.Log("<", ConsoleColor.Cyan);
                        Output.Log("value");
                        Output.Log(">", ConsoleColor.Cyan);
                    }
                    else
                    {
                        Output.Log($"{k_ArgumentPrefix}{item.Key}", ConsoleColor.Cyan);
                    }

                    Output.Log(item.Value);
                    Output.NextLine();
                }
            }
        }

        public bool TryGetValue(string argument, out string value)
        {
            int argumentIndex = -1;
            for (int i = 0; i < m_CleanedArguments.Length; i++)
            {
                if (m_CleanedArguments[i] == argument)
                {
                    argumentIndex = i;
                    break;
                }
            }

            int valueIndex = argumentIndex + 1;
            if (argumentIndex != -1 && valueIndex < m_CleanedArgumentsLength)
            {
                value = m_RawArguments[valueIndex];
                return true;
            }

            value = null;
            return false;
        }
    }
}