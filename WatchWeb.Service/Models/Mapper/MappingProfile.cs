using AutoMapper;
using WatchWeb.Model.Entities;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request.Category;
using WatchWeb.Service.Models.Request.Products;
using WatchWeb.Service.Models.Request.Roles;
using WatchWeb.Service.Models.Request.Users;

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
            CreateMap<Category, CategorySimpleDto>();
            CreateMap<Category, CategoryParent>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
            CreateMap<Category, UpdateCategoryRequest>();
            CreateMap<Document, DocumentOutputDto>();
            CreateMap<UserAccount, UserSimpleDto>();
            CreateMap<UserAccount, UserDetailDto>();
            CreateMap<CreateUserRequest, UserAccount>();
            CreateMap<UpdateUserRequest, UserAccount>();
            CreateMap<UserAccount, UpdateUserRequest>();
            CreateMap<Product, ProductSimpleDto>();
            CreateMap<ProductSimpleDto, Product>();
            CreateMap<Product, ProductDetailDto>();
            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();
            CreateMap<Product, UpdateProductRequest>();
        }
    }
}
