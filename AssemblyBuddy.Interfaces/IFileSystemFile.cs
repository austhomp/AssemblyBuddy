namespace AssemblyBuddy.Interfaces
{
    using System.IO;

    public interface IFileSystemFile
    {
        ulong Size { get; }

        Stream GetStream();

        FileComparisonResult CompareWith(IFileSystemFile other, IHashStrategy hashStrategy);
    }
}