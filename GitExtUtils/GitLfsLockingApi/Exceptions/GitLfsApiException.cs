using GitLfsApi.Dto;
using System;

namespace GitLfsApi.Exceptions
{
    public class GitLfsApiException : Exception
    {
        public Uri DocumentationUrl { get; }
        public string RequestId { get; }
        internal GitLfsApiException(ErrorResponse serverErrorResponse) : base(serverErrorResponse.Message)
        {
            DocumentationUrl = serverErrorResponse.DocumentationUrl;
            RequestId = serverErrorResponse.RequestId;
        }

        internal GitLfsApiException(string message) : base(message)
        {

        }
    }
}
