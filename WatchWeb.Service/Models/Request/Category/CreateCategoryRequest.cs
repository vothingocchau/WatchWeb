using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WatchWeb.Common.Enums;

namespace WatchWeb.Service.Models.Request.Category
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; }
        public int Status { get; set; }
        public int? ParentId { get; set; }

        [NotMapped]
        public List<CategoryStatus> CategoryStatus { get; set; } = new List<CategoryStatus>()
        {
            new CategoryStatus { Id = (int)CategoryEnumStatus.ACTIVE,DislayName= "Hoạt Động"},
            new CategoryStatus { Id = (int)CategoryEnumStatus.INACTIVE,DislayName= "Không Hoạt Động"},
        };

        [NotMapped]
        public List<CategoryParent> CategoryParent { get; set; }

        [NotMapped]
        public string ImageUrl { get; set; }
    }

    public class CategoryParent
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CategoryStatus
    {
        public int Id { get; set; }
        public string DislayName { get; set; }
    }
}
