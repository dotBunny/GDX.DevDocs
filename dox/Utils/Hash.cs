// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Dox.Utils
{
    public static class Hash
    {
        public static string GetHash(string rootFolder, IEnumerable<string> relativeFilePath)
        {
            if (relativeFilePath == null) return null;
            List<string> files = (from p in relativeFilePath select Path.Combine(rootFolder, p)).ToList();

            int fileCount = files.Count;
            for (int i = files.Count - 1; i >= 0 ; i--)
            {
                string path = files[i];
                if (!File.Exists(path))
                {
                    Output.LogLine($"[Not Found] {path}.");
                    files.RemoveAt(i);
                }
            }

            using FileCollectionStream reader = new FileCollectionStream(files);
            return GetSha256HashString(reader);
        }
        public static byte[] GetSha256Hash(Stream stream)
        {
            using SHA256 sha256 = SHA256.Create();
            return sha256.ComputeHash(stream);
        }

        public static byte[] GetSha256Hash(string content)
        {
            using SHA256 sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
        }

        public static string GetSha256HashString(string content)
            => Convert.ToBase64String(GetSha256Hash(content));

        public static string GetSha256HashString(Stream stream)
            => Convert.ToBase64String(GetSha256Hash(stream));

    }
}