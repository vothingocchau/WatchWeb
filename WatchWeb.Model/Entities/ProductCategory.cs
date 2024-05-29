﻿namespace WatchWeb.Model.Entities
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }


        public virtual Category Category { get; set; }
        public virtual Product Product { get; set; }
    }
}
