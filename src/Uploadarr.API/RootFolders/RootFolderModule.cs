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

        private async Task CreateRootFolder(HttpRequest req, HttpResponse res)
        {
            var result = await req.BindAndValidate<RootFolderDTO>();

            if (!result.ValidationResult.IsValid)
            {
                res.StatusCode = StatusCodes.Status422UnprocessableEntity;
                await res.AsJson(result.ValidationResult.GetFormattedErrors());
                return;
            }

            res.StatusCode = StatusCodes.Status201Created;

            RootFolder model = _rootFolderService.Add(_mapper.Map<RootFolder>(result.Data));

            await res.Negotiate(model);

        }
        private async Task GetRootFolders(HttpRequest req, HttpResponse res)
        {
            int typeId = GetQueryValue<int>(req, "typeId");

            var list = _mapper.Map<List<RootFolderDTO>>(_rootFolderService.All((RootFolderTypeEnum)typeId));


            if (list.Count == 0)
            {
                res.StatusCode = StatusCodes.Status204NoContent;
                // await res.Negotiate(list);
                return;
            }
            else
            {
                res.StatusCode = StatusCodes.Status200OK;
                await res.AsJson(list);
                return;
            }

        }

        private async Task DeleteRootFolder(HttpContext ctx)
        {
            int id = ctx.Request.RouteValues.As<int>("id");
            if (id == 0)
            {
                ctx.Response.StatusCode = StatusCodes.Status400BadRequest;

                await ctx.Response.WriteAsync($"Invalid Id: {id}");
            }

            _rootFolderService.Remove(id);
            ctx.Response.StatusCode = StatusCodes.Status200OK;

            await ctx.Response.WriteAsync($"Removed RootFolder with Id: {id}");
        }
    }
}
