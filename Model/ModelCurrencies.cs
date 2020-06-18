using Model.SerializationJSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ModelCurrencies
    {
        #region const/readonly
        private const string url = @"https://www.cbr-xml-daily.ru/daily_json.js";
        private readonly List<int> _preferedCurrencyCodes = new List<int>() { 840 };
        private readonly CbrValute _defaultCurrency = new CbrValute
        {
            CharCode = "RUB",
            NumCode = 643,
            ID = "empty",
            Nominal = 1,
            Name = "Российский рубль",
            Value = 1,
            Previous = 1
        };
        #endregion

        #region Properties
        public List<int> PreferedCurrencyCodes => _preferedCurrencyCodes;
        public CbrValute DefaultCurrency => _defaultCurrency;
        #endregion

        #region public Methods
        public async Task<Cbr> GetResponseJsonWithDefault()
        {
            var item = await GetResponseJson();
            item.Valute.Add(item.Valute.Count.ToString(), DefaultCurrency);
            return item;
        }
        public async Task<Cbr> GetResponseJson()
        {
            string ResponseString = await GetResponseString();
            Cbr currencies = await Task.Run(()=>JsonConvert.DeserializeObject<Cbr>(ResponseString));
            return currencies;
        }

        public async Task<string> GetResponseString()
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
        #endregion
    }
}
