using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class FavoriteSeller
    {
        [JsonProperty(PropertyName = "idCliente", NullValueHandling = NullValueHandling.Include)]
        public int IdCliente { get; set; }

        [JsonProperty(PropertyName = "idVendedor", NullValueHandling = NullValueHandling.Include)]
        public int IdVendedor { get; set; }
    }
}
