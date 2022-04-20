// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace Dox.TableOfContents
{
    [Serializable]
    public class Entry
    {
        [DefaultValue(null)]
        public string uid;
        [DefaultValue(null)]
        public string name;

        [DefaultValue(null)]
        [YamlMember(Alias = "name.vb", ApplyNamingConventions = false)]
        public string name_vb;

        [DefaultValue(null)]
        public string customType;

        [DefaultValue(null)]
        public List<Entry> items;

        public void Cleanup()
        {
            List<Entry> Namespaces = new List<Entry>();
            List<Entry> Items = new List<Entry>();

            if (items == null) return;


            // Deduplicate items?
            int itemCount = items.Count;
            List<int> removedIndexes = new List<int>();
            for (int i = 0; i < itemCount; i++)
            {
                // Skip already removed
                if (removedIndexes.Contains(i)) continue;

                Entry baseEntry = items[i];
                for (int j = 0; j < itemCount; j++)
                {
                    // Dont bother given we've handled it
                    if (removedIndexes.Contains(j)) continue;

                    Entry tempEntry = items[j];
                    if (tempEntry.uid == baseEntry.uid && i != j)
                    {
                        baseEntry.items ??= new List<Entry>();
                        if (tempEntry.items != null)
                        {
                            baseEntry.items.AddRange(tempEntry.items);
                        }

                        removedIndexes.Add(j);
                    }
                }
            }

            // Remove items
            if (removedIndexes.Count > 0)
            {
                removedIndexes.Sort();
                for (int i = removedIndexes.Count - 1; i >= 0; i--)
                {
                    items.RemoveAt(removedIndexes[i]);
                }
            }

            // Split into class / namespaces
            foreach (Entry item in items)
            {
                string ymlPath = Path.Combine(Steps.Files.TableOfContents.WorkingFolder, $"{item.uid.Replace('`', '-')}.yml");
                if (File.Exists(ymlPath))
                {
                    // Cleanup name
                    int lastIndexOf = item.name.LastIndexOf('.');
                    if (lastIndexOf != -1)
                    {
                        item.name = item.name.Substring(lastIndexOf + 1);
                    }
                    item.Cleanup();

                    // Figure it out
                    string[] ymlContent = File.ReadAllLines(ymlPath);
                    int lineCount = ymlContent.Length;
                    string itemType = "class";
                    for (int lineIndex = 0; lineIndex < lineCount; lineIndex++)
                    {
                        // Find first type in file
                        string line = ymlContent[lineIndex];
                        if (line.Contains("type:"))
                        {
                            itemType = line.Replace("type:", "").Trim().ToLower();
                            break;
                        }
                    }
                    item.customType = itemType;
                    switch (itemType)
                    {
                        case "namespace":
                            Namespaces.Add(item);
                            break;
                        default:
                            Items.Add(item);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"Unable to find YML for {item.uid} at {ymlPath}, this isn't always a bad thing.");
                }
            }

            // Add back ordered
            items.Clear();

            // Add namespaces first
            items.AddRange(Namespaces.OrderBy(entry => entry.name));

            // Then list of classes
            items.AddRange(Items.OrderBy(entry => entry.name));
        }
    }
}