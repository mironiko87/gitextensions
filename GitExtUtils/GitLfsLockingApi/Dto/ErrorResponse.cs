using System;
using Newtonsoft.Json;

namespace GitLfsApi.Dto
{
    public class ErrorResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("documentation_url")]
        public Uri DocumentationUrl { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }
}
