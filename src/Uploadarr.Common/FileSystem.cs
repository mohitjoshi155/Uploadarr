using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace Uploadarr.Common
{
    public interface IFileSystem
    {
        IDirectoryContents GetRootDirectory();
        string[] GetDirectories(string path);
    }

    public class FileSystem : IFileSystem
    {
        private readonly IFileProvider _fileProvider;
        public FileSystem(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
           
        }

        public IDirectoryContents GetRootDirectory()
        {
            var x = _fileProvider.GetDirectoryContents("");

            return x;
        }

        public string[] GetDirectories(string path = "")
        {
            var x = _fileProvider.GetDirectoryContents(path);
            List<string> directories = new List<string>();

            foreach (var fileInfo in x)
            {
                if (fileInfo.IsDirectory)
                {
                    directories.Add(fileInfo.Name);
                }
            }

            return directories.ToArray();
        }
    }
}
