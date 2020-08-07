using System.Collections.Generic;
using Core.Model.History;

namespace Core.Dal.Interfaces
{
    public interface IHistoryRepository
    {
        List<HistoryObjectModel> GetModels(string path);

        void SaveNewEvents(string filePath, IList<HistoryObjectModel> obj);
    }
}