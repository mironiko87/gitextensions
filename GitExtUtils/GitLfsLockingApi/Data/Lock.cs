using Newtonsoft.Json;
using System;

namespace GitLfsApi.Data
{
    public class Lock
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("locked_at")]
        public DateTime LockedAt { get; set; }
        [JsonProperty("owner")]
        public Owner Owner { get; set; }
    }
}
