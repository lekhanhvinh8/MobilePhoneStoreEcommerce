namespace MobilePhoneStoreEcommerce.Core.Domain
{
    using System;
    using System.Collections.Generic;

    public partial class Category
    {
        public Category()
        {
            this.Products = new List<Product>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
