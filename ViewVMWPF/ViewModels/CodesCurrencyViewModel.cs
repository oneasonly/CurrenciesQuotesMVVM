using Model.SerializationJSON;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewVMWPF.ViewModels
{
    public class CodesCurrencyViewModel : BindableBase
    {
        #region ctor
        public CodesCurrencyViewModel()
        {
            SelectionChangedCommand = new DelegateCommand(SelectionChanged);
        }
        #endregion
        #region events
        public event Action CodeSelectionChangedEvent = () => { };
        #endregion

        #region fields
        private ObservableCollection<CbrValute> _sourceValutes = new ObservableCollection<CbrValute>();
        private ObservableCollection<CbrValute> _selectedValutes = new ObservableCollection<CbrValute>();
        #endregion

        #region Properties
        public ObservableCollection<CbrValute> SourceValutes
        {
            get => _sourceValutes;
            set => SetProperty(ref _sourceValutes, value);
        }
        public ObservableCollection<CbrValute> SelectedValutes
        {
            get => _selectedValutes;
            set => SetProperty(ref _selectedValutes, value);
        }
        #endregion

        #region Commands Delegate
        public DelegateCommand SelectionChangedCommand { get; }
        #endregion


        #region private Methods
        private void SelectionChanged()
        {
            CodeSelectionChangedEvent();
        }
        #endregion
    }
}
