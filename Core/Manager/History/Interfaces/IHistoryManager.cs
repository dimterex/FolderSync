using System;
using System.Collections.Generic;
using Core.Model.History;

namespace Core.Manager.History.Interfaces
{
    public interface IHistoryManager
    {
        IList<HistoryObjectModel> HistoryObjectModels { get; }

        event EventHandler<HistoryObjectModel> ObjectAddedEvent;
    }
}