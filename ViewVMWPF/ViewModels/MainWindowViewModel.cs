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
            SelectedCodeToQuotesCommand = new DelegateCommand(SetVmExchangeRateWithSelectedValutes);
            #endregion            
        }
        #endregion

        #region fields
        private ModelCurrencies _model = new ModelCurrencies();
        private Cbr _responseRoot = new Cbr();
        ObservableCollection<CbrValute> _allValutes = new ObservableCollection<CbrValute>();
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
        public ObservableCollection<CbrValute> AllValutes
        {
            get => _allValutes;
            set => SetProperty(ref _allValutes, value);
        }
        public ObservableCollection<CbrValute> SelectedValutes => VMCodesCurrency.SelectedValutes;
        public List<CbrValute> PreferedCurrenies
        {
            get => _preferedCurrencies;
            set => SetProperty(ref _preferedCurrencies, value);
        }
        #endregion

        #region Commands Delegates
        public DelegateCommand UpdateCommand { get; }
        public DelegateCommand SelectedCodeToQuotesCommand { get; }
        #endregion

        #region Methods
        private async Task Update()
        {
            await GetRootResponse();
            AllValutes = GetAllValutes();

            BuildPreferedCurrencies();
            SetupViewmodelsWithDataSources();
            SetVmExchangeRateWithSelectedValutes();
            SetVmConversionPreferedSortedValuteList();            
        }
        private async Task GetRootResponse()
        {
            ResponseRoot = await _model.GetResponseJsonWithDefault();
        }
        private ObservableCollection<CbrValute> GetAllValutes()
        {
            var list = ResponseRoot?.Valute?.Values;
            return new ObservableCollection<CbrValute>(list?.ToList() ?? new List<CbrValute>());
        }
        private ObservableCollection<CbrValute> GetPreferedSelectedValutes()
        {
            return new ObservableCollection<CbrValute>(PreferedCurrenies?.Union(SelectedValutes));
        }
        private void BuildPreferedCurrencies()
        {
            PreferedCurrenies.Clear();
            PreferedCurrenies.Add(_model.DefaultCurrency);
            var preferedRange = AllValutes.Where(x => _model.PreferedCurrencyCodes.Contains(x.NumCode));
            PreferedCurrenies.AddRange(preferedRange);
        }
        private void SetupViewmodelsWithDataSources()
        {
            VMCodesCurrency.SourceValutes = AllValutes;
            VMSearchCurrency.AllValutes = AllValutes;
            VMSearchCurrency.PreferedCurrenies = PreferedCurrenies;                                    
        }
        private void OnSelectedValutesChanged()
        {
            SetVmExchangeRateWithSelectedValutes();
            SetVmConversionPreferedSortedValuteList();
        }
        private void SetVmConversionPreferedSortedValuteList()
        {
            var unionList = GetPreferedSelectedValutes().Union(AllValutes);
            VMConversion.ItemsSource = new ObservableCollection<CbrValute>(unionList);            
        }
        private void SetVmExchangeRateWithSelectedValutes()
        {
            var quotesList = new ObservableCollection<Quotes>();
            foreach (CbrValute item in GetPreferedSelectedValutes())
            {
                Quotes quot = new Quotes();
                quot.MainCurrency = item;
                foreach (CbrValute item2 in GetPreferedSelectedValutes())
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
