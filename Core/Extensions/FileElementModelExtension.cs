using System.Runtime.CompilerServices;
using Core.Enum;
using Core.Model.File;

namespace Core.Extensions
{
    public static class FileElementModelExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static void AddFlags(this FileElementModel model, FileActions added)
        {
            model.FileActions = model.FileActions |= added;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static void RemoveFlags(this FileElementModel model, FileActions removed)
        {
            model.FileActions = model.FileActions &= ~ removed;
        }
    }
}