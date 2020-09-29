using Newtonsoft.Json;

namespace GitLfsApi.Data
{
    public class Owner
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
