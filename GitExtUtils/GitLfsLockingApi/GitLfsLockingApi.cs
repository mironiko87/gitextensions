using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GitExtUtils;
using GitLfsApi.Data;
using GitLfsApi.Dto;
using GitLfsApi.Exceptions;
using Newtonsoft.Json;

namespace GitLfsApi
{
    public class GitLfsLockingApi
    {
        public string RepoDirectory { get; set; }
        public string GitFolder => RepoDirectory + @"\.git\";

        public GitLfsLockingApi(string repoDir)
        {
            if (!Directory.Exists($@"{repoDir}\.git"))
            {
                throw new InvalidGitRepositoryException($"{repoDir} is invalid git repository.");
            }

            RepoDirectory = repoDir;
            RegisterRepository(RepoDirectory);
        }

        private static string GetGitPath()
        {
            return GetExePath("git.exe");
        }

        private static ProcessOutput PerformGitCommand(GitArgumentBuilder args, string workingDir = "")
        {
            return RunProcessAndWaitForOutput(GetGitPath(), args.ToString(), workingDir);
        }

        private static T PerformLfsCommand<T>(GitArgumentBuilder args, string workingDir = "")
        {
            var output = RunProcessAndWaitForOutput(GetGitPath(), " lfs " + args, workingDir);
            if (string.IsNullOrEmpty(output.ErrorOutput))
            {
                return JsonConvert.DeserializeObject<T>(output.StandardOutput);
            }

            throw new GitLfsApiException(output.ErrorOutput);
        }

        private static void PerformLfsCommand(GitArgumentBuilder args, string workingDir = "")
        {
            var output = RunProcessAndWaitForOutput(GetGitPath(), " lfs " + args, workingDir);
            if (!string.IsNullOrEmpty(output.ErrorOutput))
            {
                throw new GitLfsApiException(output.ErrorOutput);
            }
        }

        private static string GetExePath(string fileName)
        {
            if (File.Exists(fileName))
            {
                return Path.GetFullPath(fileName);
            }

            var values = Environment.GetEnvironmentVariable("PATH");
            var winDir = Environment.GetEnvironmentVariable("WinDir");
            foreach (var path in values.Split(';'))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }

                if (fullPath.Contains($@"{winDir}\System32"))
                {
                    fullPath = fullPath.Replace($@"{winDir}\System32", $@"{winDir}\Sysnative");
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }
            }

            throw new Exception($"{fileName} not found.");
        }

        private static ProcessOutput RunProcessAndWaitForOutput(string process, string args, string workingDir = "")
        {
            if (workingDir == "")
            {
                workingDir = Directory.GetCurrentDirectory();
            }

            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = process;
            p.StartInfo.Arguments = args;
            p.StartInfo.WorkingDirectory = workingDir;
            p.Start();
            p.WaitForExit();
            return new ProcessOutput
            {
                StandardOutput = p.StandardOutput.ReadToEnd(),
                ErrorOutput = p.StandardError.ReadToEnd()
            };
        }

        private void TrackFileAsLockable(string path)
        {
            var args = new GitArgumentBuilder("track")
            {
                { path.TrimStart('\\', '/') },
                "--lockable"
            };
            PerformLfsCommand(args, RepoDirectory);
        }

        public Lock CreateLockForFile(string path)
        {
            TrackFileAsLockable(path);
            var args = new GitArgumentBuilder("lock")
            {
                { path },
                "--json"
            };
            return PerformLfsCommand<Lock>(args, RepoDirectory);
        }

        public List<Lock> GetLocks()
        {
            var args = new GitArgumentBuilder("locks")
            {
                "--json"
            };
            return PerformLfsCommand<List<Lock>>(args, RepoDirectory);
        }

        public Lock DeleteLockForFile(string path, bool force = false)
        {
            var forceFlag = force ? " --force" : "";
            var args = new GitArgumentBuilder("unlock")
            {
                { path },
                { forceFlag },
                "--json"
            };
            return PerformLfsCommand<Lock>(args, RepoDirectory);
        }

        public static bool IsRepoRegistered(string repoPath)
        {
            var args = new GitArgumentBuilder("config")
            {
                "--global",
                "--get-all",
                "--path",
                "gitLfsLocking.repos.repo",
                { repoPath.Replace(@"\", @"\\") } /* git adds double backslashes when adding config data */
            };
            var output = PerformGitCommand(args);
            return !string.IsNullOrEmpty(output.StandardOutput);
        }

        public static void RegisterRepository(string repoPath)
        {
            if (IsRepoRegistered(repoPath))
            {
                return;
            }

            var args = new GitArgumentBuilder("config")
            {
                "--global",
                "--add",
                "--path",
                "gitLfsLocking.repos.repo",
                { repoPath }
            };
            var output = PerformGitCommand(args);
            if (!string.IsNullOrEmpty(output.ErrorOutput))
            {
                throw new Exception(output.ErrorOutput);
            }
        }

        public static void UnregisterRepository(string repoPath)
        {
            var args = new GitArgumentBuilder("config")
            {
                "--global",
                "--unset-all",
                "--path",
                "gitLfsLocking.repos.repo",
                { repoPath.Replace(@"\", @"\\") } /* git adds double backslashes when adding config data */
            };
            var output = PerformGitCommand(args);
            if (!string.IsNullOrEmpty(output.ErrorOutput))
            {
                throw new Exception(output.ErrorOutput);
            }
        }

        public static List<string> GetRegisteredRepos()
        {
            var args = new GitArgumentBuilder("config")
            {
                "--global",
                "--get-all",
                "--path",
                "gitLfsLocking.repos.repo"
            };
            var output = PerformGitCommand(args);
            if (!string.IsNullOrEmpty(output.ErrorOutput))
            {
                throw new Exception(output.ErrorOutput);
            }

            return output.StandardOutput.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
