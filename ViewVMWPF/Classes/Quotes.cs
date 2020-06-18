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

        public Quotes()
        {
        }
        public CbrValute MainCurrency { get; set; }

        public ObservableCollection<PairValute> QuotesList { get; set; } = new ObservableCollection<PairValute>();

        public string Name => MainCurrency.CharCode;


    }
}
