using GitLfsApi.Data;
using GitLfsApi.Dto;

namespace GitLfsApi.Exceptions
{
    public class FileAlreadyLockedException : GitLfsApiException
    {
        public Lock Lock { get; }
        internal FileAlreadyLockedException(LockExistsErrorResponse response) : base(response)
        {
            Lock = response.Lock;
        }
    }
}
