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
    public class SearchCurrencyViewModel : BindableBase
    {
        #region ctor
        public SearchCurrencyViewModel()
        {
            SearchCommand = new DelegateCommand(OnSearchCommand);
        }
        #endregion

        #region fields
        private string _inputSearch = string.Empty;
        private List<CbrValute> _preferedCurrencies = new List<CbrValute>();
        private ObservableCollection<CbrValute> _allValues = new ObservableCollection<CbrValute>();
        private ObservableCollection<PairValute> _searchQuotesResult = new ObservableCollection<PairValute>();
        #endregion

        #region properties
        public string InputSearch
        {
            get => _inputSearch;
            set
            {
                SetProperty(ref _inputSearch, value);
                SearchCommand.Execute();
            }
        }
        public List<CbrValute> PreferedCurrenies
        {
            get => _preferedCurrencies;
            set => SetProperty(ref _preferedCurrencies, value);
        }
        public ObservableCollection<CbrValute> AllValutes
        {
            get => _allValues;
            set => SetProperty(ref _allValues, value);
        }
        public ObservableCollection<PairValute> SearchQuotesResult
        {
            get => _searchQuotesResult;
            set => SetProperty(ref _searchQuotesResult, value);
        }
        #endregion

        public DelegateCommand SearchCommand { get; }

        #region Methods
        private void OnSearchCommand()
        {
            int code;
            bool isNumber = int.TryParse(InputSearch, out code);
            CbrValute searchResult = null;

            if(isNumber)
            {
                searchResult = AllValutes.Where(x => x.NumCode == code).FirstOrDefault();
            }
            else
            {
                var matches = AllValutes.Where(x => x.Name.ToLower().Contains(InputSearch?.ToLower() ?? string.Empty));
                if(matches.Count()==1)
                {
                    searchResult = matches.FirstOrDefault();
                }
            }
            BuildQuotesResult(searchResult);
        }

        private void BuildQuotesResult(CbrValute searchResult)
        {
            SearchQuotesResult.Clear();
            if (searchResult != null)
            {
                foreach (CbrValute item in PreferedCurrenies)
                {
                    SearchQuotesResult.Add(new PairValute(searchResult, item));
                }
            }
        }
        #endregion
    }
}
