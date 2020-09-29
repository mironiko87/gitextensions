using GitLfsApi.Data;
using Newtonsoft.Json;

namespace GitLfsApi.Dto
{
    class DeleteLockRequest
    {
        [JsonProperty("force")]
        public bool Force { get; set; }

        [JsonProperty("ref")]
        public ServerRef Ref { get; set; }
    }
}
