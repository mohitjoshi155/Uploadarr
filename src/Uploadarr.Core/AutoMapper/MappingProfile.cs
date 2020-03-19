using AutoMapper;
using Uploadarr.Data;

namespace Uploadarr.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RootFolder, RootFolderDTO>().ReverseMap();
            CreateMap<RootFolderType, RootFolderTypeDTO>()
                .ForSourceMember(x => x.ToEnum, opt => opt.DoNotValidate())
                .ReverseMap();
        }
    }
}
