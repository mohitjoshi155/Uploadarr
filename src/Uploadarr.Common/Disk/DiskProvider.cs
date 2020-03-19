using EnsureThat;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Uploadarr.Common
{
    public class DiskProvider : IDiskProvider
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static StringComparison PathStringComparison
        {
            get
            {
                if (OsInfo.IsWindows)
                {
                    return StringComparison.OrdinalIgnoreCase;
                }

                return StringComparison.Ordinal;
            }
        }

        public long? GetAvailableSpace(string path)
        {
            Ensure.String.IsValidPath(path);

            var mount = GetMount(path);

            if (mount == null)
            {
                Log.Debug("Unable to get free space for '{0}', unable to find suitable drive", path);
                return null;
            }

            return mount.AvailableFreeSpace;
        }

        public long? GetTotalSize(string path)
        {
            Ensure.String.IsValidPath(path);

            var root = GetPathRoot(path);

            if (!FolderExists(root))
                throw new DirectoryNotFoundException(root);

            return DriveTotalSizeEx(root);
        }

        public DateTime FolderGetCreationTime(string path)
        {
            CheckFolderExists(path);

            return new DirectoryInfo(path).CreationTimeUtc;
        }

        public DateTime FolderGetLastWrite(string path)
        {
            CheckFolderExists(path);

            var dirFiles = GetFiles(path, SearchOption.AllDirectories).ToList();

            if (!dirFiles.Any())
            {
                return new DirectoryInfo(path).LastWriteTimeUtc;
            }

            return dirFiles.Select(f => new FileInfo(f)).Max(c => c.LastWriteTimeUtc);
        }

        public DateTime FileGetLastWrite(string path)
        {
            CheckFileExists(path);

            return new FileInfo(path).LastWriteTimeUtc;
        }

        private void CheckFolderExists(string path)
        {
            Ensure.String.IsValidPath(path);

            if (!FolderExists(path))
            {
                throw new DirectoryNotFoundException("Directory doesn't exist. " + path);
            }
        }

        private void CheckFileExists(string path)
        {
            Ensure.String.IsValidPath(path);

            if (!FileExists(path))
            {
                throw new FileNotFoundException("File doesn't exist: " + path);
            }
        }

        public void EnsureFolder(string path)
        {
            if (!FolderExists(path))
            {
                CreateFolder(path);
            }
        }

        public bool FolderExists(string path)
        {
            Ensure.String.IsValidPath(path);
            return Directory.Exists(path);
        }

        public bool FileExists(string path)
        {
            Ensure.String.IsValidPath(path);
            return FileExists(path, PathStringComparison);
        }

        public bool FileExists(string path, StringComparison stringComparison)
        {
            Ensure.String.IsValidPath(path);

            switch (stringComparison)
            {
                case StringComparison.CurrentCulture:
                case StringComparison.InvariantCulture:
                case StringComparison.Ordinal:
                    {
                        return File.Exists(path) && path == path.GetActualCasing();
                    }
                default:
                    {
                        return File.Exists(path);
                    }
            }
        }

        public bool FolderWritable(string path)
        {
            Ensure.String.IsValidPath(path);

            try
            {
                var testPath = Path.Combine(path, "sonarr_write_test.txt");
                var testContent = $"This file was created to verify if '{path}' is writable. It should've been automatically deleted. Feel free to delete it.";
                File.WriteAllText(testPath, testContent);
                File.Delete(testPath);
                return true;
            }
            catch (Exception e)
            {
                Log.Warn(e, $"Directory '{path}' isn't writable. {e.Message}");
                return false;
            }
        }

        public string[] GetDirectories(string path)
        {
            if (path.IsNullOrWhiteSpace())
            {
                return Directory.GetLogicalDrives();
            }
            Ensure.String.IsValidPath(path);

            return Directory.GetDirectories(path);
        }

        public string[] GetFiles(string path, SearchOption searchOption)
        {
            Ensure.String.IsValidPath(path);

            return Directory.GetFiles(path, "*.*", searchOption);
        }

        public long GetFolderSize(string path)
        {
            Ensure.String.IsValidPath(path);

            return GetFiles(path, SearchOption.AllDirectories).Sum(e => new FileInfo(e).Length);
        }

        public long GetFileSize(string path)
        {
            Ensure.String.IsValidPath(path);

            if (!FileExists(path))
            {
                throw new FileNotFoundException("File doesn't exist: " + path);
            }

            var fi = new FileInfo(path);
            return fi.Length;
        }

        public void CreateFolder(string path)
        {
            Ensure.String.IsValidPath(path);
            Directory.CreateDirectory(path);
        }

        public void DeleteFile(string path)
        {
            Ensure.String.IsValidPath(path);
            // Logger.Trace("Deleting file: {0}", path);

            RemoveReadOnly(path);

            File.Delete(path);
        }

        public void CopyFile(string source, string destination, bool overwrite = false)
        {
            Ensure.String.IsValidPath(source);
            Ensure.String.IsValidPath(destination);

            if (source.PathEquals(destination))
            {
                throw new IOException($"Source and destination can't be the same {source}");
            }

            CopyFileInternal(source, destination, overwrite);
        }

        protected virtual void CopyFileInternal(string source, string destination, bool overwrite = false)
        {
            File.Copy(source, destination, overwrite);
        }

        public void MoveFile(string source, string destination, bool overwrite = false)
        {
            Ensure.String.IsValidPath(source);
            Ensure.String.IsValidPath(destination);

            if (source.PathEquals(destination))
            {
                throw new IOException($"Source and destination can't be the same {source}");
            }

            if (FileExists(destination) && overwrite)
            {
                DeleteFile(destination);
            }

            RemoveReadOnly(source);
            MoveFileInternal(source, destination);
        }

        private static long DriveTotalSizeEx(string folderName)
        {
            Ensure.String.IsValidPath(folderName);

            if (!folderName.EndsWith("\\"))
            {
                folderName += '\\';
            }

            //ulong total = 0;
            //ulong dummy1 = 0;
            //ulong dummy2 = 0;

            // TODO Update method of retrieving disk space
            //if (GetDiskFreeSpaceEx(folderName, out dummy1, out total, out dummy2))
            //{
            //    return (long)total;
            //}

            return 0;
        }


        public void MoveFolder(string source, string destination)
        {
            Ensure.String.IsValidPath(source);
            Ensure.String.IsValidPath(destination);

            Directory.Move(source, destination);
        }

        protected virtual void MoveFileInternal(string source, string destination)
        {
            File.Move(source, destination);
        }

        public void DeleteFolder(string path, bool recursive)
        {
            Ensure.String.IsValidPath(path);

            var files = Directory.GetFiles(path, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            Array.ForEach(files, RemoveReadOnly);

            Directory.Delete(path, recursive);
        }

        public string ReadAllText(string filePath)
        {
            Ensure.String.IsValidPath(filePath);

            return File.ReadAllText(filePath);
        }

        public void WriteAllText(string filename, string contents)
        {
            // TODO Check if this is really a filename or a path
            Ensure.String.IsValidPath(filename);
            RemoveReadOnly(filename);
            File.WriteAllText(filename, contents);
        }

        public void FolderSetLastWriteTime(string path, DateTime dateTime)
        {
            Ensure.String.IsValidPath(path);

            Directory.SetLastWriteTimeUtc(path, dateTime);
        }

        public void FileSetLastWriteTime(string path, DateTime dateTime)
        {
            Ensure.String.IsValidPath(path);

            File.SetLastWriteTime(path, dateTime);
        }

        public bool IsFileLocked(string file)
        {
            try
            {
                using (File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return true;
            }
        }

        public string GetPathRoot(string path)
        {
            Ensure.String.IsValidPath(path);

            return Path.GetPathRoot(path);
        }

        public string GetParentFolder(string path)
        {
            Ensure.String.IsValidPath(path);

            var parent = Directory.GetParent(path.TrimEnd(Path.DirectorySeparatorChar));

            if (parent == null)
            {
                return null;
            }

            return parent.FullName;
        }

        //public void SetPermissions(string filename, WellKnownSidType accountSid, FileSystemRights rights, AccessControlType controlType)
        //{
        //    try
        //    {
        //        var sid = new SecurityIdentifier(accountSid, null);

        //        var directoryInfo = new DirectoryInfo(filename);
        //        var directorySecurity = directoryInfo.GetAccessControl(AccessControlSections.Access);

        //        var rules = directorySecurity.GetAccessRules(true, false, typeof(SecurityIdentifier));

        //        if (rules.OfType<FileSystemAccessRule>().Any(acl => acl.AccessControlType == controlType && (acl.FileSystemRights & rights) == rights && acl.IdentityReference.Equals(sid)))
        //        {
        //            return;
        //        }

        //        var accessRule = new FileSystemAccessRule(sid, rights,
        //                                                  InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
        //                                                  PropagationFlags.InheritOnly, controlType);

        //        bool modified;
        //        directorySecurity.ModifyAccessRule(AccessControlModification.Add, accessRule, out modified);

        //        if (modified)
        //        {
        //            directoryInfo.SetAccessControl(directorySecurity);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Warn(e, "Couldn't set permission for {0}. account:{1} rights:{2} accessControlType:{3}", filename, accountSid, rights, controlType);
        //        throw;
        //    }

        //}

        private static void RemoveReadOnly(string path)
        {
            if (File.Exists(path))
            {
                var attributes = File.GetAttributes(path);

                if (attributes.HasFlag(FileAttributes.ReadOnly))
                {
                    var newAttributes = attributes & ~(FileAttributes.ReadOnly);
                    File.SetAttributes(path, newAttributes);
                }
            }
        }

        public FileAttributes GetFileAttributes(string path)
        {
            return File.GetAttributes(path);
        }

        public void EmptyFolder(string path)
        {
            Ensure.String.IsValidPath(path);

            foreach (var file in GetFiles(path, SearchOption.TopDirectoryOnly))
            {
                DeleteFile(file);
            }

            foreach (var directory in GetDirectories(path))
            {
                DeleteFolder(directory, true);
            }
        }

        public string[] GetFixedDrives()
        {
            return GetMounts().Where(x => x.DriveType == DriveType.Fixed).Select(x => x.RootDirectory).ToArray();
        }

        public string GetVolumeLabel(string path)
        {
            var driveInfo = GetMounts().SingleOrDefault(d => d.RootDirectory.PathEquals(path));

            if (driveInfo == null)
            {
                return null;
            }

            return driveInfo.VolumeLabel;
        }

        public FileStream OpenReadStream(string path)
        {
            if (!FileExists(path))
            {
                throw new FileNotFoundException("Unable to find file: " + path, path);
            }

            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        public FileStream OpenWriteStream(string path)
        {
            return new FileStream(path, FileMode.Create);
        }

        public List<IMount> GetMounts()
        {
            return GetAllMounts().Where(d => !IsSpecialMount(d)).ToList();
        }

        protected virtual List<IMount> GetAllMounts()
        {
            return GetDriveInfoMounts().Where(d => d.DriveType == DriveType.Fixed || d.DriveType == DriveType.Network || d.DriveType == DriveType.Removable)
                                       .Select(d => new DriveInfoMount(d))
                                       .Cast<IMount>()
                                       .ToList();
        }

        protected virtual bool IsSpecialMount(IMount mount)
        {
            return false;
        }

        public virtual IMount GetMount(string path)
        {
            try
            {
                var mounts = GetAllMounts();

                return mounts.Where(drive => drive.RootDirectory.PathEquals(path) ||
                                             drive.RootDirectory.IsParentPath(path))
                          .OrderByDescending(drive => drive.RootDirectory.Length)
                          .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Debug(ex, $"Failed to get mount for path {path}");
                return null;
            }
        }

        protected List<DriveInfo> GetDriveInfoMounts()
        {
            return DriveInfo.GetDrives()
                            .Where(d => d.IsReady)
                            .ToList();
        }

        public List<DirectoryInfo> GetDirectoryInfos(string path)
        {
            Ensure.String.IsValidPath(path);

            var di = new DirectoryInfo(path);

            return di.GetDirectories().ToList();
        }

        public List<FileInfo> GetFileInfos(string path)
        {
            Ensure.String.IsValidPath(path);

            var di = new DirectoryInfo(path);

            return di.GetFiles().ToList();
        }

        public void RemoveEmptySubfolders(string path)
        {
            var subfolders = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            var files = GetFiles(path, SearchOption.AllDirectories);

            foreach (var subfolder in subfolders)
            {
                if (files.None(f => subfolder.IsParentPath(f)))
                {
                    DeleteFolder(subfolder, false);
                }
            }
        }

        public void SaveStream(Stream stream, string path)
        {
            using (var fileStream = OpenWriteStream(path))
            {
                stream.CopyTo(fileStream);
            }
        }
    }
}
