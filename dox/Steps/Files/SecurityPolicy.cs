// Copyright (c) 2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Dox.Utils;

namespace Dox.Steps.Files
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SecurityPolicy : StepBase
    {
        public const string Key = "files-security";
        static string GetPath()
        {
            return Path.Combine(Config.InputDirectory, ".docfx", "manual", "security.md");
        }

        public override void Clean()
        {
            string path = GetPath();
            if (File.Exists(path))
            {
                Output.LogLine($"Cleaning up previous Security Policy.");
                File.Delete(path);
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
            return "Security Policy";
        }

        /// <inheritdoc />
        public override void Process()
        {
            string path = GetPath();
            string contentPath = Path.Combine(Config.InputDirectory, "SECURITY.md");
            if (!File.Exists(contentPath))
            {
                Output.Error("Unable to find actual Security Policy.", -1, true);
            }

            Output.LogLine($"Reading existing Security Policy from {contentPath}.");

            TextGenerator generator = new();
            generator.AppendLine("---");
            generator.AppendLine("_disableContribution: true");
            generator.AppendLine("---");
            generator.AppendLineRange(File.ReadAllLines(contentPath));

            Output.LogLine($"Writing updated Security Policy to {path}.");
            File.WriteAllText(path, generator.ToString());
        }
    }
}