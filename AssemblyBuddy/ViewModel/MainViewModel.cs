using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace AssemblyBuddy.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using AssemblyBuddy.Core;
    using AssemblyBuddy.DesignTimeData;
    using AssemblyBuddy.Interfaces;
    using AssemblyBuddy.Plugin.TFS;
    using AssemblyBuddy.Properties;
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
                this.AssemblyList.Add(new FileMatchResult(new FileMatch(new MockFileEntry("test.dll"), new MockFileEntry("test.dll")), FileComparisonResult.Differ));
            }
            else
            {
                // Code runs "for real"
            }

            this.CopyCommand = new RelayCommand(this.PerformCopy, this.CanPerformCopy);
            this.CompareCommand = new RelayCommand(this.PerformCompare, this.CanPerformCompare);
            this.BeforeCopyTasks = new List<IBeforeCopyTask>();
            this.AfterCopyTasks = new List<IAfterCopyTask>();

            // might want an option to enable/disable this in the future
            var copyProgressReporter = new CopyProgressReporter(this.AssemblyList);
            this.BeforeCopyTasks.Add(copyProgressReporter);
            this.AfterCopyTasks.Add(copyProgressReporter);
            this.BeforeCopyTasks.Add(new CheckOutFromTFSBeforeCopy());
        }

        private async void PerformCopy()
        {
            this.isCopyInProgress = true;
            this.OutputDisplay = Resources.MainViewModel_PerformCopy_Starting_copy;
            var copier = BatchCopier.CreateBatchCopier(this.BeforeCopyTasks, this.AfterCopyTasks);
            var filesToCopy = this.AssemblyList
                .Where(x => x.ComparisonResult == FileComparisonResult.Differ)
                .Select(x => new FileMatch(x.Match.SourceFile, x.Match.DestinationFile)).ToList();

            try
            {
                await copier.Copy(filesToCopy);
            }
            catch (Exception e)
            {
                this.OutputDisplay = e.Message + Environment.NewLine + Environment.NewLine + e.ToString();
            }
            finally
            {
                this.isCopyInProgress = false;
                if (this.OutputDisplay == Resources.MainViewModel_PerformCopy_Starting_copy)
                {
                    this.OutputDisplay = Resources.MainViewModel_PerformCopy_Copy_complete;
                }
            }
        }



        private bool CanPerformCopy()
        {
            return !this.isCopyInProgress 
                && this.AssemblyList.Any(x => x.ComparisonResult == FileComparisonResult.Differ);
        }

        private void PerformCompare()
        {
            this.isCompareInProgress = true;
            this.OutputDisplay = Resources.MainViewModel_PerformCompare_CompareStarting;
            try
            {
                var finder = UpdatedAssemblyFinder.CreateUpdatedAssemblyFinder();
                var assembliesToUpdate = finder.FindUpdatedAssemblies(
                    FileSystem.CreateFileSystem(this.SourcePath),
                    FileSystem.CreateFileSystem(this.DestinationPath));

                this.AssemblyList.Clear();
                foreach (var result in assembliesToUpdate)
                {
                    this.AssemblyList.Add(result);
                }


                this.OutputDisplay = string.Format(Resources.MainViewModel_PerformCompare_ChangesDetected, assembliesToUpdate.Count);
            }
            catch (Exception e)
            {
                this.OutputDisplay = e.Message + Environment.NewLine + Environment.NewLine + e.ToString();
            }
            finally
            {
                this.isCompareInProgress = false;
            }
        }

        private bool CanPerformCompare()
        {
            return !this.isCompareInProgress
                && !this.isCopyInProgress
                && !string.IsNullOrWhiteSpace(this.SourcePath) 
                && !string.IsNullOrWhiteSpace(this.DestinationPath);
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

        private ObservableCollection<FileMatchResult> assemblyList = new ObservableCollection<FileMatchResult>();

        private IBatchCopier batchCopier;

        /// <summary>
        /// Gets the AssemblyList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<FileMatchResult> AssemblyList
        {
            get
            {
                return assemblyList;
            }
        }

        /// <summary>
        /// The <see cref="OutputDisplay" /> property's name.
        /// </summary>
        public const string OutputDisplayPropertyName = "OutputDisplay";

        private string outputDisplay = string.Empty;

        private List<IBeforeCopyTask> BeforeCopyTasks;

        private List<IAfterCopyTask> AfterCopyTasks;

        private bool isCopyInProgress;

        private bool isCompareInProgress;

        /// <summary>
        /// Gets the OutputDisplay property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string OutputDisplay
        {
            get
            {
                return outputDisplay;
            }

            set
            {
                if (outputDisplay == value)
                {
                    return;
                }

                var oldValue = outputDisplay;
                outputDisplay = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(OutputDisplayPropertyName);
            }
        }
    }
}