using AssemblyBuddy.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AssemblyBuddy.Interfaces;

namespace AssemblyBuddy.Core.Tests
{
    using System.Collections.Generic;

    using Moq;

    [TestClass()]
    public class UpdatedAssemblyFinderTest
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenSourceIsNull_ExceptionIsThrown()
        {
            var target = GetUpdatedAssemblyFinder();
            IFolder source = null;
            var target1 = new Mock<IFolder>();

            target.FindUpdatedAssemblies(source, target1.Object);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenTargetIsNull_ExceptionIsThrown()
        {
            var target = GetUpdatedAssemblyFinder();
            var source = new Mock<IFolder>();
            IFolder target1 = null;

            target.FindUpdatedAssemblies(source.Object, target1);
        }

        [TestMethod()]
        public void WhenTargetAndSourceHaveNoFilesInCommon_NoMatchesAreReturned()
        {
            var target = GetUpdatedAssemblyFinder();

            var sourceFiles = new List<string>()
                {
                    "one",
                    "two"
                };
            var source = GetMockFolder(sourceFiles);

            var destinationFiles = new List<string>()
                {
                    "three",
                    "four"
                };

            var target1 = GetMockFolder(destinationFiles);
            
            var result = target.FindUpdatedAssemblies(source, target1);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod()]
        public void WhenTargetIsEmpty_NoMatchesAreReturned()
        {
            var target = GetUpdatedAssemblyFinder();

            var sourceFiles = new List<string>()
                {
                    "one",
                    "two"
                };
            var source = GetMockFolder(sourceFiles);

            var destinationFiles = new List<string>();

            var target1 = GetMockFolder(destinationFiles);
            
            var result = target.FindUpdatedAssemblies(source, target1);
            Assert.IsTrue(result.Count == 0);
        }
        
        [TestMethod()]
        public void WhenSourceIsEmpty_NoMatchesAreReturned()
        {
            var target = GetUpdatedAssemblyFinder();

            var sourceFiles = new List<string>();

            var source = GetMockFolder(sourceFiles);

            var destinationFiles = new List<string>()
                {
                    "three",
                    "four"
                };

            var target1 = GetMockFolder(destinationFiles);
            
            var result = target.FindUpdatedAssemblies(source, target1);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod()]
        public void WhenTargetAndSourceHaveFilesInCommonThatDoNotDiffer_NoMatchesAreReturned()
        {
            var target = GetUpdatedAssemblyFinder();

            var sourceFiles = new List<string>()
                {
                    "one",
                    "two"
                };
            var source = GetMockFolder(sourceFiles);

            var destinationFiles = new List<string>()
                {
                    "one",
                    "four"
                };

            var target1 = GetMockFolder(destinationFiles);

            var result = target.FindUpdatedAssemblies(source, target1);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod()]
        public void WhenTargetAndSourceHaveFilesInCommonThatDiffer_CorrectMatchesAreReturned()
        {
            var target = GetUpdatedAssemblyFinder(true);

            var sourceFiles = new List<string>()
                {
                    "one",
                    "two"
                };
            var source = GetMockFolder(sourceFiles);

            var destinationFiles = new List<string>()
                {
                    "one",
                    "four"
                };

            var target1 = GetMockFolder(destinationFiles);

            var result = target.FindUpdatedAssemblies(source, target1);
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual(sourceFiles[0], result[0].SourceFile.Filename);
        }


        #region Heper Methods

        private static IFolder GetMockFolder(IEnumerable<string> filesToReturn)
        {
            var fileList = new List<IFileEntry>();
            foreach (var file in filesToReturn)
            {
                var m = new Mock<IFileEntry>();
                var val = file;
                m.Setup(x => x.Filename).Returns(() => val);
                m.Setup(x => x.FilePath).Returns(string.Empty);
                fileList.Add(m.Object);
            }

            var folder = new Mock<IFolder>();
            folder.Setup(x => x.Files).Returns(() => fileList);
            folder.Setup(x => x.Path).Returns("//mocked");
            return folder.Object;
        }


        private static UpdatedAssemblyFinder GetUpdatedAssemblyFinder(bool matchesShouldDiffer = false)
        {
            var folderComparer = new FolderComparer();
            var fileComparer = new Mock<IFileComparer>();
            var fileComparisonResult = matchesShouldDiffer ? FileComparisonResult.Differ : FileComparisonResult.Match;
            fileComparer.Setup(x => x.Compare(It.IsAny<IFileEntry>(), It.IsAny<IFileEntry>())).Returns(
                fileComparisonResult);

            return new UpdatedAssemblyFinder(folderComparer, fileComparer.Object);
        }

        #endregion
    }
}
