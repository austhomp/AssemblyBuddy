namespace AssemblyBuddy.Interfaces
{
    using System.Collections.Generic;

    public interface IUpdatedAssemblyFinder
    {
        List<FileMatchResult> FindUpdatedAssemblies(IFileSystem source, IFileSystem destination);
    }
}