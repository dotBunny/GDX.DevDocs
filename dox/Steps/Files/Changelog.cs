// Copyright (c) 2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Dox.Utils;

namespace Dox.Steps.Files
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Changelog : IStep
    {
        public const string Key = "files-changelog";
        public void Clean()
        {
            string path = GetPath();
            if (File.Exists(path))
            {
                Output.LogLine($"Cleaning up previous Changelog.");
                File.Delete(path);
            }
        }
        static string GetPath()
        {
            return Path.Combine(Config.InputDirectory, ".docfx", "changelog.md");
        }

        /// <inheritdoc />
        public string GetIdentifier()
        {
            return Key;
        }

        /// <inheritdoc />
        public string GetHeader()
        {
            return "Create Changelog";
        }

        /// <inheritdoc />
        public void Process()
        {
            string path = GetPath();
            string contentPath = Path.Combine(Config.InputDirectory, "CHANGELOG.md");
            if (!File.Exists(contentPath))
            {
                Output.Error("Unable to find actual Changelog.", -1, true);
            }

            Output.LogLine($"Reading existing Changelog from {contentPath}.");

            TextGenerator generator = new();
            generator.AppendLine("---");
            generator.AppendLine("_disableBreadcrumb: true");
            generator.AppendLine("_disableContribution: true");
            generator.AppendLine("---");
            generator.AppendLineRange(File.ReadAllLines(contentPath));

            Output.LogLine($"Writing updated Changelog to {path}.");
            File.WriteAllText(path, generator.ToString());
        }
    }
}