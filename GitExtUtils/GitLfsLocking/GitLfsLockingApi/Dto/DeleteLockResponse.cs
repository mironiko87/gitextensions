using GitLfsApi.Data;
using Newtonsoft.Json;

namespace GitLfsApi.Dto
{
    class DeleteLockResponse
    {
        [JsonProperty("lock")]
        public Lock Lock { get; set; }
    }
}
