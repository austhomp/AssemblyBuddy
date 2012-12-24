namespace AssemblyBuddy.Interfaces
{
    using System.Collections.Generic;

    public interface IBatchCopier
    {
        void Copy(IEnumerable<FileMatch> fileMatches);
    }
}