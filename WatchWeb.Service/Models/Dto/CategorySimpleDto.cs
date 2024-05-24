using System.ComponentModel.DataAnnotations.Schema;
using WatchWeb.Common.Enums;
using WatchWeb.Service.Models.Request.Category;

namespace WatchWeb.Service.Models.Dto
{
    public class CategorySimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int Status { get; set; }
        public string ImageUrl { get; set; }
        public CategorySimpleDto? Parent { get; set; }
        public List<CategorySimpleDto> Childrens { get; set; }
    }
}
