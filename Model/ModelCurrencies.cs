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
        private string url = @"https://www.cbr-xml-daily.ru/daily_json.js";
        private readonly List<int> _predefinedCurrencies = new List<int>() { 840 };
        public async Task<Cbr> GetResponseJson()
        {
            string ResponseString = await GetResponseString();
            Cbr currencies = await Task.Run(()=>JsonConvert.DeserializeObject<Cbr>(ResponseString));
            var select = currencies.Valute.Values.Select(x => $"{x.NumCode} {x.CharCode}").ToList();
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

        public List<int> PredefinedCurrencies => _predefinedCurrencies;
    }
}
