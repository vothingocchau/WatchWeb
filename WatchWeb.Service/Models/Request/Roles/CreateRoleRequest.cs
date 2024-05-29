using WatchWeb.Service.Models.Dto;

namespace WatchWeb.Service.Models.Request.Roles
{
    public class CreateRoleRequest
    {
        public string Name { get; set; }
        public int Status { get; set; }

        public List<int> Permission { get; set;}

        public List<PermissionSimpleDto> Permissions { get; set;}
    }
}
