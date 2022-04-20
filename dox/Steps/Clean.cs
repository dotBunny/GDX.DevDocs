// Copyright (c) 2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using Dox.Steps.Files;
using Dox.Utils;

namespace Dox.Steps
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Clean : IStep
    {

        public const string Key = "clean";

        /// <inheritdoc />
        void IStep.Clean()
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
            return "Clean Previous";
        }

        /// <inheritdoc />
        public void Process()
        {
            string objFolder = Path.Combine(Config.InputDirectory, ".docfx", "obj");
            if (Directory.Exists(objFolder))
            {
                Output.LogLine("Deleting previous intermediate folder ...");
                Directory.Delete(objFolder, true);
            }

            // Cleanup file generations
            foreach (KeyValuePair<string, IStep> kvp in Program.RegisteredSteps)
            {
                kvp.Value.Clean();
            }
        }
    }
}