using System;
using GitLfsApi.Dto;

namespace GitLfsApi.Exceptions
{
    public class GitLfsApiException : Exception
    {
        public Uri DocumentationUrl { get; }
        public string RequestId { get; }
        public GitLfsApiException(ErrorResponse serverErrorResponse) : base(serverErrorResponse.Message)
        {
            DocumentationUrl = serverErrorResponse.DocumentationUrl;
            RequestId = serverErrorResponse.RequestId;
        }

        public GitLfsApiException(string message) : base(message)
        {
        }
    }
}
