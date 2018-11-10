namespace FolderSyns.Interfaces
{
    public interface IHistoryManager
    {
        T DeSerializeObject<T>();
        void SerializeObject<T>(T serializableObject);
    }
}