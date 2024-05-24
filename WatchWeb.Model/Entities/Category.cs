namespace WatchWeb.Model.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int? ParentId { get; set; }
    }
}
