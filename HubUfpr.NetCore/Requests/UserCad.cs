using System;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class UserCad
    {
        [JsonProperty(PropertyName = "usuario", NullValueHandling = NullValueHandling.Ignore)]
        public string usuario { get; set; }

        [JsonProperty(PropertyName = "senha", NullValueHandling = NullValueHandling.Ignore)]
        public string senha { get; set; }

        [JsonProperty(PropertyName = "confirmacaoSenha", NullValueHandling = NullValueHandling.Ignore)]
        public string confirmacaoSenha { get; set; }
    }
}
