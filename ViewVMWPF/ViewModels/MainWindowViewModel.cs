using Model;
using Model.SerializationJSON;
using Prism.Commands;
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
    /// Main Parent VM for managing: child VMs and common properties for multiple VMs    
    public class MainWindowViewModel : BindableBase
    {
        #region ctor
        public MainWindowViewModel()
        {
            VMCodesCurrency = new CodesCurrencyViewModel();
            VMExchangeRate = new ExchangeRatesViewModel();
            VMSearchCurrency = new SearchCurrencyViewModel();
            VMConversion = new ConversionViewModel();
            VMCodesCurrency.CodeSelectionChangedEvent += OnSelectedValutesChanged;
            #region Commands
            UpdateCommand = new DelegateCommand(async () => await Update());
            SelectedCodeToQuotesCommand = new DelegateCommand(SetSelectedCodesToQuotes);
            Test1Command = new DelegateCommand(Test1Method);
            #endregion            
        }
        #endregion

        #region fields
        private ModelCurrencies _model = new ModelCurrencies();
        private Cbr _responseRoot = new Cbr();
        #endregion

        #region Properties
        public CodesCurrencyViewModel VMCodesCurrency { get; }
        public ExchangeRatesViewModel VMExchangeRate { get; }
        public SearchCurrencyViewModel VMSearchCurrency { get; }
        public ConversionViewModel VMConversion { get; }

        public Cbr ResponseRoot
        {
            get => _responseRoot;
            set => SetProperty(ref _responseRoot, value);
        }

        //public ObservableCollection<string> SelectedCodes => 
        public ObservableCollection<CbrValute> SelectedValutes => VMCodesCurrency.SelectedValutes;
        public ObservableCollection<CbrValute> AllValutes => new ObservableCollection<CbrValute>(ResponseRoot?.Valute?.Values.ToList());
        #endregion

        #region Commands Delegates
        public DelegateCommand UpdateCommand { get; }
        public DelegateCommand SelectedCodeToQuotesCommand { get; }
        public DelegateCommand Test1Command { get; }
        #endregion

        #region Commands Methods
        private async Task Update()
        {
            await GetRootResponse();
            SetAllValutesFromResponse();
            //SetSelectedCodes();
        }
        private void Test1Method()
        {
            SetSelectedCodesToQuotes();
        }
        #endregion

        #region private Methods
        private async Task GetRootResponse()
        {
            ResponseRoot = await _model.GetResponseJson();
        }
        private void SetAllValutesFromResponse()
        {
            VMCodesCurrency.SourceValutes = AllValutes;
            OnAllAndSelectedValutesChanged();
        }
        private void OnSelectedValutesChanged()
        {
            SetSelectedCodesToQuotes();
            OnAllAndSelectedValutesChanged();
        }
        private void OnAllAndSelectedValutesChanged()
        {
            VMConversion.ItemsSource = new ObservableCollection<CbrValute>(SelectedValutes.Union(AllValutes));
        }
        private void SetSelectedCodesToQuotes()
        {
            var quotesList = new ObservableCollection<Quotes>();
            foreach (CbrValute item in SelectedValutes)
            {
                //QuotLine
                Quotes quot = new Quotes("rub");
                quot.MainCurrency = item;
                quot.RelativeCurrencies = SelectedValutes;
                foreach (CbrValute item2 in SelectedValutes)
                {
                    if(item != item2)
                    quot.QuotesList.Add(new PairValute(item, item2));
                }
                quotesList.Add(quot);
            }
            VMExchangeRate.SourceQuotes = quotesList;
        }
        #endregion
    }
}
