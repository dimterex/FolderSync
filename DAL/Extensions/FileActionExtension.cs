using System.Runtime.CompilerServices;
using DAL.History;

namespace DAL.Extensions
{
    internal static class FileActionExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Core.Enum.FileActions Convert(this FileActions fileActions)
        {
            var fileAction = Core.Enum.FileActions.Not;

            if (fileActions.HasFlag(FileActions.Not))
                fileAction = fileAction | Core.Enum.FileActions.Not; 
            
            if (fileActions.HasFlag(FileActions.Copy))
                fileAction = fileAction | Core.Enum.FileActions.Copy;
            
            if (fileActions.HasFlag(FileActions.Delete))
                fileAction = fileAction | Core.Enum.FileActions.Delete;
            
            return fileAction;
        }
    }
}