namespace AssemblyBuddy.Interfaces
{
    public interface IFileSystemFile
    {
        ulong Size { get; }

        string ComputeHash(IHashStrategy hashStrategy);
    }
}