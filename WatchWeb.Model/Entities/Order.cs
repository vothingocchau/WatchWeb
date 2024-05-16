using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchWeb.Model.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public int Status { get; set; }
    }
}
