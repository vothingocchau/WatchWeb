namespace WatchWeb.Model.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock {  get; set; }
        public int CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string? MadeIn { get; set; }
        public string? Description { get; set; }
        public int Diameter { get; set; }
        public string? Crystal {  get; set; }
        public string? WaterProof { get; set; }
        public string? Machine { get; set; }
        public string? Albert { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<ProductCategory> ProductCategory { get; set; }
    }
}
