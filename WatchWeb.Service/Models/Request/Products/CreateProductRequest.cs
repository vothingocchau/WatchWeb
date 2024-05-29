using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WatchWeb.Common.Enums;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request.Category;

namespace WatchWeb.Service.Models.Request.Products
{
    public class CreateProductRequest
    {
        public List<int> CategoryId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string Name { get; set; }
        [Range(0, int.MaxValue, ConvertValueInInvariantCulture = true, ErrorMessage = "Vui lòng nhập số dương")]
        [Required(ErrorMessage = "Vui lòng nhập tiền sản phẩm")]
        public decimal UnitPrice { get; set; }
        public int CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string MadeIn { get; set; }
        public string Description { get; set; }
        public int Diameter { get; set; }
        public string Crystal { get; set; }
        public string WaterProof { get; set; }
        public string Machine { get; set; }
        public string Albert { get; set; }
        public int Status { get; set; }
        [Range(0,int.MaxValue)]
        public int Stock { get; set; }


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

        [NotMapped]
        public List<ProductInfo> Crystals { get; set; } = new List<ProductInfo>()
        {
            new ProductInfo {Id = "Kính khoáng (Mineral)",Name ="Kính khoáng (Mineral)"},
            new ProductInfo {Id = "Kính Sapphire",Name ="Kính Sapphire"},
            new ProductInfo {Id = "Nhựa Resin",Name ="Nhựa Resin"},
        };


        [NotMapped]
        public List<ProductInfo> WaterProofs { get; set; } = new List<ProductInfo>()
        {
            new ProductInfo {Id = "3 ATM - Rửa tay, đi mưa",Name ="3 ATM - Rửa tay, đi mưa"},
            new ProductInfo {Id = "5 ATM - Tắm",Name ="5 ATM - Tắm"},
            new ProductInfo {Id = "10 ATM - Bơi",Name ="10 ATM - Bơi"},
            new ProductInfo {Id = "20 ATM - Bơi, lặn",Name ="20 ATM - Bơi, lặn"},
        };

        [NotMapped]
        public List<ProductInfo> Machines { get; set; } = new List<ProductInfo>()
        {
            new ProductInfo {Id = "Eco-Drive",Name ="Eco-Drive"},
            new ProductInfo {Id = "Pin",Name ="Pin"},
        };

        [NotMapped]
        public List<ProductInfo> Alberts { get; set; } = new List<ProductInfo>()
        {
            new ProductInfo {Id = "Thép không gỉ",Name ="Thép không gỉ"},
            new ProductInfo {Id = "Nhựa",Name ="Nhựa"},
            new ProductInfo {Id = "Da thật",Name ="Da thật"},
            new ProductInfo {Id = "Hợp kim",Name ="Hợp kim"},
            new ProductInfo {Id = "Cao su",Name ="Cao su"},
        };

    }


    public class ProductStatus
    {
        public int Id { get; set; }
        public string DislayName { get; set; }
    }


    public class ProductInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
