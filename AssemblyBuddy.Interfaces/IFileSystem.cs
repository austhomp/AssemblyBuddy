namespace AssemblyBuddy.Interfaces
{
    public interface IFileSystem
    {
        IFolder Folder { get; }

        IFileSystemFile GetFileSystemFile(IFileEntry fileEntry);
    }
}