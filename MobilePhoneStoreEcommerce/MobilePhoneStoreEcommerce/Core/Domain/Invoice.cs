using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Core.Domain
{
    public class Invoice
    {
        public string ID { get; set; }
        public DateTime DateOfInvoice { get; set; }
        public double TotalCost { get; set; }

        public virtual Order Order { get; set; }
    }
}