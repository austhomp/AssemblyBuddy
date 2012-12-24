using GalaSoft.MvvmLight;

namespace AssemblyBuddy.ViewModel
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using AssemblyBuddy.Core;
    using AssemblyBuddy.DesignTimeData;
    using AssemblyBuddy.Interfaces;

    using GalaSoft.MvvmLight.Command;

    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                this.SourcePath = @"Z:\some\source\diectory";
                this.DestinationPath = @"Z:\some\destination\diectory";
                this.AssemblyList.Add(new FileMatch(new MockFileEntry("test.dll"), new MockFileEntry("test.dll")));
            }
            else
            {
                // Code runs "for real"
            }

            this.CopyCommand = new RelayCommand(this.PerformCopy, this.CanPerformCopy);
            this.CompareCommand = new RelayCommand(this.PerformCompare, this.CanPerformCompare);

            
        }

        private void PerformCopy()
        {
            foreach (var fileMatch in AssemblyList)
            {
                System.Diagnostics.Debug.WriteLine("Going to copy {0} to {1}", fileMatch.SourceFile.FilePath, fileMatch.DestinationFile.FilePath);    
            }
            
        }

        private bool CanPerformCopy()
        {
            // todo: add more logic here as needed
            return this.AssemblyList.Count > 0;
        }

        private void PerformCompare()
        {
            var finder = UpdatedAssemblyFinder.CreateUpdatedAssemblyFinder();
            var assembliesToUpdate = finder.FindUpdatedAssemblies(
                FileSystem.CreateFileSystem(this.SourcePath),
                FileSystem.CreateFileSystem(this.DestinationPath));

            this.AssemblyList = assembliesToUpdate;
        }

        private bool CanPerformCompare()
        {
            // todo: add logic for directories existing
            return !string.IsNullOrWhiteSpace(this.SourcePath) && !string.IsNullOrWhiteSpace(this.DestinationPath);
        }

        public ICommand CopyCommand { get; private set; }
        
        public ICommand CompareCommand { get; private set; }

        /// <summary>
        /// The <see cref="SourcePath" /> property's name.
        /// </summary>
        public const string SourcePathPropertyName = "SourcePath";

        private string sourcePath = "";

        /// <summary>
        /// Gets the SourcePath property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SourcePath
        {
            get
            {
                return sourcePath;
            }

            set
            {
                if (sourcePath == value)
                {
                    return;
                }

                var oldValue = sourcePath;
                sourcePath = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(SourcePathPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="DestinationPath" /> property's name.
        /// </summary>
        public const string DestinationPathPropertyName = "DestinationPath";

        private string destinationPath = "";

        /// <summary>
        /// Gets the DestinationPath property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DestinationPath
        {
            get
            {
                return destinationPath;
            }

            set
            {
                if (destinationPath == value)
                {
                    return;
                }

                var oldValue = destinationPath;
                destinationPath = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(DestinationPathPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="AssemblyList" /> property's name.
        /// </summary>
        public const string AssemblyListPropertyName = "AssemblyList";

        private IList<FileMatch> assemblyList = new List<FileMatch>();

        private IBatchCopier batchCopier;

        /// <summary>
        /// Gets the AssemblyList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<FileMatch> AssemblyList
        {
            get
            {
                return assemblyList;
            }

            set
            {
                if (assemblyList == value)
                {
                    return;
                }

                var oldValue = assemblyList;
                assemblyList = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(AssemblyListPropertyName);
            }
        }

    }
}