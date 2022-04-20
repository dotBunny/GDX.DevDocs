// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE below for full license information.

/*
The MIT License (MIT)

Copyright (c) Microsoft Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

// A small subset taken from https://github.com/dotnet/docfx to allow for some manipulation of the created data

namespace Dox.TableOfContents
{
    using System.Collections.Generic;

    public class ExtractMetadataOptions
    {
        public bool ShouldSkipMarkup { get; set; }

        public bool PreserveRawInlineComments { get; set; }

        public string FilterConfigFile { get; set; }

        public Dictionary<string, string> MSBuildProperties { get; set; }

        public string CodeSourceBasePath { get; set; }

        public bool DisableDefaultFilter { get; set; }
    }
}