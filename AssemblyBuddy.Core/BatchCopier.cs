namespace AssemblyBuddy.Core
{
    using System;
    using System.Collections.Generic;

    using AssemblyBuddy.Interfaces;

    public class BatchCopier : IBatchCopier
    {
        public static BatchCopier CreateBatchCopier()
        {
            return new BatchCopier(new Copier(), new List<IBeforeCopyTask>(), new List<IAfterCopyTask>());
        }

        private readonly ICopier fileCopier;

        private readonly List<IBeforeCopyTask> beforeCopyTasks;
        
        private readonly List<IAfterCopyTask> afterCopyTasks;

        public BatchCopier(ICopier fileCopier, IEnumerable<IBeforeCopyTask> beforeCopyTasks, IEnumerable<IAfterCopyTask> afterCopyTasks)
        {
            if (fileCopier == null)
            {
                throw new ArgumentNullException("fileCopier");
            }

            if (beforeCopyTasks == null)
            {
                throw new ArgumentNullException("beforeCopyTasks");
            }

            if (afterCopyTasks == null)
            {
                throw new ArgumentNullException("afterCopyTasks");
            }

            this.fileCopier = fileCopier;
            this.beforeCopyTasks = new List<IBeforeCopyTask>();
            this.beforeCopyTasks.AddRange(beforeCopyTasks);
            this.afterCopyTasks = new List<IAfterCopyTask>();
            this.afterCopyTasks.AddRange(afterCopyTasks);
        }

        public void Copy(IEnumerable<FileMatch> fileMatches)
        {
            foreach (var fileMatch in fileMatches)
            {
                RunBeforeCopyTasks(fileMatch);
                this.fileCopier.CopyFile(fileMatch.SourceFile, fileMatch.DestinationFile);
                RunAfterCopyTasks(fileMatch);
            }
        }

        private void RunBeforeCopyTasks(FileMatch fileMatch)
        {
            foreach (var beforeCopyTask in beforeCopyTasks)
            {
                beforeCopyTask.RunBeforeCopy(fileMatch);
            }
        }

        private void RunAfterCopyTasks(FileMatch fileMatch)
        {
            foreach (var afterCopyTask in afterCopyTasks)
            {
                afterCopyTask.RunAfterCopy(fileMatch);
            }
        }
    }
}