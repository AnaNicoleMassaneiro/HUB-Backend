using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class SellerStatus
    {
        [JsonProperty(PropertyName = "idVendedor", NullValueHandling = NullValueHandling.Include)]
        public int? IdVendedor { get; set; }

        [JsonProperty(PropertyName = "isOpen", NullValueHandling = NullValueHandling.Include)]
        public bool? IsOpen { get; set; }

        [JsonProperty(PropertyName = "isAtivo", NullValueHandling = NullValueHandling.Include)]
        public bool? IsAtivo { get; set; }
    }
}