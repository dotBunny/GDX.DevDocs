// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Runtime.InteropServices;
using Dox.Utils;

namespace Dox.Steps.TeamCity
{
    public class CodeInspection : StepBase
    {
        //Staging\ResharperInspection.xml
        public const string Key = "code-inspection";

        static string GetPath()
        {
            return Path.Combine(Config.InputDirectory, ".docfx", "reports", "inspection");
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
            return "Code Inspection";
        }

        /// <inheritdoc />
        public override void Process()
        {
            if (!Program.IsTeamCityAgent)
            {
                Output.LogLine("Skipping. Not running inside of CI/CD.");
                return;
            }

            string teamCityArtifact =
                Path.Combine(Program.ProcessDirectory, "..", "Staging", "ResharperInspection.xml");
            if (File.Exists(teamCityArtifact))
            {
                Output.Log("Copying code inspection artifacts.");
                if (!Directory.Exists(GetPath()))
                {
                    Directory.CreateDirectory(GetPath());
                }
                File.Copy(teamCityArtifact, Path.Combine(GetPath(), "inspection.xml"));
            }
            else
            {
                Output.Error("Unable to find code inspection artifacts.", -1, true);
            }
        }
    }
}