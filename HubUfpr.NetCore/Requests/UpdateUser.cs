using System;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class UpdateUser
    {
        [JsonProperty(PropertyName = "nome", NullValueHandling = NullValueHandling.Include)]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "telefone", NullValueHandling = NullValueHandling.Include)]
        public string Telefone { get; set; }
    }
}
