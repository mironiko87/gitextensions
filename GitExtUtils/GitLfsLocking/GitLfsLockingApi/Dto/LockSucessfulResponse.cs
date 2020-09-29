using GitLfsApi.Data;
using Newtonsoft.Json;

namespace GitLfsApi.Dto
{
    class CreateLockReponse
    {
        [JsonProperty("lock")]
        public Lock Lock { get; set;}
    }
}
