using WatchWeb.Service.Models.Dto;

namespace WatchWeb.Service.Models.Response
{
    public class PaginationProductResponse
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<ProductSimpleDto> Data { get; set; }
    }
}
