using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class CreateReserve
    {
        [JsonProperty(PropertyName = "idCliente", NullValueHandling = NullValueHandling.Include)]
        public int IdCliente { get; set; }

        [JsonProperty(PropertyName = "idProduto", NullValueHandling = NullValueHandling.Include)]
        public int IdProduto { get; set; }

        [JsonProperty(PropertyName = "quantidade", NullValueHandling = NullValueHandling.Include)]
        public int QuantidadeDesejada { get; set; }

        [JsonProperty(PropertyName = "latitude", NullValueHandling = NullValueHandling.Include)]
        public float Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude", NullValueHandling = NullValueHandling.Include)]
        public float Longitude { get; set; }
    }
}
