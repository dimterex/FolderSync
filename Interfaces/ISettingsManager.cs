namespace FolderSyns.Interfaces
{
    using System.Collections.Generic;

    public interface ISettingsManager
    {
        string DefaultSourceFolder { get; set; }
        string DefaultTargetFolder { get; set; }
        string FolderForHistory { get; set; }
        bool IsUseFillter { get; set; }
        bool IsUseIgnoreFillter { get; set; }
        IList<string> IgnorableFileFormat { get; set; }
        IList<string> FilteredFileFormat { get; set; }
    }
}