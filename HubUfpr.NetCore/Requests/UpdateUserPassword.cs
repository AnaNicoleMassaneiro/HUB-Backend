using System;
using Newtonsoft.Json;

namespace HubUfpr.API.Requests
{
    public class UpdateUserPassword
    {
        [JsonProperty(PropertyName = "password", NullValueHandling = NullValueHandling.Include)]
        public string NewPassword { get; set; }

        [JsonProperty(PropertyName = "confirmPassword", NullValueHandling = NullValueHandling.Include)]
        public string ConfirmNewPassword { get; set; }
    }
}
