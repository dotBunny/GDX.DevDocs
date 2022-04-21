// Copyright (c) 2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System;
using Dox.Steps;
using Dox.Steps.Files;
using Dox.Steps.Reports;
using Dox.Steps.TeamCity;
using Dox.Steps.Templates;
using Dox.Utils;

namespace Dox
{
    public static class Config
    {
        public const string DoxUri = "https://github.com/dotBunny/GDX.Documentation/";
        public const string DocFxUri = "https://dotnet.github.io/docfx/";

        public const string GitCommit = "https://github.com/dotBunny/GDX/commit/";

        public static string ShortDate = DateTime.Now.ToString("yyyy-MM-dd");

        public static readonly string[] AllSteps = new[]
        {
            Dox.Steps.Unpack.DocFx.Key, Dox.Steps.Unpack.ApiPort.Key,
            Clean.Key,

            Changelog.Key,
            SecurityPolicy.Key,
            CodeOfConduct.Key,
            License.Key,
            Footer.Key,

            XmlDocs.Key,

            Metadata.Key,
            Dox.Steps.Files.TableOfContents.Key,

            Portability.Key,
            CodeInspection.Key,
            Duplicates.Key,
            CodeCoverage.Key,

            Build.Key,
            Host.Key
        };


        public static readonly string CleanBuildSteps = AllSteps.Concatenate( ",", false);

        /// <summary>
        ///     Relative to the
        /// </summary>
        public const string PackageFolder = "Packages";

        /// <summary>
        ///     The defined hostname to use when pinging to determine an outside connection.
        /// </summary>
        public static string PingHost = "github.com";

        public static string Steps = CleanBuildSteps;

        public static string Defines =
            "GDX_LICENSED;GDX_ADDRESSABLES;GDX_PLATFORMS;GDX_VISUALSCRIPTING";

        public static string InputDirectory = "input";
        public static string OutputDirectory = "output";
    }
}