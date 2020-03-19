using AutoMapper;
using Carter.ModelBinding;
using Carter.Request;
using Carter.Response;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uploadarr.Data;

namespace Uploadarr.API
{
    public class RootFolderModule : ApiBaseModule
    {
        private readonly IRootFolderService _rootFolderService;
        private readonly IMapper _mapper;

        public RootFolderModule(IRootFolderService rootFolderService, IMapper mapper) : base("/rootfolder")
        {
            _rootFolderService = rootFolderService;
            _mapper = mapper;

            Get("/", GetRootFolders);
            Get("/{id:int}", GetRootFolder);
            Post("/", CreateRootFolder);
            Delete("/{id:int}", DeleteRootFolder);
        }

        private Task GetRootFolder(HttpRequest req, HttpResponse res)
        {
            int id = req.RouteValues.As<int>("id");
            RootFolderDTO result = _mapper.Map<RootFolderDTO>(_rootFolderService.Get(id));
            return res.Negotiate(result);
        }

        private Task CreateRootFolder(HttpRequest req, HttpResponse res)
        {
            var result = req.BindAndValidate<RootFolderValidator>().Result;

            if (!result.ValidationResult.IsValid)
            {
                res.StatusCode = StatusCodes.Status422UnprocessableEntity;
                return res.Negotiate(result.ValidationResult.GetFormattedErrors());
            }

            res.StatusCode = StatusCodes.Status201Created;

            RootFolder model = _mapper.Map<RootFolder>(result.Data);

            int id = _rootFolderService.Add(model).Id;

            // TODO Give a sensible response back
            return res.WriteAsync(id.ToString());

        }
        private Task GetRootFolders(HttpContext ctx)
        {
            var list = _mapper.Map<List<RootFolderDTO>>(_rootFolderService.AllWithUnmappedFolders());

            ctx.Response.StatusCode = list.Count > 0 ? StatusCodes.Status200OK : StatusCodes.Status204NoContent;

            return ctx.Response.Negotiate(list);
        }

        private Task DeleteRootFolder(HttpContext ctx)
        {
            int id = ctx.Request.RouteValues.As<int>("id");
            if (id == 0)
            {
                ctx.Response.StatusCode = StatusCodes.Status400BadRequest;

                return ctx.Response.WriteAsync($"Invalid Id: {id}");
            }

            _rootFolderService.Remove(id);
            ctx.Response.StatusCode = StatusCodes.Status200OK;

            return ctx.Response.WriteAsync($"Removed RootFolder with Id: {id}");
        }
    }
}
