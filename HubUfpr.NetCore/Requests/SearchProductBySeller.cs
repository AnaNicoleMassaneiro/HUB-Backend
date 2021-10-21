using System;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class SearchProductBySeller
    {

        [JsonProperty(PropertyName = "idVendedor", NullValueHandling = NullValueHandling.Include)]
        public int SellerId { get; set; }

        [JsonProperty(PropertyName = "ReturnActiveOnly", NullValueHandling = NullValueHandling.Include)]
        public bool ReturnActiveOnly { get; set; }

    }
}
