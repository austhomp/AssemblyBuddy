using AssemblyBuddy.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AssemblyBuddy.Interfaces;

namespace AssemblyBuddy.Core.Tests
{
    using System.Collections.Generic;
    using System.IO;

    using Moq;

    using System.Linq;

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

            var sourceFiles = new List<FakeFileEntry>()
                {
                    new FakeFileEntry() { Filename = "one", FileContents = "SAME"},
                    new FakeFileEntry() { Filename = "two", FileContents = "SAME"},
                };
            var source = GetMockFolder(sourceFiles);

            var destinationFiles = new List<FakeFileEntry>()
                {
                    new FakeFileEntry() { Filename = "three", FileContents = "SAME"},
                    new FakeFileEntry() { Filename = "four", FileContents = "SAME"},
                };

            var destination = GetMockFolder(destinationFiles);
            
            var result = target.FindUpdatedAssemblies(source, destination);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod()]
        public void WhenDestinationIsEmpty_NoMatchesAreReturned()
        {
            var target = GetUpdatedAssemblyFinder();

            var sourceFiles = new List<FakeFileEntry>()
                {
                    new FakeFileEntry() { Filename = "one", FileContents = "SAME"},
                    new FakeFileEntry() { Filename = "two", FileContents = "SAME"},
                };
            var source = GetMockFolder(sourceFiles);

            var destinationFiles = new List<FakeFileEntry>();

            var destination = GetMockFolder(destinationFiles);
            
            var result = target.FindUpdatedAssemblies(source, destination);
            Assert.IsTrue(result.Count == 0);
        }
        
        [TestMethod()]
        public void WhenSourceIsEmpty_NoMatchesAreReturned()
        {
            var target = GetUpdatedAssemblyFinder();

            var sourceFiles = new List<FakeFileEntry>();

            var source = GetMockFolder(sourceFiles);

            var destinationFiles = new List<FakeFileEntry>()
                {
                    new FakeFileEntry() { Filename = "three", FileContents = "SAME"},
                    new FakeFileEntry() { Filename = "four", FileContents = "SAME"},
                };

            var destination = GetMockFolder(destinationFiles);
            
            var result = target.FindUpdatedAssemblies(source, destination);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod()]
        public void WhenDestinationAndSourceHaveFilesInCommonThatDoNotDiffer_NoMatchesAreReturned()
        {
            var target = GetUpdatedAssemblyFinder();

            var sourceFiles = new List<FakeFileEntry>()
                {
                    new FakeFileEntry() { Filename = "one", FileContents = "SAME"},
                    new FakeFileEntry() { Filename = "two", FileContents = "SAME"},
                };
            var source = GetMockFolder(sourceFiles);

            var destinationFiles = new List<FakeFileEntry>()
                {
                    new FakeFileEntry() { Filename = "one", FileContents = "SAME"},
                    new FakeFileEntry() { Filename = "four", FileContents = "SAME"},
                };

            var destination = GetMockFolder(destinationFiles);

            var result = target.FindUpdatedAssemblies(source, destination);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod()]
        public void WhenDestinationAndSourceHaveFilesInCommonThatDiffer_CorrectMatchesAreReturned()
        {
            var target = GetUpdatedAssemblyFinder();

            var sourceFiles = new List<FakeFileEntry>()
                {
                    new FakeFileEntry() { Filename = "one", FileContents = "SAME"},
                    new FakeFileEntry() { Filename = "two", FileContents = "SAME"},
                };
            var source = GetMockFolder(sourceFiles);

            var destinationFiles = new List<FakeFileEntry>()
                {
                    new FakeFileEntry() { Filename = "one", FileContents = "DIFFERENT"},
                    new FakeFileEntry() { Filename = "four", FileContents = "SAME"},
                };

            var destination = GetMockFolder(destinationFiles);

            var result = target.FindUpdatedAssemblies(source, destination);
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual(sourceFiles[0].Filename, result[0].SourceFile.Filename);
        }


        #region Heper Methods

        private static IFileSystem GetMockFolder(IList<FakeFileEntry> filesToReturn)
        {
            var fileList = new List<IFileEntry>();
            foreach (var file in filesToReturn)
            {
                var m = new Mock<IFileEntry>();
                var val = file.Filename;
                m.Setup(x => x.Filename).Returns(() => val);
                m.Setup(x => x.FilePath).Returns(string.Empty);
                fileList.Add(m.Object);
            }

            var folder = new Mock<IFolder>();
            folder.Setup(x => x.Files).Returns(() => fileList);
            folder.Setup(x => x.Path).Returns("//mocked");
            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(x => x.Folder).Returns(() => folder.Object);

            foreach (var fileEntry in fileList)
            {
                var fileSystemFile = new Mock<IFileSystemFile>();
                var copyOfFileEntry = fileEntry;
                fileSystemFile.Setup(x => x.FileEntry).Returns(copyOfFileEntry);
                fileSystemFile.Setup(x => x.GetStream()).Returns(() =>
                    {
                        var memoryStream = new MemoryStream();
                        foreach (var character in filesToReturn.First(x => x.Filename == copyOfFileEntry.Filename).FileContents.ToArray())
                        {
                            memoryStream.WriteByte((byte)character);
                        }
                        memoryStream.Position = 0;
                        return memoryStream;
                    });

                fileSystem.Setup(x => x.GetFileSystemFile(copyOfFileEntry)).Returns(() => fileSystemFile.Object );
                
            }

            return fileSystem.Object;
        }


        private static UpdatedAssemblyFinder GetUpdatedAssemblyFinder()
        {
            var folderComparer = new FolderComparer();
            var comparisonStrategy = new DefaultComparisonStrategy();
            var fileComparer = new FileComparer(comparisonStrategy);

            return new UpdatedAssemblyFinder(folderComparer, fileComparer);
        }

        #endregion

        private class FakeFileEntry
        {
            public string Filename { get; set; }

            public string FileContents { get; set; }
        }
    }
}
