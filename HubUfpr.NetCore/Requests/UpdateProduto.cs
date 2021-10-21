using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class UpdateProduto
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

        public IFormFile ProductImage { get; set; }
    }
}
