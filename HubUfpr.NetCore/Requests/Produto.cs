using System;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class Produto
    {
        [JsonProperty(PropertyName = "nome", NullValueHandling = NullValueHandling.Ignore)]
        public string nome { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string status { get; set; }

        [JsonProperty(PropertyName = "preco", NullValueHandling = NullValueHandling.Ignore)]
        public float preco { get; set; }
        
        [JsonProperty(PropertyName = "notaProduto", NullValueHandling = NullValueHandling.Ignore)]
        public string notaProduto { get; set; }

        [JsonProperty(PropertyName = "descricao", NullValueHandling = NullValueHandling.Ignore)]
        public string descricao { get; set; }

        [JsonProperty(PropertyName = "qtdProdutosDisponiveis", NullValueHandling = NullValueHandling.Ignore)]
        public int qtdProdutosDisponiveis { get; set; }
    }
}
