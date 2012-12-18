namespace AssemblyBuddy.Interfaces
{
    public interface IFileComparer
    {
        FileComparisonResult Compare(IFileEntry source, IFileEntry destination);
    }
}