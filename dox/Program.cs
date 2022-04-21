// Copyright (c) 2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using Dox.Steps;
using Dox.Utils;

namespace Dox
{
    /// <summary>
    ///     The GDX documentation generator, called Dox for some strange reason.
    /// </summary>
    static class Program
    {
        /// <summary>
        ///     The processed arguments for the generator.
        /// </summary>
        public static Arguments Args;

        /// <summary>
        ///     Is an internet connection present and able to ping an outside host?
        /// </summary>
        public static bool IsOnline;

        /// <summary>
        ///     Is the execution occuring inside of our CI
        /// </summary>
        public static bool IsTeamCityAgent;

        /// <summary>
        ///     A cached absolute path to the process directory.
        /// </summary>
        public static readonly string ProcessDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        ///     An ordered list of the steps to be processed as defined.
        /// </summary>
        static readonly List<IStep> k_OrderedSteps = new();

        /// <summary>
        ///     An instantiated dictionary (by <see cref="IStep.GetIdentifier"/>) of possible steps.
        /// </summary>
        public static readonly Dictionary<string, IStep> RegisteredSteps = new();

        /// <summary>
        ///     A cached reference to the programs assembly.
        /// </summary>
        static Assembly s_Assembly;

        // ReSharper disable once UnusedMember.Local
        static void Main(string[] args)
        {
            // Cache reference to local assembly
            s_Assembly = typeof(Program).Assembly;

            Output.LogLine(
                $"GDX Documentation Generator | Version {s_Assembly.GetName().Version} | Copyright (c) 2022 dotBunny Inc.",
                ConsoleColor.Green);
            Output.LogLine($"Started on {DateTime.Now:F}", ConsoleColor.DarkGray);
            Output.LogLine("Initializing ...");
            Output.Value("Config.ProcessDirectory", ProcessDirectory);

            // Parse the arguments
            Args = new Arguments(args);

            // We require that an input is provided, regardless.
            if (!Args.Has(Arguments.InputKey))
            {
                Args.Help();
                Output.Error("Input Required", -1, true);
            }

            // Validate provided input directory
            bool validInput = GetParameter(Arguments.InputKey, Config.InputDirectory, out Config.InputDirectory,
                s =>
                {
                    if (Path.IsPathFullyQualified(s))
                    {
                        return s;
                    }
                    return Path.GetFullPath(Path.Combine(ProcessDirectory, s));
                }, Directory.Exists);
            if (!validInput)
            {
                Output.Error("Unable to find input folder. Please provide a valid absolute path.", -1, true);
            }

            // TODO: Check for docfx.json file and update validation


            Output.Value("Config.InputDirectory", Config.InputDirectory);

            // Output Directory
            GetParameter(Arguments.OutputKey, Build.GetDefaultOutputFolder(), out Config.OutputDirectory,
                s =>
                {
                    if (Path.IsPathFullyQualified(s))
                    {
                        return s;
                    }
                    return Path.GetFullPath(Path.Combine(ProcessDirectory, s));
                });
            Output.Value("Config.OutputDirectory", Config.OutputDirectory);

            // Defines
            GetParameter(Arguments.DefinesKey, Config.Defines, out Config.Defines);
            Output.Value("Config.Defines", Config.Defines);


            // PingHost
            GetParameter(Arguments.PingHostKey, "github.com", out Config.PingHost);
            Output.Value("Config.PingHost", Config.PingHost);

            // Check Internet Connection
            Ping ping = new();
            try
            {
                PingReply reply = ping.Send(Config.PingHost, 3000);
                if (reply != null)
                {
                    IsOnline = reply.Status == IPStatus.Success;
                }
            }
            catch (Exception e)
            {
                Output.LogLine(e.Message, ConsoleColor.Yellow);
                IsOnline = false;
            }
            finally
            {
                Output.Value("Program.IsOnline", IsOnline.ToString());
            }

            // Establish if this is a TeamCity
            IsTeamCityAgent = Environment.GetEnvironmentVariable("TEAMCITY_VERSION") != null;

            // Search the assemblies for included IStep's and create an instance of each, using the system activator.
            Type stepInterface = typeof(IStep);
            Type[] types = s_Assembly.GetTypes();
            foreach (Type t in types)
            {
                if (t == stepInterface || !stepInterface.IsAssignableFrom(t))
                {
                    continue;
                }

                // Create instance and register instance
                IStep step = (IStep)Activator.CreateInstance(t);
                if (step == null) continue;
                RegisteredSteps.Add(step.GetIdentifier().ToLower(), step);
            }

            // Check for help request
            if (Args.Has(Arguments.HelpKey))
            {
                Args.Help();
                return;
            }

            // Build out ordered steps
            GetParameter(Arguments.StepsKey, Config.Steps, out Config.Steps);
            string[] stepSplit = Config.Steps.Split(',', StringSplitOptions.TrimEntries);
            foreach (string targetStep in stepSplit)
            {
                string targetStepLower = targetStep.ToLower();
                if(RegisteredSteps.ContainsKey(targetStepLower))
                {
                    k_OrderedSteps.Add(RegisteredSteps[targetStepLower]);
                }
                else
                {
                    Output.Log($"Unable to find '{targetStepLower}' step.", ConsoleColor.Yellow);
                }
            }
            if (k_OrderedSteps.Count == 0)
            {
                Output.Error("No steps defined.", -1, true);
            }

            // Process steps
            foreach (IStep step in k_OrderedSteps)
            {
                Output.SectionHeader(step.GetHeader());
                step.Process();
            }
        }

        /// <summary>
        ///     Get the value for a given parameter from the config, overriden by the arguments, but also have a
        ///     failsafe default value.
        /// </summary>
        /// <param name="key">The argument identifier or config identifier.</param>
        /// <param name="defaultValue">A built-in default value.</param>
        /// <param name="resolvedValue">The resolved parameter value written to the provided <see cref="string"/>.</param>
        /// <param name="processFunction">A function to manipulate the value retrieved.</param>
        /// <param name="validateFunction">A function to validate the working value.</param>
        /// <returns>true/false if value was found.</returns>
        public static bool GetParameter(string key, string defaultValue, out string resolvedValue,
            Func<string, string> processFunction = null, Func<string, bool> validateFunction = null)
        {
            bool success = false;
            resolvedValue = processFunction != null ? processFunction.Invoke(defaultValue) : defaultValue;
            if (Args.TryGetValue(key, out string foundOverride))
            {
                if (processFunction != null)
                {
                    foundOverride = processFunction.Invoke(foundOverride);
                }

                if (validateFunction != null)
                {
                    if (validateFunction.Invoke(foundOverride))
                    {
                        resolvedValue = foundOverride;
                        success = true;
                    }
                }
                else
                {
                    resolvedValue = foundOverride;
                    success = true;
                }
            }

            return success;
        }

        /// <summary>
        ///     Set a variable for future reference in the running environment.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public static void SetEnvironmentVariable(string name, string value)
        {
            if (Args.Has(Arguments.SetTeamCityKey))
            {
                // Set for TeamCity
                Output.LogLine($"##teamcity[setParameter name='{name}' value='{value}']", ConsoleColor.Yellow);
            }

            // Set for user (no-perm request)
            if (Args.Has(Arguments.SetUserEnvironmentKey))
            {
                Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.User);
            }
        }
    }
}