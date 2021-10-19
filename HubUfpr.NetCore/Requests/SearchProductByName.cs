using System;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class SearchProductByName
    {

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Include)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "ReturnActiveOnly", NullValueHandling = NullValueHandling.Include)]
        public bool ReturnActiveOnly { get; set; }

    }
}
