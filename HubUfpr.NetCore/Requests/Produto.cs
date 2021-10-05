using System;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class Produto
    {
        [JsonProperty(PropertyName = "idProduto", NullValueHandling = NullValueHandling.Ignore)]
        public int idProduto { get; set; }

        [JsonProperty(PropertyName = "nome", NullValueHandling = NullValueHandling.Ignore)]
        public string nome { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public bool status { get; set; }

        [JsonProperty(PropertyName = "preco", NullValueHandling = NullValueHandling.Ignore)]
        public float preco { get; set; }
        
        [JsonProperty(PropertyName = "notaProduto", NullValueHandling = NullValueHandling.Ignore)]
        public string notaProduto { get; set; }

        [JsonProperty(PropertyName = "descricao", NullValueHandling = NullValueHandling.Ignore)]
        public string descricao { get; set; }

        [JsonProperty(PropertyName = "quantidadeDisponivel", NullValueHandling = NullValueHandling.Ignore)]
        public int quantidadeDisponivel { get; set; }

        [JsonProperty(PropertyName = "idVendedor", NullValueHandling = NullValueHandling.Ignore)]
        public int idVendedor { get; set; }

    }
}
