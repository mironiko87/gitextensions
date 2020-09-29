using GitLfsApi.Data;
using Newtonsoft.Json;

namespace GitLfsApi.Dto
{
    class LockExistsErrorResponse : ErrorResponse
    {
        [JsonProperty("lock")]
        public Lock Lock { get; set; }
    }
}
