namespace AssemblyBuddy.Core
{
    using System.Collections.Generic;
    using AssemblyBuddy.Interfaces;

    internal class Folder : IFolder
    {
        private readonly string path;

        public Folder(string path)
        {
            this.path = path;
        }

        public IList<IFileEntry> Files
        {
            get
            {
                var files = System.IO.Directory.GetFiles(this.path);
                var list = new List<IFileEntry>(files.GetUpperBound(0));
                foreach (var file in files)
                {
                    list.Add(new FileEntry(file));
                }

                return list;
            }
        }

        public string Path
        {
            get
            {
                return this.path;
            }
        }
    }
}