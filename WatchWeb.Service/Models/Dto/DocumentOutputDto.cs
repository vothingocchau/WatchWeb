namespace WatchWeb.Service.Models.Dto
{
    public class DocumentOutputDto
    {
        public int Id { get; set; }
        public int ReferenceId { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}
