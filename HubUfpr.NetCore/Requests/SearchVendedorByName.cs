using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class SearchVendedorByName
    {
        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

    }
}