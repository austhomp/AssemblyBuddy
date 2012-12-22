namespace AssemblyBuddy.DesignTimeData
{
    using AssemblyBuddy.Interfaces;

    public class MockFileEntry : IFileEntry
    {
        public MockFileEntry(string filename)
        {
            this.Filename = filename;
            this.FilePath = @"Z:\" + filename;
        }

        public MockFileEntry(string filePath, string filename)
        {
            this.FilePath = filePath;
            this.Filename = filename;
        }

        public string FilePath { get; private set; }

        public string Filename { get; private set; }
    }
}