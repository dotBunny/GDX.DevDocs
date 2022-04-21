// Copyright (c) 2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.IO.Compression;
using Dox.Utils;

namespace Dox.Steps.Unpack
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DocFx : StepBase
    {
        public const string Key = "unpack-docfx";
        const string k_PackageName = "docfx-2.59.2.zip";
        const string k_InstallPathKey = "docfx-path";
        public static string InstallPath;

        public DocFx()
        {
            Program.Args.RegisterHelp("DocFX Install", $"{k_InstallPathKey} <value>",
                "\t\tPath to existing DocFX install.");
        }

        /// <inheritdoc />
        public override string GetIdentifier()
        {
            return Key;
        }

        /// <inheritdoc />
        public override string GetHeader()
        {
            return "Unpack DocFX";
        }

        /// <inheritdoc />
        public override void Process()
        {
            Program.GetParameter(k_InstallPathKey,
                Path.Combine(Program.ProcessDirectory, "docfx"), out InstallPath,
                s =>
                {
                    if (Path.IsPathFullyQualified(s))
                    {
                        return s;
                    }

                    return Path.GetFullPath(Path.Combine(Program.ProcessDirectory, s));
                }
            );

            if (Directory.Exists(InstallPath) && File.Exists(Path.Combine(InstallPath, "docfx.exe")))
            {
                Output.LogLine("Found existing DocFX.");
                return;
            }


            string zipPath = Path.Combine(Program.ProcessDirectory, Config.PackageFolder, k_PackageName);
            // Check package existence
            if (!File.Exists(zipPath))
            {
                Output.Error("Unable to find packaged DocFX.", -1, true);
                return;
            }

            Output.LogLine($"Extracting {zipPath}");
            ZipFile.ExtractToDirectory(zipPath, InstallPath);
        }
    }
}