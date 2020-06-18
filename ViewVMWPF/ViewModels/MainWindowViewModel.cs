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
        private List<CbrValute> _preferedCurrencies = new List<CbrValute>();
        #endregion

        #region child ViewModels
        public CodesCurrencyViewModel VMCodesCurrency { get; }
        public ExchangeRatesViewModel VMExchangeRate { get; }
        public SearchCurrencyViewModel VMSearchCurrency { get; }
        public ConversionViewModel VMConversion { get; }
        #endregion

        #region Properties
        public Cbr ResponseRoot
        {
            get => _responseRoot;
            set => SetProperty(ref _responseRoot, value);
        }

        //public ObservableCollection<string> SelectedCodes => 
        public ObservableCollection<CbrValute> SelectedValutes => VMCodesCurrency.SelectedValutes;
        public ObservableCollection<CbrValute> AllValutes => new ObservableCollection<CbrValute>(ResponseRoot?.Valute?.Values.ToList());
        public List<CbrValute> PreferedCurrenies
        {
            get => _preferedCurrencies;
            set => SetProperty(ref _preferedCurrencies, value);
        }
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
            ResponseRoot = await _model.GetResponseJsonWithDefault();
            BuildPreferedCurrencies();
        }

        private void BuildPreferedCurrencies()
        {
            PreferedCurrenies.Clear();
            PreferedCurrenies.Add(_model.DefaultCurrency);
            var preferedRange = AllValutes.Where(x => _model.PreferedCurrencyCodes.Contains(x.NumCode));
            PreferedCurrenies.AddRange(preferedRange);

            VMSearchCurrency.PreferedCurrenies = PreferedCurrenies;
        }

        private void SetAllValutesFromResponse()
        {
            VMCodesCurrency.SourceValutes = AllValutes;
            VMSearchCurrency.AllValutes = AllValutes;
            OnAllAndSelectedValutesChanged();
        }
        private void OnSelectedValutesChanged()
        {
            SetSelectedCodesToQuotes();
            OnAllAndSelectedValutesChanged();
        }
        private void OnAllAndSelectedValutesChanged()
        {
            var unionList = PreferedCurrenies.Union(SelectedValutes.Union(AllValutes));
            VMConversion.ItemsSource = new ObservableCollection<CbrValute>(unionList);
            SetSelectedCodesToQuotes();
        }
        private void SetSelectedCodesToQuotes()
        {
            var unionList = PreferedCurrenies.Union(SelectedValutes);
            var quotesList = new ObservableCollection<Quotes>();
            foreach (CbrValute item in unionList)
            {
                //QuotLine
                Quotes quot = new Quotes();
                quot.MainCurrency = item;
                foreach (CbrValute item2 in unionList)
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
