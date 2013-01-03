namespace AssemblyBuddy.Core
{
    using System;
    using System.IO;

    using AssemblyBuddy.Interfaces;

    internal class DefaultComparisonStrategy : IComparisonStrategy
    {
        private readonly int BufferSize;

        public DefaultComparisonStrategy(int streamComparisonBufferSize = 4096)
        {
            this.BufferSize = streamComparisonBufferSize;
        }

        public FileComparisonResult Compare(IFileSystemFile source, IFileSystemFile destination)
        {
            if (source.Size != destination.Size)
            {
                return FileComparisonResult.Differ;
            }

            return CompareStreams(source, destination);
        }

        private FileComparisonResult CompareStreams(IFileSystemFile source, IFileSystemFile destination)
        {
            using (Stream sourceStream = source.GetStream())
            {
                using (Stream destinationStream = destination.GetStream())
                {
                    var sourceBuffer = new byte[this.BufferSize];
                    var destinationBuffer = new byte[this.BufferSize];

                    int sourceReadCount;
                    do
                    {
                        sourceReadCount = sourceStream.Read(sourceBuffer, 0, this.BufferSize);
                        int destinationReadCount = destinationStream.Read(destinationBuffer, 0, this.BufferSize);

                        if (sourceReadCount != destinationReadCount)
                        {
                            return FileComparisonResult.Differ;
                        }

                        for (int i = 0; i < sourceReadCount; i++)
                        {
                            if (sourceBuffer[i] != destinationBuffer[i])
                            {
                                return FileComparisonResult.Differ;
                            }
                        }
                    }
                    while (sourceReadCount > 0);

                    return FileComparisonResult.Same;
                }
            }
        }
    }
}