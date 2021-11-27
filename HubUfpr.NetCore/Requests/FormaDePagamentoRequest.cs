using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class FormaDePagamentoRequest
    {
        [JsonProperty(PropertyName = "idFormaDePagamento", NullValueHandling = NullValueHandling.Include)]
        public int IdFormaDePagamento { get; set; }

        [JsonProperty(PropertyName = "idVendedor", NullValueHandling = NullValueHandling.Include)]
        public int IdVendedor { get; set; }
    }
}
