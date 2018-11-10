namespace FolderSyns.Interfaces
{
    using System;

    public interface ILogManager
    {
        void SaveError(Exception ex);
    }
}