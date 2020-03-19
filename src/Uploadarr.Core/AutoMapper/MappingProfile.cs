using AutoMapper;
using Uploadarr.Data;

namespace Uploadarr.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RootFolder, RootFolderDTO>().ReverseMap();
        }
    }
}
