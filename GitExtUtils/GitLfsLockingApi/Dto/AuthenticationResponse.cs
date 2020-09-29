using Newtonsoft.Json;
using System;

namespace GitLfsApi.Dto
{
    class AuthenticationResponse
    {
        public class AuthorizationHeader
        {
            [JsonProperty("authorization")]
            public string Authorization { get; set; }
        }

        [JsonProperty("href")]
        public string Href { get; set; }
        [JsonProperty("header")]
        public AuthorizationHeader Header { get; set; }
        [JsonProperty("expires_at")]
        public DateTime? ExpiresAt { get; set; } = null;
        [JsonProperty("expires_in")]
        public int? ExpiresIn { get; set; } = null;
    }
}
