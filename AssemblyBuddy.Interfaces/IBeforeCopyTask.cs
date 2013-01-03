namespace AssemblyBuddy.Interfaces
{
    public interface IBeforeCopyTask
    {
        void RunBeforeCopy(FileMatch fileMatch);
    }
}