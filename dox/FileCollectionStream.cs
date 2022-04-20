// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Dox
{
    public class FileCollectionStream : Stream
    {
        private IEnumerator<string> _fileEnumerator;
        private FileStream _stream;

        public FileCollectionStream(IEnumerable<string> files)
        {
            if (files == null) _fileEnumerator = null;
            else _fileEnumerator = files.GetEnumerator();
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override void Flush() => throw new NotSupportedException();

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_fileEnumerator == null)
            {
                return 0;
            }

            if (_stream == null && !TryGetNextFileStream(out _stream))
            {
                return 0;
            }

            int readed;
            while (true)
            {
                readed = _stream.Read(buffer, offset, count);
                if (readed == 0)
                {
                    // Dispose current stream before fetching the next one
                    _stream.Dispose();
                    if (!TryGetNextFileStream(out _stream))
                    {
                        return 0;
                    }
                }
                else
                {
                    return readed;
                }
            }
        }

        public override long Seek(long offset, SeekOrigin origin)=>
            throw new NotSupportedException();

        public override void SetLength(long value) =>
            throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) =>
            throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_fileEnumerator != null)
                {
                    _fileEnumerator.Dispose();
                }
                if (_stream != null)
                {
                    _stream.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private bool TryGetNextFileStream(out FileStream stream)
        {
            bool next = _fileEnumerator.MoveNext();
            if (!next)
            {
                stream = null;
                return false;
            }

            stream = new FileStream(_fileEnumerator.Current, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return true;
        }
    }
}