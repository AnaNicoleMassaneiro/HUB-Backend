using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class UpdateUserLocation
    {
        [JsonProperty(PropertyName = "longitude", NullValueHandling = NullValueHandling.Include, Required = Required.Always)]
        public float Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude", NullValueHandling = NullValueHandling.Ignore, Required = Required.Always)]
        public float Latitude { get; set; }
    }
}
