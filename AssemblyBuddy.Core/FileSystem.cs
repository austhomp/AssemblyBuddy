namespace AssemblyBuddy.Core
{
    using AssemblyBuddy.Interfaces;

    class FileSystem : IFileSystem
    {
        private readonly IFolder folder;

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