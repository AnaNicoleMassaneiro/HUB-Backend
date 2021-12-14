using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class CreateReport
    {
        [JsonProperty(PropertyName = "idVendedor", NullValueHandling = NullValueHandling.Include)]
        public int IdVendedor { get; set; }

        [JsonProperty(PropertyName = "tipo", NullValueHandling = NullValueHandling.Include)]
        public string Tipo{ get; set; }

        [JsonProperty(PropertyName = "dateFilter", NullValueHandling = NullValueHandling.Include)]
        public string DateFilter { get; set; }
    }
}
