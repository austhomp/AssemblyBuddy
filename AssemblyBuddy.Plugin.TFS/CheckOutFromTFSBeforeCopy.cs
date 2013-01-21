namespace AssemblyBuddy.Plugin.TFS
{
    using System;
    using AssemblyBuddy.Interfaces;

    public class CheckOutFromTFSBeforeCopy : IBeforeCopyTask
    {
        private readonly ITfsHandler tfsHandler;

        public CheckOutFromTFSBeforeCopy()
            : this(new TfsHandler())
        {
        }

        public CheckOutFromTFSBeforeCopy(ITfsHandler tfsHandler)
        {
            if (tfsHandler == null)
            {
                throw new ArgumentNullException("tfsHandler");
            }

            this.tfsHandler = tfsHandler;
        }

        public void RunBeforeCopy(FileMatch fileMatch)
        {
            if (fileMatch == null)
            {
                throw new ArgumentNullException("fileMatch");
            }

            var destinationFilePath = fileMatch.DestinationFile.FilePath;
            if (System.IO.File.Exists(destinationFilePath))
            {
                this.CheckFileOutFromTfs(destinationFilePath);
            }
            
        }

        private void CheckFileOutFromTfs(string destinationFilePath)
        {
            this.tfsHandler.CheckOutFile(destinationFilePath);
        }
    }
}