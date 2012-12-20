namespace AssemblyBuddy.Core
{
    using AssemblyBuddy.Interfaces;

    internal class FileComparer : IFileComparer
    {
        private readonly IComparisonStrategy comparisonStrategy;

        public FileComparer(IComparisonStrategy comparisonStrategy)
        {
            this.comparisonStrategy = comparisonStrategy;
        }

        public FileComparisonResult Compare(IFileSystemFile source, IFileSystemFile destination)
        {
            return comparisonStrategy.Compare(source, destination);
        }
    }
}