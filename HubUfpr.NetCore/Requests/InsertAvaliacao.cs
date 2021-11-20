using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class InsertAvaliacao
    {
        [JsonProperty(PropertyName = "idProduto", NullValueHandling = NullValueHandling.Include)]
        public int IdProduto { get; set; }

        [JsonProperty(PropertyName = "idCliente", NullValueHandling = NullValueHandling.Include)]
        public int IdCliente { get; set; }
        
        [JsonProperty(PropertyName = "idVendedor", NullValueHandling = NullValueHandling.Include)]
        public int IdVendedor { get; set; }

        [JsonProperty(PropertyName = "nota", NullValueHandling = NullValueHandling.Include)]
        public int Nota{ get; set; }

        [JsonProperty(PropertyName = "titulo", NullValueHandling = NullValueHandling.Include)]
        public string Titulo { get; set; }

        [JsonProperty(PropertyName = "descricao", NullValueHandling = NullValueHandling.Include)]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "tipoAvaliacao", NullValueHandling = NullValueHandling.Include)]
        public int TipoAvaliacao { get; set; }
    }
}
