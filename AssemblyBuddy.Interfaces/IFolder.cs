namespace AssemblyBuddy.Interfaces
{
    using System.Collections.Generic;

    public interface IFolder
    {
        IList<IFileEntry> Files { get; }

        string Path { get; }
    }
}