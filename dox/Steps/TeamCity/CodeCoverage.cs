// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Runtime.InteropServices;
using Dox.Utils;

namespace Dox.Steps.TeamCity
{
    public class CodeCoverage : StepBase
    {
        //Staging\ResharperInspection.xml
        public const string Key = "code-coverage";

        static string GetPath()
        {
            return Path.Combine(Config.InputDirectory, ".docfx", "reports", "coverage");
        }

        /// <inheritdoc />
        public override void Clean()
        {
            string folder = GetPath();
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
                Output.LogLine($"Removed previous coverage report.");
            }
        }

        public override string GetIdentifier()
        {
            return Key;
        }

        /// <inheritdoc />
        public override string GetHeader()
        {
            return "Code Coverage";
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
                Path.Combine(Program.ProcessDirectory, "..", "Staging", "CodeCoverage");

            if (Directory.Exists(teamCityArtifact))
            {
                if (!Directory.Exists(GetPath()))
                {
                    Directory.CreateDirectory(GetPath());
                }
                Platform.CopyFilesRecursively(teamCityArtifact, GetPath());
            }
            else
            {
                Output.Error("Unable to find code coverage artifacts.", -1, true);
            }
        }
    }
}