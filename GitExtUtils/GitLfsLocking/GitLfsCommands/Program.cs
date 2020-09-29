using CommandLine;
using GitLfsApi;
using GitLfsApi.Exceptions;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace GitLfsCommands
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            try
            {
                Parser.Default.ParseArguments<LockFile, UnlockFile, UpdateLockList>(args)
                    .WithParsed<LockFile>(ar => RunLockFileCommand(ar))
                    .WithParsed<UnlockFile>(ar => RunUnlockFileCommand(ar))
                    .WithParsed<UpdateLockList>(ar => RunUpdateLockListCommand(ar));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private static void RunUpdateLockListCommand(UpdateLockList opts)
        {
            if(opts.UpdateAll)
            {
                foreach(string repo in GitLfsLockingApi.GetRegisteredRepos())
                {
                    try
                    {
                        var api = new GitLfsLockingApi(repo);
                        File.WriteAllLines($@"{api.GitFolder}/git_lfs_ext_locks", api.GetLocks().
                            Select(x => "\\" + x.Path.Replace('/', '\\')).ToArray());
                    }
                    catch (InvalidGitRepositoryException)
                    {
                        GitLfsLockingApi.UnregisterRepository(repo);
                    }
                    catch(Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                var fullPath = Path.GetFullPath(opts.Path);
                var gitDir = ExtractGitBaseDirectory(fullPath);
                var api = new GitLfsLockingApi(gitDir);
                File.WriteAllLines($@"{api.GitFolder}/git_lfs_ext_locks", api.GetLocks().Select(x => "\\" + x.Path.Replace('/', '\\')).ToArray());
            }
        }

        private static void RunUnlockFileCommand(UnlockFile opts)
        {
            var fullPath = Path.GetFullPath(opts.Path);
            var gitDir = ExtractGitBaseDirectory(fullPath);
            var relativeFilePath = fullPath.Remove(0, gitDir.Length);
            var api = new GitLfsLockingApi(gitDir);
            api.DeleteLockForFile(relativeFilePath, opts.Force);
            RemoveLockFromListFile(api.GitFolder, relativeFilePath);
        }

        private static void RunLockFileCommand(LockFile opts)
        {
            var fullPath = Path.GetFullPath(opts.Path);
            var gitDir = ExtractGitBaseDirectory(fullPath);
            var relativeFilePath = fullPath.Remove(0, gitDir.Length);
            var api = new GitLfsLockingApi(gitDir);
            api.CreateLockForFile(relativeFilePath);
            AddToLockListFile(api.GitFolder, relativeFilePath);
        }

        private static void AddToLockListFile(string gitFolderPath, string entry)
        {
            File.AppendAllText($@"{gitFolderPath}\git_lfs_ext_locks", entry + "\n");
        }

        private static void RemoveLockFromListFile(string gitFolderPath, string entry)
        {
            var fileName = $@"{gitFolderPath}\git_lfs_ext_locks";
            var lines = File.ReadLines(fileName);
            File.WriteAllLines(fileName, lines.Where(l => l != entry).ToArray());
        }

        private static string ExtractGitBaseDirectory(string dir)
        {
            if (Directory.Exists($@"{dir}\.git\"))
                return dir;
            if (dir == Directory.GetDirectoryRoot(dir))
                throw new ArgumentException("Directory is not valid git directory.");
            return ExtractGitBaseDirectory(Directory.GetParent(dir).FullName);
        }
    }
}