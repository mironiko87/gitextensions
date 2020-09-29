using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLfsApi.Exceptions
{
    public class InvalidGitRepositoryException : Exception
    {
        public InvalidGitRepositoryException(string message) : base(message) { }
    }
}
