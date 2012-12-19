namespace AssemblyBuddy.Interfaces
{
    public interface IFileComparer
    {
        FileComparisonResult Compare(IFileSystemFile source, IFileSystemFile destination);
    }
}