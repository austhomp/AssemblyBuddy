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

        public List<FileMatch> FindUpdatedAssemblies(IFolder source, IFolder target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            
            var potentialMatches = this.folderComparer.FindMatches(source, target);
            var matches = new List<FileMatch>(potentialMatches.Count);
            
            foreach (var potentialMatch in potentialMatches)
            {
                if (FileComparisonResult.Differ == this.fileComparer.Compare(potentialMatch.SourceFile, potentialMatch.DestinationFile))
                {
                    matches.Add(new FileMatch(potentialMatch.SourceFile, potentialMatch.DestinationFile));
                }
            }

            return matches;

        }
    }
}