namespace WatchWeb.Service.Models.Dto
{
    public class RoleDetailDto : RoleSimpleDto
    {
        public List<int> Permission {  get; set; }
    }
}
