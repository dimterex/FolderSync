namespace FolderSyns.Interfaces
{
    public interface IConfigManager
    {
        /// <summary>
        /// Загрузить значение по ключу.
        /// </summary>
        /// <param name="property">Ключ.</param>
        T LoadSetting<T>(string property);

        /// <summary>
        /// Сохранить значение.
        /// </summary>
        /// <param name="property">Ключ.</param>
        /// <param name="path">Значение.</param>
        void SaveFolderForHistory<T>(string property, T path);
    }
}