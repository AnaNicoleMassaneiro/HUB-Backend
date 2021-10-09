using System;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class InsertProduto
    {
        [JsonProperty(PropertyName = "nome", NullValueHandling = NullValueHandling.Ignore)]
        public string nome { get; set; }

        [JsonProperty(PropertyName = "isAtivo", NullValueHandling = NullValueHandling.Ignore)]
        public bool isAtivo { get; set; }

        [JsonProperty(PropertyName = "preco", NullValueHandling = NullValueHandling.Ignore)]
        public float preco { get; set; }
        
        [JsonProperty(PropertyName = "descricao", NullValueHandling = NullValueHandling.Ignore)]
        public string descricao { get; set; }

        [JsonProperty(PropertyName = "quantidadeDisponivel", NullValueHandling = NullValueHandling.Ignore)]
        public int quantidadeDisponivel { get; set; }

        [JsonProperty(PropertyName = "idVendedor", NullValueHandling = NullValueHandling.Ignore)]
        public int idVendedor { get; set; }

    }
}
