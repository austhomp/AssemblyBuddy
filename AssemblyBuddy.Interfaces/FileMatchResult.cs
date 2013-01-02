namespace AssemblyBuddy.Interfaces
{
    public class FileMatchResult
    {
        public FileMatchResult(FileMatch match, FileComparisonResult comparisonResult)
        {
            this.Match = match;
            this.ComparisonResult = comparisonResult;
        }

        public FileComparisonResult ComparisonResult { get; set; }

        public FileMatch Match { get; private set; }
    }
}