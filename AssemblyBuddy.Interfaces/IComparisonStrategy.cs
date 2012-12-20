namespace AssemblyBuddy.Interfaces
{
    public interface IComparisonStrategy
    {
        FileComparisonResult Compare(IFileSystemFile source, IFileSystemFile destination);
    }
}