using Newtonsoft.Json;

namespace GitLfsApi.Dto
{
    class CreateLockRequest
    {
        [JsonProperty("path", Required = Required.Always)]
        public string Path { get; set; }
    }
}
