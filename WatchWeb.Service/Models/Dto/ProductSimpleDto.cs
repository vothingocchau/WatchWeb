namespace WatchWeb.Service.Models.Dto
{
    public class ProductSimpleDto
    {
        public int Id { get; set; }
        public List<ProductSimpleDto> Categories {  get; set; }
        public string Name { get; set; }
        public string UnitPrice { get; set; }
    }
}
