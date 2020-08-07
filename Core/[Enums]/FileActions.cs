using System;

namespace Core.Enum
{
    [Flags]
    public enum FileActions
    {
        Not,
        Copy,
        Delete,
    }
}