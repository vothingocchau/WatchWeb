using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchWeb.Model.Entities
{
    public class Address
    {
        public int Id {  get; set; }
        public int UserId { get; set; }
        public string Number { get; set; }
        public string Street { get; set; }
        public string Subdistrict { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Status { get; set; }
    }
}
