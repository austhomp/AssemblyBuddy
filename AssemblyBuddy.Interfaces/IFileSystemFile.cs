namespace AssemblyBuddy.Interfaces
{
    using System.IO;

    public interface IFileSystemFile
    {
        IFileEntry FileEntry { get;  }

        ulong Size { get; }

        Stream GetStream();
    }
}