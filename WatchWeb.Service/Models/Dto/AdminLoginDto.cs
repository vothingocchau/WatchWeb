namespace WatchWeb.Service.Models.Dto
{
    public class AdminLoginDto
    {
        public List<int> RoleIds { get; set; }
        public int UserId { get; set; } 
        public string Name { get; set; }
    }
}
