using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Core.Domain
{
    public class StarRating
    {
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int NumberOfStart { get; set; }

        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}