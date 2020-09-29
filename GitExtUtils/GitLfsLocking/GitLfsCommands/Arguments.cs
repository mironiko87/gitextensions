using CommandLine;
using System.Collections.Generic;

namespace GitLfsCommands
{
    [Verb("lock", HelpText = "Add file contents to the index.")]
    class LockFile
    {
        [Value(0, Required = true, MetaName = "path", HelpText = "Path to file that you want to lock.")]
        public string Path { get; set; }
    }

    [Verb("unlock", HelpText = "Add file contents to the index.")]
    class UnlockFile
    {
        [Option("force",
          Default = false,
          HelpText = "Force unlock file.")]
        public bool Force { get; set; }

        [Value(0, Required = true, MetaName = "path", HelpText = "Path to file that you want to unlock.")]
        public string Path { get; set; }
    }

    [Verb("update-lock-list", HelpText = "Add file contents to the index.")]
    class UpdateLockList
    {
        [Option("all",
           Default = false,
           HelpText = "Update all registered repositories.")]
        public bool UpdateAll { get; set; }

        [Value(0, Required = false, MetaName = "path", HelpText = "Path to git folder for which you want to update list of locked files.")]
        public string Path { get; set; }
    }

    [Verb("register-repo", HelpText = "Add file contents to the index.")]
    class RegisterRepo
    {
        [Value(0, Required = true, MetaName = "path", HelpText = "Path to git folder for which you want to update list of locked files.")]
        public string Path { get; set; }
    }
}
