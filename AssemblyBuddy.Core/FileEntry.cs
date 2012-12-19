namespace AssemblyBuddy.Core
{
    using System;

    using AssemblyBuddy.Interfaces;

    internal class FileEntry : IFileEntry
    {
        private readonly string filePath;

        public FileEntry(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            this.filePath = filePath;
        }

        public string FilePath
        {
            get
            {
                return this.filePath;
            }
        }

        public string Filename
        {
            get
            {
                return System.IO.Path.GetFileName(this.filePath);
            }
        }
        
    }
}