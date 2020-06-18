using Model.SerializationJSON;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewVMWPF.ViewModels
{
    public class SearchCurrencyViewModel : BindableBase
    {
        #region fields
        private string _inputSearch = string.Empty;
        private List<CbrValute> _preferedCurrencies = new List<CbrValute>();
        #endregion
        public SearchCurrencyViewModel()
        {
            SearchCommand = new DelegateCommand(OnSearchCommand);
        }
        public string InputSearch
        {
            get => _inputSearch;
            set => SetProperty(ref _inputSearch, value);
        }
        public List<CbrValute> PreferedCurrenies
        {
            get => _preferedCurrencies;
            set => SetProperty(ref _preferedCurrencies, value);
        }
        DelegateCommand SearchCommand;
        private void OnSearchCommand()
        {
            int code;
            bool isNumber = int.TryParse(InputSearch, out code);
            if(isNumber)
            {

            }

        }
    }
}
