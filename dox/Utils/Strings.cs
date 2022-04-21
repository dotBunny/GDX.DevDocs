// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace Dox.Utils
{
    public static class Strings
    {
        /// <summary>
        ///     Concatenate an array of strings into one unified string.
        /// </summary>
        /// <param name="pieces">An array of strings</param>
        /// <param name="delimiter">An optional string which to use between <paramref name="pieces"/> when combining.</param>
        /// <param name="trailingDelimiter">Should a trailing <paramref name="delimiter"/> be appended?</param>
        /// <returns>A concatenated <see cref="string"/>.</returns>
        public static string Concatenate(this string[] pieces, string delimiter = null, bool trailingDelimiter = false)
        {
            StringBuilder builder = new StringBuilder();

            int count = pieces.Length;
            bool hasDelimiter = delimiter != null;
            int tail = count - 1;

            if (trailingDelimiter)
            {
                for (int i = 0; i < count; i++)
                {
                    builder.Append(pieces[i]);
                    if (hasDelimiter)
                    {
                        builder.Append(delimiter);
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    builder.Append(pieces[i]);
                    if (hasDelimiter && i != tail)
                    {
                        builder.Append(delimiter);
                    }
                }
            }
            return builder.ToString();
        }
    }
}