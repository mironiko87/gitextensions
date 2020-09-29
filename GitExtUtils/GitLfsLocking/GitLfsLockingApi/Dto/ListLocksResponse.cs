using GitLfsApi.Data;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GitLfsApi.Dto
{
    class ListLocksResponse
    {
        [JsonProperty("locks")]
        public List<Lock> Locks { get; set; }
        [JsonProperty("next_cursor")]
        public string NextCursor { get; set; }
    }
}
