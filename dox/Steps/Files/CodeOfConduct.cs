// Copyright (c) 2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Dox.Utils;

namespace Dox.Steps.Files
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CodeOfConduct : StepBase
    {
        public const string Key = "files-conduct";
        static string GetPath()
        {
            return Path.Combine(Config.InputDirectory, ".docfx", "manual", "contributing", "code-of-conduct.md");
        }

        public override void Clean()
        {
            string path = GetPath();
            if (File.Exists(path))
            {
                Output.LogLine($"Cleaning up previous Code of Conduct.");
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
            return "Code Of Conduct";
        }

        /// <inheritdoc />
        public override void Process()
        {
            string path = GetPath();
            string contentPath = Path.Combine(Config.InputDirectory, "CODE_OF_CONDUCT.md");
            if (!File.Exists(contentPath))
            {
                Output.Error("Unable to find actual Code of Conduct.", -1, true);
            }

            Output.LogLine($"Reading existing Code of Conduct from {contentPath}.");

            TextGenerator generator = new();
            generator.AppendLine("---");
            generator.AppendLine("_disableContribution: true");
            generator.AppendLine("---");
            generator.AppendLineRange(File.ReadAllLines(contentPath));

            Output.LogLine($"Writing updated Code of Conduct to {path}.");
            File.WriteAllText(path, generator.ToString());
        }
    }
}