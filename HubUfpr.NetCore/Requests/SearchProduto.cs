using System;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class SearchProduto
    {
        [JsonProperty(PropertyName = "idProduto", NullValueHandling = NullValueHandling.Ignore)]
        public int idProduto { get; set; }

        [JsonProperty(PropertyName = "nome", NullValueHandling = NullValueHandling.Ignore)]
        public string nome { get; set; }

        [JsonProperty(PropertyName = "idVendedor", NullValueHandling = NullValueHandling.Ignore)]
        public int idVendedor { get; set; }

    }
}
