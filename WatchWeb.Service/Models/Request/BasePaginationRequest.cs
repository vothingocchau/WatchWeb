namespace WatchWeb.Service.Models.Request
{
    public class BasePaginationRequest
    {
        public string KeyWord { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
