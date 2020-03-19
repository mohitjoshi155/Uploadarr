using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Uploadarr.Common;

namespace Uploadarr.API
{
    public class FileSystemModule : ApiBaseModule
    {
        private readonly IDiskProvider _diskProvider;
        private readonly IFileSystemLookupService _fileSystemLookupService;

        public FileSystemModule(IFileSystemLookupService fileSystemLookupService, IDiskProvider diskProvider) : base("/filesystem")
        {
            _diskProvider = diskProvider;
            _fileSystemLookupService = fileSystemLookupService;

            Get("/", GetContents);
            Get("/type", GetEntityType);
        }

        private Task GetContents(HttpRequest req, HttpResponse res)
        {
            string path = GetQueryValue<string>(req, "path");
            bool includeFiles = GetQueryValue<bool>(req, "includeFiles");
            bool allowFoldersWithoutTrailingSlashes = GetQueryValue<bool>(req, "allowFoldersWithoutTrailingSlashes");
            var result =
                _fileSystemLookupService.LookupContents(path, includeFiles, allowFoldersWithoutTrailingSlashes);
            return res.WriteAsync(result.ToJson());

        }



        private Task GetEntityType(HttpRequest req, HttpResponse res)
        {
            var pathQuery = req.Path;
            var path = (string)pathQuery.Value;


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
