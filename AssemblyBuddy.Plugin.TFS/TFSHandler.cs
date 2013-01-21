namespace AssemblyBuddy.Plugin.TFS
{
    using System;
    using System.Diagnostics;
    using System.IO;

    internal class TfsHandler : ITfsHandler
    {
        private const string CheckoutCommandLine = "checkout /lock:none \"{0}\"";
        private const string PathTemplateFromProgramsToTfExe = @"Microsoft Visual Studio {0}.0\Common7\IDE\TF.exe";
        private readonly string pathToTfExe;
        
        public TfsHandler()
        {
            this.pathToTfExe = this.FindPathToTfExe();
        }

        public void CheckOutFile(string destinationFilePath)
        {
            if (destinationFilePath == null)
            {
                throw new ArgumentNullException("destinationFilePath");
            }

            if (this.pathToTfExe != null)
            {
                string arguments = string.Format(CheckoutCommandLine, destinationFilePath );
                this.StartHiddenProcess(this.pathToTfExe, arguments);
            }
        }

        private string FindPathToTfExe()
        {
            string programFilesDir = System.Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            if (programFilesDir != null)
            {
                for (int visualStudioVersion = 12; visualStudioVersion >= 9; visualStudioVersion--)
                {
                    string exePath = Path.Combine(programFilesDir, string.Format(PathTemplateFromProgramsToTfExe, visualStudioVersion));
                    if (File.Exists(exePath))
                    {
                        return exePath;
                    }
                }
            }
            
            return null;
        }

        private void StartHiddenProcess(string command, string arguments)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo { FileName = command, Arguments = arguments, WindowStyle = ProcessWindowStyle.Hidden};
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit(5000);
        }
    }
}