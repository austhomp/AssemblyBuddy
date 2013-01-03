namespace AssemblyBuddy.Core
{
    using System.IO;

    using AssemblyBuddy.Interfaces;

    internal class Copier : ICopier
    {
        public void CopyFile(IFileEntry source, IFileEntry destination)
        {
            File.Copy(source.Filename, destination.Filename, true);
        }
    }
}