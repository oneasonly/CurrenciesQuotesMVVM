using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SerializationJSON
{

    public class Cbr
    {
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get; set; }
        public string PreviousURL { get; set; }
        public DateTime Timestamp { get; set; }
        [JsonProperty("Valute")]
        public Dictionary<string, CbrValute> Valute { get; set; }
    }
}
