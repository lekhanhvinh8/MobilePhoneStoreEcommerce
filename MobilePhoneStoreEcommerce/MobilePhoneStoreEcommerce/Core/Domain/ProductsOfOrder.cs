
namespace MobilePhoneStoreEcommerce.Core.Domain
{
    using System;
    using System.Collections.Generic;

    public partial class ProductsOfOrder
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Amount { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
