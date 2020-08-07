using System.Collections.ObjectModel;
using System.Windows.Data;
using Core.Manager.History.Interfaces;
using Core.Model.History;
using Prism.Mvvm;

namespace UI.ViewModel.History
{
    public class HistoryWindowViewModel : BindableBase
    {
        private readonly object _bindingLockObject;
        private readonly IHistoryManager _historyManager;

        public HistoryWindowViewModel(IHistoryManager historyManager)
        {
            _historyManager = historyManager;

            FileActions = new ObservableCollection<HistoryElementViewModel>();
            _bindingLockObject = new object();
            BindingOperations.EnableCollectionSynchronization(FileActions, _bindingLockObject);

            foreach (var historyObjectModel in _historyManager.HistoryObjectModels)
                FileActions.Add(new HistoryElementViewModel(historyObjectModel));

            _historyManager.ObjectAddedEvent += HistoryManagerOnObjectAddedEvent;
        }

        #region Properties

        public ObservableCollection<HistoryElementViewModel> FileActions { get; }

        #endregion Properties

        private void HistoryManagerOnObjectAddedEvent(object sender, HistoryObjectModel historyObjectModel)
        {
            var viewModel = new HistoryElementViewModel(historyObjectModel);
            FileActions.Add(viewModel);
        }
    }
}