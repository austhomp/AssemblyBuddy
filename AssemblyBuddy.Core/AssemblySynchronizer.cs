namespace AssemblyBuddy.Core
{
    using System;

    using AssemblyBuddy.Interfaces;

    public class AssemblySynchronizer : IAssemblySynchronizer
    {
        public void Sync(IFolder source, IFolder target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
        }
    }
}