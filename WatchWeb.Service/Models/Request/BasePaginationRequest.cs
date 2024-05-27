namespace WatchWeb.Service.Models.Request
{
    public class BasePaginationRequest
    {
        public string KeyWord { get; set; }
        public string CategoryId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
