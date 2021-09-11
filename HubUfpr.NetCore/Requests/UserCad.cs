using System;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class UserCad
    {
        [JsonProperty(PropertyName = "nome", NullValueHandling = NullValueHandling.Ignore)]
        public string nome { get; set; }

        [JsonProperty(PropertyName = "isVendedor", NullValueHandling = NullValueHandling.Include)]
        public bool isVendedor { get; set; }

        [JsonProperty(PropertyName = "senha", NullValueHandling = NullValueHandling.Ignore)]
        public string senha { get; set; }

        [JsonProperty(PropertyName = "confirmacaoSenha", NullValueHandling = NullValueHandling.Ignore)]
        public string confirmacaoSenha { get; set; }

        [JsonProperty(PropertyName = "grr", NullValueHandling = NullValueHandling.Ignore)]
        public string grr { get; set; }

        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public string email { get; set; }
    }
}
