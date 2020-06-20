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
    public class ConversionViewModel : BindableBase
    {
        #region ctor
        public ConversionViewModel()
        {

        }        
        #endregion

        #region fields
        private CbrValute _selected1 = new CbrValute();
        private CbrValute _selected2 = new CbrValute();
        private decimal _currencyAmount1;
        private decimal _currencyAmount2;
        private ObservableCollection<CbrValute> _itemsSource = new ObservableCollection<CbrValute>();
        #endregion

        #region Properties
        public CbrValute Selected1
        {
            get => _selected1;
            set => SetProperty(ref _selected1, value);
        }
        public CbrValute Selected2
        {
            get => _selected2;
            set => SetProperty(ref _selected2, value);
        }
        public decimal CurrencyAmount1
        {
            get => _currencyAmount1;
            set
            {
                SetProperty(ref _currencyAmount1, value);
                SetInputbox2();
            }
        }
        public decimal CurrencyAmount2
        {
            get => _currencyAmount2;
            set
            {
                SetProperty(ref _currencyAmount2, value);
                SetInputbox1();
            }
        }

        public ObservableCollection<CbrValute> ItemsSource
        {
            get => _itemsSource;
            set => SetProperty(ref _itemsSource, value);
        }
        #endregion

        #region Methods
        private void SetInputbox2()
        {
            decimal divisor = Selected2.Nominal * Selected2.Value;
            if (divisor == 0) return;
            decimal val1 = CurrencyAmount1 * Selected1.Nominal * Selected1.Value;
            decimal val2 = val1 / divisor;
            if (val2 != CurrencyAmount2)
            {
                CurrencyAmount2 = val2;
            }
        }

        private void SetInputbox1()
        {
            decimal divisor = Selected1.Nominal * Selected1.Value;
            if (divisor == 0) return;
            decimal val2 = CurrencyAmount2 * Selected2.Nominal * Selected2.Value;
            decimal val1 = val2 / divisor;
            if (val1 != CurrencyAmount1)
            {
                CurrencyAmount1 = val1;
            }
        }
        #endregion
    }
}
