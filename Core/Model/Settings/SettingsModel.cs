using System;
using System.Collections.Generic;
using System.Globalization;

namespace Core.Model.Settings
{
    public class SettingsModel
    {
        public string FolderForHistory { get; set; }

        public bool IsUseFilter { get; set; }

        public bool IsUseIgnoreFilter { get; set; }

        public string DefaultSourceFolder { get; set; }

        public string DefaultTargetFolder { get; set; }

        public List<string> IgnorableFileFormat { get; set; }

        public List<string> FilteredFileFormat { get; set; }
        
        public CultureInfo CultureInfo { get; set; }

        public SettingsModel()
        {
            IgnorableFileFormat = new List<string>();
            FilteredFileFormat = new List<string>();
            FolderForHistory = string.Empty;
            DefaultSourceFolder = string.Empty;
            DefaultTargetFolder = string.Empty;
            CultureInfo = new CultureInfo(string.Empty);
        }
    }
}