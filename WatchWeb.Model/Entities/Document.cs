using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchWeb.Model.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public int ReferenceId { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}
