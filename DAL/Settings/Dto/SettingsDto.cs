using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DAL.DTO
{
    public class SettingsDto
    {
        [JsonProperty(PropertyName = "FolderForHistory")]
        public string FolderForHistory { get; set; }

        [JsonProperty(PropertyName = "IsUseFilter")]
        public bool IsUseFilter { get; set; }

        [JsonProperty(PropertyName = "IsUseIgnoreFilter")]
        public bool IsUseIgnoreFilter { get; set; }

        [JsonProperty(PropertyName = "DefaultSourceFolder")]
        public string DefaultSourceFolder { get; set; }

        [JsonProperty(PropertyName = "DefaultTargetFolder")]
        public string DefaultTargetFolder { get; set; }

        [JsonProperty(PropertyName = "IgnorableFileFormat")]
        public List<string> IgnorableFileFormat { get; set; }

        [JsonProperty(PropertyName = "FilteredFileFormat")]
        public List<string> FilteredFileFormat { get; set; }
        
        [JsonProperty(PropertyName = "Locale")]
        public string Locale { get; set; }

        public SettingsDto()
        {
            IgnorableFileFormat = new List<string>();
            FilteredFileFormat = new List<string>();
            FolderForHistory = string.Empty;
            DefaultSourceFolder = string.Empty;
            DefaultTargetFolder = string.Empty;
            Locale = string.Empty;
        }
    }
}