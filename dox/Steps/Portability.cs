// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Runtime.CompilerServices;
using Dox.Utils;

namespace Dox.Steps
{
    public class Portability : IStep
    {
        public const string Key = "portability";

        public string[] GetRequiredStepIdentifiers()
        {
            // Generates project
            return new []{ MsBuild.Key};
        }

        /// <inheritdoc />
        public void Clean()
        {
            string dgml = GetDgmlPath();
            if (File.Exists(dgml))
            {
                Output.LogLine($"Cleaning up previous DGML portability report.");
                File.Delete(dgml);
            }
            string html = GetHtmlPath();
            if (File.Exists(html))
            {
                Output.LogLine($"Cleaning up previous HTML portability report.");
                File.Delete(html);
            }
            string json = GetJsonPath();
            if (File.Exists(json))
            {
                Output.LogLine($"Cleaning up previous JSON portability report.");
                File.Delete(json);
            }
        }

        /// <inheritdoc />
        public string GetIdentifier()
        {
            return Key;
        }

        /// <inheritdoc />
        public string GetHeader()
        {
            return "API Portability";
        }

        public string GetJsonPath()
        {
            return Path.Combine(Config.InputDirectory, ".docfx", "reports", "portability", "portability.json");
        }
        public string GetHtmlPath()
        {
            return Path.Combine(Config.InputDirectory, ".docfx", "reports", "portability", "index.html");
        }
        public string GetDgmlPath()
        {
            return Path.Combine(Config.InputDirectory, ".docfx", "reports","portability",  "portability.dgml");
        }


        /// <inheritdoc />
        public void Process()
        {
            string gdxLibraryPath =
                Path.GetFullPath(Path.Combine(Config.InputDirectory, "..", "..", "Library", "ScriptAssemblies", "GDX.dll"));

            if (!File.Exists(gdxLibraryPath))
            {
                Output.Error($"Unable to find GDX library at {gdxLibraryPath}", -1, true);
            }

            string gdxEditorLibraryPath =
                Path.GetFullPath(Path.Combine(Config.InputDirectory, "..", "..", "Library", "ScriptAssemblies", "GDX.Editor.dll"));

            if (!File.Exists(gdxLibraryPath))
            {
                Output.Error($"Unable to find GDX library at {gdxLibraryPath}", -1, true);
            }

            bool jsonExecute = ChildProcess.WaitFor(
                Path.Combine(Unpack.ApiPort.InstallPath, "ApiPort.exe"),
                Unpack.ApiPort.InstallPath,
                $"analyze -f {gdxLibraryPath} -f {gdxEditorLibraryPath} -o {GetJsonPath()} -r json");
            bool htmlExecute = ChildProcess.WaitFor(
                Path.Combine(Unpack.ApiPort.InstallPath, "ApiPort.exe"),
                Unpack.ApiPort.InstallPath,
                $"analyze -f {gdxLibraryPath} -f {gdxEditorLibraryPath} -o {GetHtmlPath()} -r html");
            bool dgmlExecute = ChildProcess.WaitFor(
                Path.Combine(Unpack.ApiPort.InstallPath, "ApiPort.exe"),
                Unpack.ApiPort.InstallPath,
                $"analyze -f {gdxLibraryPath} -f {gdxEditorLibraryPath} -o {GetDgmlPath()} -r dgml");

            if (!jsonExecute || !htmlExecute || !dgmlExecute)
            {
                Output.Error("ApiPort did not execute successfully.", -1, true);
            }
        }
    }
}



