namespace WatchWeb.Service.Models.Dto
{
    public class ProductSimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Status { get; set; }
    }
}
    