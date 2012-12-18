namespace AssemblyBuddy.Interfaces
{
    using System.Collections.Generic;

    public interface IFolderComparer
    {
        IList<FileMatch> FindMatches(IFolder source, IFolder destination);
    }
}