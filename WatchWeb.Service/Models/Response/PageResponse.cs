namespace WatchWeb.Service.Models.Response
{
    public class PageResponse<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public T Data {  get; set; }
    }
}
