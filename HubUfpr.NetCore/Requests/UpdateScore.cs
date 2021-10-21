using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class UpdateScore
    {
        [JsonProperty(PropertyName = "score", NullValueHandling = NullValueHandling.Include)]
        public float Score { get; set; }
    }
}
