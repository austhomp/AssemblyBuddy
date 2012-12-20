namespace AssemblyBuddy.Interfaces
{
    using System.IO;

    public interface IFileSystemFile
    {
        IFileEntry FileEntry { get;  }

        long? Size { get; }

        Stream GetStream();
    }
}