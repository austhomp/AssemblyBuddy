namespace AssemblyBuddy.Interfaces
{
    using System.Collections.Generic;

    public interface IUpdatedAssemblyFinder
    {
        IList<FileMatch> FindUpdatedAssemblies(IFileSystem source, IFileSystem destination);
    }
}