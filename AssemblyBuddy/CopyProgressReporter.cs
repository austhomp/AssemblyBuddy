namespace AssemblyBuddy
{
    using System.Collections.Generic;
    using System.Linq;
    using AssemblyBuddy.Interfaces;

    public class CopyProgressReporter : IBeforeCopyTask, IAfterCopyTask
    {
        private readonly List<FileMatchResult> fileMatches;

        public CopyProgressReporter(IEnumerable<FileMatchResult> fileMatches)
        {
            this.fileMatches = fileMatches.ToList();
        }

        public void RunBeforeCopy(FileMatch fileMatch)
        {
            var matchFromList = this.fileMatches.Where(x => x.Match.SourceFile == fileMatch.SourceFile).SingleOrDefault();
            if (matchFromList != null)
            {
                System.Diagnostics.Debug.Write("Starting copy for " + matchFromList.Match.SourceFile);
            }
        }

        public void RunAfterCopy(FileMatch fileMatch)
        {
            var matchFromList = this.fileMatches.Where(x => x.Match.SourceFile == fileMatch.SourceFile).SingleOrDefault();
            if (matchFromList != null)
            {
                System.Diagnostics.Debug.Write("Finished copy for " + matchFromList.Match.SourceFile); 
                matchFromList.ComparisonResult = FileComparisonResult.Same;
            }
        }
    }
}