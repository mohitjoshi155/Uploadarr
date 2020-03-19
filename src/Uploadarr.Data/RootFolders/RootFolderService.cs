using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Uploadarr.Common;

namespace Uploadarr.Data
{
    public interface IRootFolderService
    {
        List<RootFolder> All();
        List<RootFolder> AllWithUnmappedFolders();
        RootFolder Add(RootFolder rootDir);
        void Remove(int id);
        RootFolder Get(int id);
        string GetBestRootFolderPath(string path);
    }

    public class RootFolderService : IRootFolderService
    {
        private readonly IDiskProvider _diskProvider;
        private readonly MainDatabaseContext _dbContext;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly HashSet<string> SpecialFolders = new HashSet<string>
                                                                 {
                                                                     "$recycle.bin",
                                                                     "system volume information",
                                                                     "recycler",
                                                                     "lost+found",
                                                                     ".appledb",
                                                                     ".appledesktop",
                                                                     ".appledouble",
                                                                     "@eadir",
                                                                     ".grab"
                                                                 };


        public RootFolderService(IDiskProvider diskProvider,
                                 MainDatabaseContext dbContext)
        {
            _diskProvider = diskProvider;
            _dbContext = dbContext;
        }

        public List<RootFolder> All()
        {

            var rootFolders = _dbContext.RootFolders.AsNoTracking().ToList();

            return rootFolders;
        }

        public List<RootFolder> AllWithUnmappedFolders()
        {
            var rootFolders = _dbContext.RootFolders.AsNoTracking().ToList();

            rootFolders.ForEach(folder =>
            {
                try
                {
                    if (folder.Path.IsPathValid())
                    {
                        GetDetails(folder);
                    }
                }
                //We don't want an exception to prevent the root folders from loading in the UI, so they can still be deleted
                catch (Exception ex)
                {
                    Log.Error(ex, "Unable to get free space and unmapped folders for root folder {0}", folder.Path);
                    folder.UnmappedFolders = new List<UnmappedFolder>();
                }
            });

            return rootFolders;
        }

        public RootFolder Add(RootFolder rootFolder)
        {
            var all = All();

            if (string.IsNullOrWhiteSpace(rootFolder.Path) || !Path.IsPathRooted(rootFolder.Path))
            {
                throw new ArgumentException("Invalid path");
            }

            if (!_diskProvider.FolderExists(rootFolder.Path))
            {
                throw new DirectoryNotFoundException("Can't add root directory that doesn't exist.");
            }

            if (all.Exists(r => r.Path.PathEquals(rootFolder.Path)))
            {
                throw new InvalidOperationException("Recent directory already exists.");
            }

            if (!_diskProvider.FolderWritable(rootFolder.Path))
            {
                throw new UnauthorizedAccessException($"Root folder path '{rootFolder.Path}' is not writable by user '{Environment.UserName}'");
            }

            _dbContext.RootFolders.Add(rootFolder);
            _dbContext.SaveChanges();

            GetDetails(rootFolder);

            return rootFolder;
        }

        public void Remove(int id)
        {
            _dbContext.RootFolders.Remove(_dbContext.RootFolders.Find(id));
            _dbContext.SaveChanges();
        }

        private List<UnmappedFolder> GetUnmappedFolders(string path)
        {
            Log.Debug("Generating list of unmapped folders");

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Invalid path provided", nameof(path));
            }

            var results = new List<UnmappedFolder>();
            // var series = new List<Series>();

            if (!_diskProvider.FolderExists(path))
            {
                Log.Debug("Path supplied does not exist: {0}", path);
                return results;
            }

            var possibleSeriesFolders = _diskProvider.GetDirectories(path).ToList();
            // TODO Re-enable when SeriesService has been implemented
            // var unmappedFolders = possibleSeriesFolders.Except(series.Select(s => s.Path), PathEqualityComparer.Instance).ToList();
            var unmappedFolders = new List<string>();

            foreach (string unmappedFolder in unmappedFolders)
            {
                var di = new DirectoryInfo(unmappedFolder.Normalize());
                results.Add(new UnmappedFolder { Name = di.Name, Path = di.FullName });
            }

            var setToRemove = SpecialFolders;
            results.RemoveAll(x => setToRemove.Contains(new DirectoryInfo(x.Path.ToLowerInvariant()).Name));

            Log.Debug("{0} unmapped folders detected.", results.Count);
            return results.OrderBy(u => u.Name, StringComparer.InvariantCultureIgnoreCase).ToList();
        }

        public RootFolder Get(int id)
        {
            var rootFolder = _dbContext.RootFolders.Find(id);
            GetDetails(rootFolder);

            return rootFolder;
        }

        public string GetBestRootFolderPath(string path)
        {
            var possibleRootFolder = All().Where(r => r.Path.IsParentPath(path))
                                          .OrderByDescending(r => r.Path.Length)
                                          .FirstOrDefault();

            if (possibleRootFolder == null)
            {
                return Path.GetDirectoryName(path);
            }

            return possibleRootFolder.Path;
        }

        private void GetDetails(RootFolder rootFolder)
        {
            Task.Run(() =>
            {
                if (_diskProvider.FolderExists(rootFolder.Path))
                {
                    rootFolder.Accessible = true;
                    rootFolder.FreeSpace = _diskProvider.GetAvailableSpace(rootFolder.Path);
                    rootFolder.TotalSpace = _diskProvider.GetTotalSize(rootFolder.Path);
                    rootFolder.UnmappedFolders = GetUnmappedFolders(rootFolder.Path);
                }
            }).Wait(5000);
        }
    }
}
