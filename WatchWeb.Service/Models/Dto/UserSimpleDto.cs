namespace WatchWeb.Service.Models.Dto
{
    public class UserSimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public int Gender { get; set; }
        public bool Active { get; set; }
        public string Phone { get; set; }
        public string TypeAccount { get; set; }
    }
}
