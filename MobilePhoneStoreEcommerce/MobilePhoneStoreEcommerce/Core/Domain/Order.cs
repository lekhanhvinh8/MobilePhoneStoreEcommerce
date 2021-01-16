
namespace MobilePhoneStoreEcommerce.Core.Domain
{
    using System;
    using System.Collections.Generic;

    public partial class Order
    {
        public Order()
        {
            this.ProductsOfOrders = new List<ProductsOfOrder>();
        }

        public int ID { get; set; }
        public DateTime OrderTime { get; set; }
        public int Status { get; set; }
        public int CustomerID { get; set; }
        public int? InvoiceID { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual List<ProductsOfOrder> ProductsOfOrders { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
