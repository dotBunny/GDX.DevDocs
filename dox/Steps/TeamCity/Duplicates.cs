// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Dox.Utils;

namespace Dox.Steps.TeamCity
{
    // ReSharper disable once UnusedType.Global
    public class Duplicates : StepBase
    {
       // ResharperDuplicates.xml
       public const string Key = "code-duplication";

       static string GetPath()
       {
           return Path.Combine(Config.InputDirectory, ".docfx", "reports", "duplicates");
       }

       /// <inheritdoc />
       public override void Clean()
       {
           string folder = GetPath();
           if (Directory.Exists(folder))
           {
               Directory.Delete(folder, true);
               Output.LogLine($"Removed previous inspection report.");
           }
       }

       public override string GetIdentifier()
       {
           return Key;
       }

       /// <inheritdoc />
       public override string GetHeader()
       {
           return "Code Duplication";
       }

       public override void Process()
       {
           if (!Program.IsTeamCityAgent)
           {
               Output.LogLine("Skipping. Not running inside of CI/CD.");
               return;
           }

           string teamCityArtifact =
               Path.Combine(Program.ProcessDirectory, "..", "Staging", "ResharperDuplicates.xml");
           if (File.Exists(teamCityArtifact))
           {
               Output.Log("Copying code duplication artifacts.");
               if (!Directory.Exists(GetPath()))
               {
                   Directory.CreateDirectory(GetPath());
               }
               File.Copy(teamCityArtifact, Path.Combine(GetPath(), "duplicates.xml"));
           }
           else
           {
               Output.Error("Unable to find code duplication artifacts.", -1, true);
           }
       }
    }
}