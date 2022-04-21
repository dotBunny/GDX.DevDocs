// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Dox.Utils;

namespace Dox.Steps
{
    public class Build : StepBase
    {
        public const string Key = "build";

        public static string GetDefaultOutputFolder()
        {
            return Path.Combine(Config.InputDirectory, ".docfx", "_site");
        }
        /// <inheritdoc />
        public override void Clean()
        {
            if (Directory.Exists(Config.OutputDirectory))
            {
                Output.LogLine("Deleting previous output folder ...");
                Directory.Delete(Config.OutputDirectory, true);
            }
        }

        /// <inheritdoc />
        public override string GetIdentifier()
        {
            return Key;
        }

        /// <inheritdoc />
        public override string GetHeader()
        {
            return "Build Documentation";
        }

        /// <inheritdoc />
        public override void Process()
        {
            string docfxJsonPath = Path.Combine(Config.InputDirectory, ".docfx", "docfx.json");
            if (!File.Exists(docfxJsonPath))
            {
                Output.Error($"Unable to find required docfx.json at {docfxJsonPath}.", -1, true);
            }

            bool execute = ChildProcess.WaitFor(
                Path.Combine(Unpack.DocFx.InstallPath, "docfx.exe"),
                Unpack.DocFx.InstallPath,
                $"{docfxJsonPath} --build");

            if (!execute)
            {
                Output.Error("An error occured while building the documentation.", -1, true);
            }

            // We need to move this somewhere
            if (Config.OutputDirectory != GetDefaultOutputFolder())
            {

            }
        }
    }
}