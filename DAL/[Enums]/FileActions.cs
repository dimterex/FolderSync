using System;
using System.Runtime.Serialization;

namespace DAL
{
    [Flags]
    internal enum FileActions : byte
    {
        [EnumMember(Value = "not")]
        Not = 0,
        
        [EnumMember(Value = "copy")]
        Copy = 1 << 0,
        
        [EnumMember(Value = "delete")]
        Delete = 1 << 1,
    }
}