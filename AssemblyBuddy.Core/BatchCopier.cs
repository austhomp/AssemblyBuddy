namespace AssemblyBuddy.Core
{
    using System;
    using System.Collections.Generic;

    using AssemblyBuddy.Interfaces;
    using System.Linq;

    public class BatchCopier : IBatchCopier
    {
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

        public static BatchCopier CreateBatchCopier()
        {
            return new BatchCopier(new Copier(), new List<IBeforeCopyTask>(), new List<IAfterCopyTask>());
        }

        public static BatchCopier CreateBatchCopier(IEnumerable<IBeforeCopyTask> beforeCopyTasks, IEnumerable<IAfterCopyTask> afterCopyTasks)
        {
            if (beforeCopyTasks == null)
            {
                throw new ArgumentNullException("beforeCopyTasks");
            }

            if (afterCopyTasks == null)
            {
                throw new ArgumentNullException("afterCopyTasks");
            }

            return new BatchCopier(new Copier(), beforeCopyTasks.ToList(), afterCopyTasks.ToList());
        }

        public void Copy(IEnumerable<FileMatch> fileMatches)
        {
            foreach (var fileMatch in fileMatches)
            {
                this.RunBeforeCopyTasks(fileMatch);
                this.fileCopier.CopyFile(fileMatch.SourceFile, fileMatch.DestinationFile);
                this.RunAfterCopyTasks(fileMatch);
            }
        }

        private void RunBeforeCopyTasks(FileMatch fileMatch)
        {
            foreach (var beforeCopyTask in this.beforeCopyTasks)
            {
                beforeCopyTask.RunBeforeCopy(fileMatch);
            }
        }

        private void RunAfterCopyTasks(FileMatch fileMatch)
        {
            foreach (var afterCopyTask in this.afterCopyTasks)
            {
                afterCopyTask.RunAfterCopy(fileMatch);
            }
        }
    }
}