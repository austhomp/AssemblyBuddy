namespace AssemblyBuddy.Core
{
    using AssemblyBuddy.Interfaces;

    internal class DefaultComparisonStrategy : IComparisonStrategy
    {
        public FileComparisonResult Compare(IFileSystemFile source, IFileSystemFile destination)
        {
            if (source.Size != destination.Size)
            {
                return FileComparisonResult.Differ;
            }

            return FileComparisonResult.Match;
        }
    }
}