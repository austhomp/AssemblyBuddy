namespace AssemblyBuddy.Core
{
    using System.Collections.Generic;

    using AssemblyBuddy.Interfaces;

    public class BatchCopier : IBatchCopier
    {
        public static BatchCopier CreateBatchCopier()
        {
            return new BatchCopier(new Copier());
        }

        private ICopier fileCopier;

        public BatchCopier(ICopier fileCopier)
        {
            this.fileCopier = fileCopier;
        }

        public void Copy(IEnumerable<FileMatch> fileMatches)
        {
            foreach (var fileMatch in fileMatches)
            {
                this.fileCopier.CopyFile(fileMatch.SourceFile, fileMatch.DestinationFile);
            }
        }
    }
}