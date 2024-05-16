using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchWeb.Model.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int ProductId {  get; set; }
        public int OrderId { get; set; }
        public float UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
