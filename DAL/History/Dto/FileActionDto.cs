using System;
using Core.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.History.Dto
{
    internal class FileActionDto
    {
        [JsonProperty(PropertyName = "actions")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FileActions FileActions { get; set; }
        
        [JsonProperty(PropertyName = "file_name")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "target_folder")]
        public string NewFolder { get; set; }

        [JsonProperty(PropertyName = "source_folder")]
        public string OldFolder { get; set; }

        [JsonProperty(PropertyName = "time_stamp")]
        public DateTime DateTime { get; set; }
    }
}