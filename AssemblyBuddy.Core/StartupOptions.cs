using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace AssemblyBuddy.Core
{
    public class StartupOptions
    {
        [Option('s', "source", Required = false, HelpText = "Source directory")]
        public string SourceDirectory { get; set; }

        [Option('d', "destination", Required = false, HelpText = "Destination directory")]
        public string DestinationDirectory { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}