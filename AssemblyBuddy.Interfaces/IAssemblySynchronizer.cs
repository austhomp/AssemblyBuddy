namespace AssemblyBuddy.Interfaces
{
    public interface IAssemblySynchronizer
    {
        void Sync(IFolder source, IFolder target);
    }
}