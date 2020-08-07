using System;

namespace Core.Manager.Event.Interfaces
{
    public interface IEventManager
    {
        event EventHandler<string> CopyActionStarted;
        
        event EventHandler<string> RemoveActionStarted;
        
        event EventHandler<EventModel> ActionCompleted;

        event EventHandler SaveEvent;
        
        void RaiseStartCopyAction(string itemPath);
        void RaiseStartRemoveAction(string itemPath);
        void RaiseActionCompleted(EventModel eventModel);

        void RaiseSaveEvent();
    }
}