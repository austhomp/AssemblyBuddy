namespace AssemblyBuddy.Interfaces
{
    using System;

    public class FileMatch
    {
        public FileMatch(IFileEntry sourceFile, IFileEntry destinationFile)
        {
            if (sourceFile == null)
            {
                throw new ArgumentNullException("sourceFile");
            }

            if (destinationFile == null)
            {
                throw new ArgumentNullException("destinationFile");
            }

            this.SourceFile = sourceFile;
            this.DestinationFile = destinationFile;
        }

        public IFileEntry SourceFile { get; private set; }
        
        public IFileEntry DestinationFile { get; private set; }
    }
}