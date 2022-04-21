// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dox.TableOfContents;
using Dox.Utils;
using YamlDotNet.Serialization;

namespace Dox.Steps.Files
{
    public class TableOfContents : StepBase
    {
        public const string Key = "files-toc";
        public static string WorkingFolder = null;
        string GetPath()
        {
            return Path.Combine(Config.InputDirectory, ".docfx", "api", "toc.yml");
        }
        /// <inheritdoc />
        public override void Clean()
        {
            string path = GetPath();
            if (File.Exists(path))
            {
                Output.LogLine($"Cleaning up previous Table of Contents.");
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
            return "API: Table of Contents";
        }

        public override string[] GetRequiredStepIdentifiers()
        {
            return new [] { Metadata.Key };
        }

        /// <inheritdoc />
        public override void Process()
        {
            string path = GetPath();
            if (!File.Exists(path))
            {
                Output.Error($"Unable to find API Table of Contents yaml at {path}.", -1, true);
            }

            WorkingFolder = Directory.GetParent(path).FullName;
            string content = File.ReadAllText(path);

            // Create deserializer
            IDeserializer deserializer = new DeserializerBuilder().Build();

            // Load Content
            List<Entry> originalContent = deserializer.Deserialize<List<Entry>>(content);

            // Create our new namespace mapping
            Dictionary<string, Entry> namespaces = new Dictionary<string, Entry>();

            // Create our roots
            Entry gdxRoot = new Entry
            {
                name = "GDX", items = new List<Entry>()
            };
            namespaces.Add(gdxRoot.name, gdxRoot);

            Entry gdxEditorRoot = new Entry
            {
                name = "GDX.Editor", items = new List<Entry>()
            };
            namespaces.Add(gdxEditorRoot.name, gdxEditorRoot);

            int outerCount = originalContent.Count;
            for (int outerIndex = 0; outerIndex < outerCount; outerIndex++)
            {
                int innerCount = originalContent[outerIndex].items.Count;
                for (int innerIndex = 0; innerIndex < innerCount; innerIndex++)
                {
                    // Get iteration item
                    Entry item = originalContent[outerIndex].items[innerIndex];

                    // Figure out assembly
                    Entry workingRoot = item.uid.StartsWith("GDX.Editor.") ? gdxEditorRoot : gdxRoot;

                    // Determine our namespace
                    int lastIndex = item.uid.LastIndexOf('.');
                    string itemNamespace = item.uid.Substring(0, lastIndex);
                    bool hasNamespace = lastIndex != -1;

                    if (hasNamespace)
                    {
                        if (namespaces.ContainsKey(itemNamespace))
                        {
                            // We have a parent and its a simple add
                            Entry parent = namespaces[itemNamespace];
                            parent.items ??= new List<Entry>();
                            parent.items.Insert(0, item);
                        }
                        else
                        {
                            StringBuilder rebuildPath = new StringBuilder();

                            // We need to build the namespace out reversed
                            string[] parts = itemNamespace.Split('.');
                            int partCount = parts.Length;
                            Entry tempParent = workingRoot;
                            Entry firstParent = null;

                            for (int partIndex = 0; partIndex < partCount; partIndex++)
                            {
                                if (partIndex > 0)
                                {
                                    rebuildPath.Append('.');
                                }

                                rebuildPath.Append(parts[partIndex]);


                                string tempNamespace = rebuildPath.ToString();
                                if (namespaces.ContainsKey(tempNamespace))
                                {
                                    tempParent = namespaces[tempNamespace];
                                }
                                else
                                {
                                    // The name will be cleaned up later during TableOfContentsEntry.Cleanup
                                    Entry newParent = new Entry
                                    {
                                        name = tempNamespace, uid = tempNamespace
                                    };
                                    namespaces.Add(tempNamespace, newParent);

                                    if (tempParent != null)
                                    {
                                        tempParent.items ??= new List<Entry>();
                                        tempParent.items.Add(newParent);
                                    }

                                    tempParent = newParent;
                                    firstParent ??= newParent;
                                }
                            }

                            if (tempParent != null)
                            {
                                tempParent.items ??= new List<Entry>();
                                tempParent.items.Add(item);
                            }
                        }
                    }
                    else
                    {
                        // Failsafe
                        workingRoot.items.Add(item);
                    }
                }
            }


            // Sort root
            gdxRoot.Cleanup();
            gdxEditorRoot.Cleanup();

            // Output Content
            List<Entry> outputList = new List<Entry>(1);
            outputList.Add(gdxRoot);
            outputList.Add(gdxEditorRoot);

            ISerializer serializer = new SerializerBuilder()
                .WithEmissionPhaseObjectGraphVisitor(args =>
                    new GraphVisitor
                        (args.InnerVisitor, args.TypeConverters, args.NestedObjectSerializer))
                .Build();
            string yaml = "### YamlMime:TableOfContent" + Environment.NewLine + serializer.Serialize(outputList);
            File.WriteAllText(path, yaml);


            // Need to update the hash of the global cache
            string relativePath = Path.GetDirectoryName(path); // to API folder
            string objPath = Path.Combine(relativePath, "..", "..", "..", "..", "obj", "xdoc", "cache", "final");

            string[] cacheFiles = Directory.GetFiles(objPath);
            foreach (string cacheFile in cacheFiles)
            {
                Dictionary<string, BuildInfo> cache = Json.Deserialize<Dictionary<string, BuildInfo>>(cacheFile);
                foreach (KeyValuePair<string, BuildInfo> cacheItem in cache)
                {
                    cacheItem.Value.CheckSum = Hash.GetHash(relativePath, cacheItem.Value.RelativeOutputFiles);
                    Console.WriteLine($"[CHECKSUM] {relativePath} {cacheItem.Value.CheckSum}");
                }

                Console.WriteLine($"Saving cache file to {cacheFile}");
                File.WriteAllText(cacheFile, Json.Serialize(cache));
            }
        }
    }
}