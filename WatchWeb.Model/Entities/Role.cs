namespace WatchWeb.Model.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

        public ICollection<RolePermission> RolePermission { get; set; }
    }
}
