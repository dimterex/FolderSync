using System;
using Core.Manager.Event.Interfaces;

namespace Core.Manager.Event
{
    public class EventManager : IEventManager
    {
        public event EventHandler<string> CopyActionStarted;
        public event EventHandler<string> RemoveActionStarted;
        public event EventHandler<EventModel> ActionCompleted;
        public event EventHandler SaveEvent;

        public void RaiseStartCopyAction(string itemPath)
        {
            CopyActionStarted?.Invoke(this, itemPath);
        }

        public void RaiseStartRemoveAction(string itemPath)
        {
            RemoveActionStarted?.Invoke(this, itemPath);
        }

        public void RaiseActionCompleted(EventModel eventModel)
        {
            ActionCompleted?.Invoke(this, eventModel);
        }

        public void RaiseSaveEvent()
        {
            SaveEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}