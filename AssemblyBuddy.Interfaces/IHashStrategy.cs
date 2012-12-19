namespace AssemblyBuddy.Interfaces
{
    using System.IO;

    public interface IHashStrategy
    {
        string HashStream(Stream stream);
    }
}