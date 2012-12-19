namespace AssemblyBuddy.Interfaces
{
    using System.Collections.Generic;

    public interface IUpdatedAssemblyFinder
    {
        List<FileMatch> FindUpdatedAssemblies(IFileSystem source, IFileSystem target);
    }
}