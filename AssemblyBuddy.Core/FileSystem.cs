namespace AssemblyBuddy.Core
{
    using AssemblyBuddy.Interfaces;

    public class FileSystem : IFileSystem
    {
        private readonly IFolder folder;

        public static FileSystem CreateFileSystem(string path)
        {
            return new FileSystem(new Folder(path));
        }

        public FileSystem(IFolder folder)
        {
            this.folder = folder;
        }

        public IFolder Folder
        {
            get
            {
                return this.folder;
            }
        }

        public IFileSystemFile GetFileSystemFile(IFileEntry fileEntry)
        {
            return new FileSystemFile(fileEntry);
        }
    }
}