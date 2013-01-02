namespace AssemblyBuddy.Core
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

        public static UpdatedAssemblyFinder CreateUpdatedAssemblyFinder()
        {
            return new UpdatedAssemblyFinder(new FolderComparer(), new FileComparer(new DefaultComparisonStrategy()));
        }

        public List<FileMatchResult> FindUpdatedAssemblies(IFileSystem source, IFileSystem destination)
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
            var matchResults = new List<FileMatchResult>(potentialMatches.Count);
            
            foreach (var potentialMatch in potentialMatches)
            {
                var sourceFile = source.GetFileSystemFile(potentialMatch.SourceFile);
                var destinationFile = destination.GetFileSystemFile(potentialMatch.DestinationFile);
                var fileMatch = new FileMatch(potentialMatch.SourceFile, potentialMatch.DestinationFile);
                var comparsionResult = this.fileComparer.Compare(sourceFile, destinationFile);
                matchResults.Add(new FileMatchResult(fileMatch, comparsionResult));
            }

            return matchResults;

        }
    }
}