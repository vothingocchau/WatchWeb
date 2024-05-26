using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WatchWeb.Service.Models.Dto;

namespace WatchWeb.Service.Models.Request.Users
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập tên người dùng")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tài khoản người dùng")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email người dùng")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập tên đúng định dạng email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Vui lòng nhập số điện thoại định dạng")]
        public string Phone { get; set; }
        public bool Active { get; set; }
        public int Gender { get; set; }
        public List<int> ListRoleIds { get; set; }

        [NotMapped]
        public string ImageUrl { get; set; }

        [NotMapped]
        public List<UserStatus> UserStatus { get; set; } = new List<UserStatus>()
        {
            new UserStatus { Id = true,DislayName= "Hoạt Động"},
            new UserStatus { Id = false,DislayName= "Không Hoạt Động"},
        };

        [NotMapped]
        public List<RoleSimpleDto> UserRoles { get; set; }
    }

    public class UserStatus
    {
        public bool Id { get; set; }
        public string DislayName { get; set; }
    }
}
