using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Uploadarr.Common;

namespace Uploadarr.API
{
    public class FileSystemModule : ApiBaseModule
    {
        private readonly IFileSystem _fileSystem;

        public FileSystemModule(IFileSystem fileSystem) : base("/filesystem")
        {
            _fileSystem = fileSystem;
            Get("/", GetContents);
            Get("/type", GetEntityType);
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
            ;
            return res.WriteAsync(_fileSystem.GetDirectories("").ToJson());
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
