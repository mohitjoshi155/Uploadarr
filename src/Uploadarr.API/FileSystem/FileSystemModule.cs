using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Uploadarr.Common;

namespace Uploadarr.API
{
    public class FileSystemModule : ApiBaseModule
    {
        private readonly IFileSystemLookupService _fileSystemLookupService;
        private readonly IDiskProvider _diskProvider;
        // private readonly IDiskScanService _diskScanService;

        public FileSystemModule(IFileSystemLookupService fileSystemLookupService,
                                IDiskProvider diskProvider)
            : base("/filesystem")
        {
            _fileSystemLookupService = fileSystemLookupService;
            _diskProvider = diskProvider;
            // _diskScanService = diskScanService;
            Get("/", GetContents);
            Get("/type", GetEntityType);
            //Get("/mediafiles",  x => GetMediaFiles());
        }

        private Task GetContents(HttpRequest req, HttpResponse res)
        {
            var pathQuery = req.Path;
            bool includeFiles = false;
            if (req.Query.ContainsKey("includeFiles"))
            {
                includeFiles = bool.Parse(req.Query["includeFiles"]);
            }    
            bool allowFoldersWithoutTrailingSlashes = false;
            if (req.Query.ContainsKey("allowFoldersWithoutTrailingSlashes"))
            {
                allowFoldersWithoutTrailingSlashes = bool.Parse(req.Query["allowFoldersWithoutTrailingSlashes"]);
            }

            var result = _fileSystemLookupService.LookupContents(pathQuery.Value, includeFiles, allowFoldersWithoutTrailingSlashes);
            return res.WriteAsync(result.ToString());
        }

        private Task GetEntityType(HttpRequest req, HttpResponse res)
        {
            var pathQuery = req.Path;
            var path = (string)pathQuery.Value;

            if (_diskProvider.FileExists(path))
            {
                return res.WriteAsync( "file");
            }

            //Return folder even if it doesn't exist on disk to avoid leaking anything from the UI about the underlying system
                return res.WriteAsync("folder");
        }

        //private object GetMediaFiles()
        //{
        //    var pathQuery = Request.Query.path;
        //    var path = (string)pathQuery.Value;

        //    if (!_diskProvider.FolderExists(path))
        //    {
        //        return new string[0];
        //    }

        //    return _diskScanService.GetVideoFiles(path).Select(f => new {
        //        Path = f,
        //        RelativePath = path.GetRelativePath(f),
        //        Name = Path.GetFileName(f)
        //    });
        //}
    }
}
