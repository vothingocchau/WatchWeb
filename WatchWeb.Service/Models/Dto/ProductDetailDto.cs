using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchWeb.Service.Models.Dto
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
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
        public bool IsDeleted { get; set; }
        public string ImageUrl { get; set; }
    }
}
