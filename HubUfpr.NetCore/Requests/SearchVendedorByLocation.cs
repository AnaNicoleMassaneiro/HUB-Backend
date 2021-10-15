using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class SearchVendedorByLocation
    {
        [JsonProperty(PropertyName = "longitude", NullValueHandling = NullValueHandling.Ignore)]
        public float Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude", NullValueHandling = NullValueHandling.Ignore)]
        public float Latitude { get; set; }
    }
}