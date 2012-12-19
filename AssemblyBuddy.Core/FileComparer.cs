﻿namespace AssemblyBuddy.Core
{
    using AssemblyBuddy.Interfaces;

    internal class FileComparer : IFileComparer
    {
        private IHashStrategy hashStrategy;

        public FileComparer(IHashStrategy hashStrategy)
        {
            this.hashStrategy = hashStrategy;
        }

        public FileComparisonResult Compare(IFileSystemFile source, IFileSystemFile destination)
        {
            if (source.Size != destination.Size)
            {
                return FileComparisonResult.Differ;
            }

            return source.CompareWith(destination, this.hashStrategy);
        }
    }
}