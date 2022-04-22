// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System;
using Dox.Utils;

namespace Dox;

public static class Generate
{
    public const string Argument = "generate";

    public static void Process()
    {
        Output.LogLine("Generating ...");

        // Build out ordered steps
        Program.GetParameter(Arguments.StepsKey, Config.Steps, out Config.Steps);
        string[] stepSplit = Config.Steps.Split(',', StringSplitOptions.TrimEntries);
        foreach (string targetStep in stepSplit)
        {
            string targetStepLower = targetStep.ToLower();
            if(Program.RegisteredSteps.ContainsKey(targetStepLower))
            {
                Program.OrderedSteps.Add(Program.RegisteredSteps[targetStepLower]);
            }
            else
            {
                Output.Log($"Unable to find '{targetStepLower}' step.", ConsoleColor.Yellow);
            }
        }
        if (Program.OrderedSteps.Count == 0)
        {
            Output.Error("No steps defined.", -1, true);
        }

        // Process steps
        foreach (IStep step in Program.OrderedSteps)
        {
            Output.SectionHeader(step.GetHeader());
            step.Process();
        }
    }
}