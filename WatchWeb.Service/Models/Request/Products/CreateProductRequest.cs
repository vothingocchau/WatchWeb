using System.ComponentModel.DataAnnotations.Schema;
using WatchWeb.Common.Enums;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request.Category;

namespace WatchWeb.Service.Models.Request.Products
{
    public class CreateProductRequest
    {
        public List<int> CategoryId { get; set; }
        public string Name { get; set; }
        public float UnitPrice { get; set; }
        public int CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string MadeIn { get; set; }
        public string Description { get; set; }
        public string Diameter { get; set; }
        public string Crystal { get; set; }
        public string WaterProof { get; set; }
        public string Machine { get; set; }
        public string Albert { get; set; }
        public int Status { get; set; }


        [NotMapped]
        public List<ProductStatus> ProductStatus { get; set; } = new List<ProductStatus>()
        {
            new ProductStatus { Id = (int)ProductStatusEnumStatus.ACTIVE,DislayName= "Hoạt Động"},
            new ProductStatus { Id = (int)ProductStatusEnumStatus.INACTIVE,DislayName= "Không Hoạt Động"},
        };

        [NotMapped]

        public List<CategorySimpleDto> Categories { get; set; }

        [NotMapped]
        public string ImageUrl { get; set; }    

    }


    public class ProductStatus
    {
        public int Id { get; set; }
        public string DislayName { get; set; }
    }
}
