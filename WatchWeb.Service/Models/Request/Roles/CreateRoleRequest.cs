namespace WatchWeb.Service.Models.Request.Roles
{
    public class CreateRoleRequest
    {
        public string Name { get; set; }
        public int Status { get; set; }

        public List<int> Permission { get; set;}
    }
}
