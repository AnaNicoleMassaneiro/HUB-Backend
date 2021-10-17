using System;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class SearchProduto
    {
        [JsonProperty(PropertyName = "idProduto", NullValueHandling = NullValueHandling.Include)]
        public int idProduto { get; set; }

        [JsonProperty(PropertyName = "nome", NullValueHandling = NullValueHandling.Include)]
        public string nome { get; set; }

        [JsonProperty(PropertyName = "idVendedor", NullValueHandling = NullValueHandling.Include)]
        public int idVendedor { get; set; }

    }
}
