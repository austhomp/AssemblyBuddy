namespace AssemblyBuddy.Plugin.TFS
{
    public interface ITfsHandler
    {
        void CheckOutFile(string destinationFilePath);
    }
}