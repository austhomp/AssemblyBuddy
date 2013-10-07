namespace AssemblyBuddy.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBatchCopier
    {
        Task Copy(IEnumerable<FileMatch> fileMatches);
    }
}