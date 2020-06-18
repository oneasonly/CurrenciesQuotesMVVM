using Model.SerializationJSON;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewVMWPF.Classes;

namespace ViewVMWPF.ViewModels
{
    public class ExchangeRatesViewModel : BindableBase
    {
        #region ctor
        public ExchangeRatesViewModel()
        {
        }
        #endregion

        #region private fields
        private ObservableCollection<CbrValute> _sourceCurrencies = new ObservableCollection<CbrValute>();
        private ObservableCollection<Quotes> _sourceQuotes = new ObservableCollection<Quotes>();
        #endregion

        #region Properties
        public ObservableCollection<CbrValute> SourceCurrencies
        {
            get => _sourceCurrencies;
            set => SetProperty(ref _sourceCurrencies, value);
        }
        public ObservableCollection<Quotes> SourceQuotes
        {
            get => _sourceQuotes;
            set => SetProperty(ref _sourceQuotes, value);
        }
        #endregion
    }
}
