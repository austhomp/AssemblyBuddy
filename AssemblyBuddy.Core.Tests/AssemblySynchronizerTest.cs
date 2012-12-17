using AssemblyBuddy.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AssemblyBuddy.Interfaces;

namespace AssemblyBuddy.Core.Tests
{


    [TestClass()]
    public class AssemblySynchronizerTest
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenSourceIsNull_ExceptionIsThrown()
        {
            var target = new AssemblySynchronizer();
            IFolder source = null;
            IFolder target1 = new Folder(string.Empty);
            target.Sync(source, target1);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenTargetIsNull_ExceptionIsThrown()
        {
            var target = new AssemblySynchronizer();
            IFolder source = new Folder(string.Empty);
            IFolder target1 = null;
            target.Sync(source, target1);
        }
    
    }
}
