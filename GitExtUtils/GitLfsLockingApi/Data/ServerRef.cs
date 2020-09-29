using Newtonsoft.Json;

namespace GitLfsApi.Data
{
    public class ServerRef
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
