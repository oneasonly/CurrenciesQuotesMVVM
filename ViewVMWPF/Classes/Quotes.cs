using Model.SerializationJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewVMWPF.Classes
{
    public class Quotes
    {
        public Quotes(string CharCodeDefaultValute)
        {
            string _charCodeDefaultValute = CharCodeDefaultValute?.ToUpper();
        }
        private string _charCodeDefaultValute;
        public CbrValute MainCurrency { get; set; }
        public ObservableCollection<CbrValute> RelativeCurrencies { get; set; } = new ObservableCollection<CbrValute>();
        public ObservableCollection<PairValute> QuotesList { get; set; } = new ObservableCollection<PairValute>();

        public string Name => MainCurrency.CharCode;


        void hz()
        {
            //RelativeCurrencies.ForEach(x=>x.)
        }
    }
}
