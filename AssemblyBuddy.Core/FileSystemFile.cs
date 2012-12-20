namespace AssemblyBuddy.Core
{
    using System;
    using System.IO;

    using AssemblyBuddy.Interfaces;

    internal class FileSystemFile : IFileSystemFile
    {
        private readonly IFileEntry fileEntry;

        public FileSystemFile(IFileEntry fileEntry)
        {
            this.fileEntry = fileEntry;
        }

        public IFileEntry FileEntry
        {
            get
            {
                return this.fileEntry;
            }
        }

        public long? Size
        {
            get
            {
                long? size = null;
                try
                {
                    var fileInfo = new FileInfo(this.fileEntry.FilePath);
                    size = fileInfo.Length;
                }
                catch (Exception e)
                {
                    // todo: consider logging the exception
                }
                return size;
            }
        }

        public Stream GetStream()
        {
            return File.Open(this.fileEntry.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
    }
}