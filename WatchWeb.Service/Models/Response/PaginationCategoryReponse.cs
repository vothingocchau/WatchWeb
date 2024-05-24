using WatchWeb.Service.Models.Dto;

namespace WatchWeb.Service.Models.Response
{
    public class PaginationCategoryReponse
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<CategorySimpleDto> Data { get; set; }
    }
}
