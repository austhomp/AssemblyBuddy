using AssemblyBuddy.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AssemblyBuddy.Interfaces;

namespace AssemblyBuddy.Core.Tests
{
    using System.Collections.Generic;
    using System.IO;

    using Moq;

    [TestClass()]
    public class UpdatedAssemblyFinderTest
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenSourceIsNull_ExceptionIsThrown()
        {
            var target = GetUpdatedAssemblyFinder();
            IFileSystem source = null;
            var destination = new Mock<IFileSystem>();

            target.FindUpdatedAssemblies(source, destination.Object);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenDestinationIsNull_ExceptionIsThrown()
        {
            var target = GetUpdatedAssemblyFinder();
            var source = new Mock<IFileSystem>();
            IFileSystem destination = null;

            target.FindUpdatedAssemblies(source.Object, destination);
        }

        [TestMethod()]
        public void WhenDestinationAndSourceHaveNoFilesInCommon_NoMatchesAreReturned()
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

            var destination = GetMockFolder(destinationFiles);
            
            var result = target.FindUpdatedAssemblies(source, destination);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod()]
        public void WhenDestinationIsEmpty_NoMatchesAreReturned()
        {
            var target = GetUpdatedAssemblyFinder();

            var sourceFiles = new List<string>()
                {
                    "one",
                    "two"
                };
            var source = GetMockFolder(sourceFiles);

            var destinationFiles = new List<string>();

            var destination = GetMockFolder(destinationFiles);
            
            var result = target.FindUpdatedAssemblies(source, destination);
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

            var destination = GetMockFolder(destinationFiles);
            
            var result = target.FindUpdatedAssemblies(source, destination);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod()]
        public void WhenDestinationAndSourceHaveFilesInCommonThatDoNotDiffer_NoMatchesAreReturned()
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

            var destination = GetMockFolder(destinationFiles);

            var result = target.FindUpdatedAssemblies(source, destination);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod()]
        public void WhenDestinationAndSourceHaveFilesInCommonThatDiffer_CorrectMatchesAreReturned()
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

            var destination = GetMockFolder(destinationFiles);

            var result = target.FindUpdatedAssemblies(source, destination);
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual(sourceFiles[0], result[0].SourceFile.Filename);
        }


        #region Heper Methods

        private static IFileSystem GetMockFolder(IEnumerable<string> filesToReturn)
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
            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(x => x.Folder).Returns(() => folder.Object);
            ////fileSystem.Setup(x => x.GetFileSystemFile(It.IsAny<IFileEntry>())).Returns(null);

            return fileSystem.Object;
        }


        private static UpdatedAssemblyFinder GetUpdatedAssemblyFinder(bool shouldDiffer = false)
        {
            var comparisonResult = shouldDiffer ? FileComparisonResult.Differ : FileComparisonResult.Match;
            var folderComparer = new FolderComparer();
            var comparisonStrategy = new Mock<IComparisonStrategy>();
            comparisonStrategy.Setup(x => x.Compare(It.IsAny<IFileSystemFile>(), It.IsAny<IFileSystemFile>())).Returns(comparisonResult);
            var fileComparer = new FileComparer(comparisonStrategy.Object);

            return new UpdatedAssemblyFinder(folderComparer, fileComparer);
        }

        #endregion
    }
}
