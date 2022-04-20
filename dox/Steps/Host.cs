// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Dox.Utils;

namespace Dox.Steps
{
    public class Host : IStep
    {
        public const string Key = "host";

        /// <inheritdoc />
        public void Clean()
        {

        }

        /// <inheritdoc />
        public string GetIdentifier()
        {
            return Key;
        }

        /// <inheritdoc />
        public string GetHeader()
        {
            return "Host";
        }

        /// <inheritdoc />
        public void Process()
        {
            string docfxJsonPath = Path.Combine(Config.InputDirectory, ".docfx", "docfx.json");
            if (!File.Exists(docfxJsonPath))
            {
                Output.Error($"Unable to find required docfx.json at {docfxJsonPath}.", -1, true);
            }

            int execute = ChildProcess.Spawn(
                Path.Combine(Unpack.DocFx.InstallPath, "docfx.exe"),
                Unpack.DocFx.InstallPath,
                $"{docfxJsonPath}  --serve");

            if (execute == -1)
            {
                Output.Error("An error occured while building the documentation.", -1, true);
            }
        }
    }
}