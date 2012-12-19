namespace AssemblyBuddy.Core
{
    using System.Collections.Generic;

    using AssemblyBuddy.Interfaces;

    internal class FolderComparer : IFolderComparer
    {
        public IList<FileMatch> FindMatches(IFolder source, IFolder destination)
        {
            var matches = new List<FileMatch>();
            
            foreach (var sourceFile in source.Files)
            {
                foreach (var destinationFile in destination.Files)
                {
                    if (sourceFile.Filename == destinationFile.Filename)
                    {
                        matches.Add(new FileMatch(sourceFile, destinationFile));
                    }
                }
                
            }

            return matches;
        }
    }
}