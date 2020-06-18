using Model.SerializationJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewVMWPF.Classes
{
    public class PairValute
    {
        #region ctor
        public PairValute() { }
        public PairValute(CbrValute Valute1, CbrValute Valute2) : this()
        {
            this.Valute1 = Valute1;
            this.Valute2 = Valute2;
        }
        #endregion

        #region private fields & properties
        private bool isEqualNominal => Valute1.Nominal == Valute2.Nominal;
        private string Nominal1 => Valute1.Nominal == 1 || isEqualNominal
            ? string.Empty : Valute1.Nominal.ToString();
        private string Nominal2 => Valute2.Nominal == 1 || isEqualNominal
            ? string.Empty : Valute2.Nominal.ToString();
        #endregion

        #region public Properties
        public CbrValute Valute1 { get; set; }
        public CbrValute Valute2 { get; set; }
        public string NamePair => $"{Nominal1}{Valute1.CharCode} / {Nominal2}{Valute2.CharCode}";
        public decimal ValueRatio => Valute1.Value / Valute2.Value;
        public string NameAndValue => $"{NamePair}  {ValueRatio.ToString("0.####")}";
        #endregion
    }
}
