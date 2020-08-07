using System;
using System.Collections.Generic;
using System.IO;
using Core.Dal.Interfaces;
using Core.Manager.Event;
using Core.Manager.Event.Interfaces;
using Core.Manager.History.Interfaces;
using Core.Manager.Settings.Interface;
using Core.Model.History;

namespace Core.Manager.History
{
    public class HistoryManager : IHistoryManager
    {
        public event EventHandler<HistoryObjectModel> ObjectAddedEvent;

        public IList<HistoryObjectModel> HistoryObjectModels { get; }

        #region Fields

        private readonly ISettingsManager _settingsManager;
        private readonly IHistoryRepository _historyRepository;
        private readonly string _filePath;

        private readonly IList<HistoryObjectModel> _sessionHistoryObjectModels;

        #endregion Fields

        #region Constuctors

        public HistoryManager(ISettingsManager settingsManager, IHistoryRepository historyRepository,
            IEventManager eventManager)
        {
            _settingsManager = settingsManager;
            _historyRepository = historyRepository;

            eventManager.ActionCompleted += EventManagerOnActionCompleted;
            eventManager.SaveEvent += EventManagerOnSaveEvent;

            _filePath = _settingsManager.SettingsModel.FolderForHistory;
            HistoryObjectModels = _historyRepository.GetModels(_filePath);
            _sessionHistoryObjectModels = new List<HistoryObjectModel>();
        }

        private void EventManagerOnSaveEvent(object sender, EventArgs e)
        {
            _historyRepository.SaveNewEvents(_filePath, _sessionHistoryObjectModels);
        }

        private void EventManagerOnActionCompleted(object sender, EventModel item)
        {
            var model = new HistoryObjectModel(item.FileName, item.SourcePath, item.TargetPath);
            model.FileActions = item.FileActions;
            model.DateTime = DateTime.UtcNow;

            ObjectAddedEvent?.Invoke(this, model);
            _sessionHistoryObjectModels.Add(model);
        }

        #endregion Constuctors
    }
}