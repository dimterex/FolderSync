using Core.Enum;

namespace Core.Manager.Event
{
    public class EventModel
    {
        public EventModel(string itemPath, string source, string target, FileActions fileActions)
        {
            FileName = itemPath;
            SourcePath = source;
            TargetPath = target;
            FileActions = fileActions;
        }

        public string FileName { get; }
        public string SourcePath { get; }
        public string TargetPath { get; }
        public FileActions FileActions { get; }
    }
}