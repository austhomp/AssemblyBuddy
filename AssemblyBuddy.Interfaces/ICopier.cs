namespace AssemblyBuddy.Interfaces
{
    public interface ICopier
    {
        void CopyFile(IFileEntry source, IFileEntry destination);
    }
}