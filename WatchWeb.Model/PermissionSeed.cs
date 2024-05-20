using WatchWeb.Model.Entities;

namespace WatchWeb.Model
{
    public static class PermissionSeed
    {
        public static readonly List<Permission> AllPermissions = new List<Permission>
        {
            new Permission { Id = 1, Name = Admin, Status = 1 },
            new Permission { Id = 2, Name = ProductAdmin, Status = 1 },
            new Permission { Id = 3, Name = ProductAdd, Status = 1 },
            new Permission { Id = 4, Name = ProductEdit, Status = 1 },
            new Permission { Id = 5, Name = ProductDelete, Status = 1 },
            new Permission { Id = 6, Name = ProductView, Status = 1 },
            new Permission { Id = 7, Name = ProductViewDetails, Status = 1 },
            new Permission { Id = 8, Name = CategoryAdmin, Status = 1 },
            new Permission { Id = 9, Name = CategoryAdd, Status = 1 },
            new Permission { Id = 10, Name = CategoryEdit, Status = 1 },
            new Permission { Id = 11, Name = CategoryDelete, Status = 1 },
            new Permission { Id = 12, Name = CategoryView, Status = 1 },
            new Permission { Id = 13, Name = CategoryViewDetails, Status = 1 },
            new Permission { Id = 14, Name = BrandAdmin, Status = 1 },
            new Permission { Id = 15, Name = BrandAdd, Status = 1 },
            new Permission { Id = 16, Name = BrandEdit, Status = 1 },
            new Permission { Id = 17, Name = BrandDelete, Status = 1 },
            new Permission { Id = 18, Name = BrandView, Status = 1 },
            new Permission { Id = 19, Name = BrandViewDetails, Status = 1 },
            new Permission { Id = 20, Name = UserAdmin, Status = 1 },
            new Permission { Id = 21, Name = UserAdd, Status = 1 },
            new Permission { Id = 22, Name = UserEdit, Status = 1 },
            new Permission { Id = 23, Name = UserDelete, Status = 1 },
            new Permission { Id = 24, Name = UserView, Status = 1 },
            new Permission { Id = 25, Name = UserViewDetails, Status = 1 },
        };


        // Quyền của Role
        public const string Admin = "admin";

        // Quyền của Product
        public const string ProductAdmin = "product.admin";
        public const string ProductAdd = "product.add";
        public const string ProductEdit = "product.edit";
        public const string ProductDelete = "product.delete";
        public const string ProductView = "product.view";
        public const string ProductViewDetails = "product.view_details";

        // Quyền của Category
        public const string CategoryAdmin = "category.admin";
        public const string CategoryAdd = "category.add";
        public const string CategoryEdit = "category.edit";
        public const string CategoryDelete = "category.delete";
        public const string CategoryView = "category.view";
        public const string CategoryViewDetails = "category.view_details";

        // Quyền của Brand
        public const string BrandAdmin = "brand.admin";
        public const string BrandAdd = "brand.add";
        public const string BrandEdit = "brand.edit";
        public const string BrandDelete = "brand.delete";
        public const string BrandView = "brand.view";
        public const string BrandViewDetails = "brand.view_details";

        // Quyền của User
        public const string UserAdmin = "user.admin";
        public const string UserAdd = "user.add";
        public const string UserEdit = "user.edit";
        public const string UserDelete = "user.delete";
        public const string UserView = "user.view";
        public const string UserViewDetails = "user.view_details";
    }


}
