using AutoMapper;
using WatchWeb.Model.Entities;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request.Roles;

namespace WatchWeb.Service.Models.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserAccount, AdminLoginDto>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(z => z.Id))
                .ForMember(x => x.RoleIds, opt => opt.MapFrom(z => z.UserRole.Select(x => x.RoleId).ToList()));


            CreateMap<CreateRoleRequest, Role>();
            CreateMap<UpdateRoleRequest, Role>();
            CreateMap<Role, RoleSimpleDto>();
            CreateMap<Role, RoleDetailDto>();
            CreateMap<RoleDetailDto, UpdateRoleRequest>();
        }
    }
}
