﻿namespace AssemblyBuddy.Core
{
    using System;
    using System.Collections.Generic;

    using AssemblyBuddy.Interfaces;

    public class UpdatedAssemblyFinder : IUpdatedAssemblyFinder
    {
        private readonly IFolderComparer folderComparer;

        private readonly IFileComparer fileComparer;

        public UpdatedAssemblyFinder(IFolderComparer folderComparer, IFileComparer fileComparer)
        {
            this.folderComparer = folderComparer;
            this.fileComparer = fileComparer;
        }

        public List<FileMatch> FindUpdatedAssemblies(IFileSystem source, IFileSystem destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }
            
            var potentialMatches = this.folderComparer.FindMatches(source.Folder, destination.Folder);
            var matches = new List<FileMatch>(potentialMatches.Count);
            
            foreach (var potentialMatch in potentialMatches)
            {
                var sourceFile = source.GetFileSystemFile(potentialMatch.SourceFile);
                var destinationFile = destination.GetFileSystemFile(potentialMatch.DestinationFile);
                if (FileComparisonResult.Differ == this.fileComparer.Compare(sourceFile, destinationFile))
                {
                    matches.Add(new FileMatch(potentialMatch.SourceFile, potentialMatch.DestinationFile));
                }
            }

            return matches;

        }
    }
}