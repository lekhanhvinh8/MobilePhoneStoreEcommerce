using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Core.Domain
{
    public class Seller
    {
        public Seller()
        {
            this.Products = new List<Product>();
            this.Orders = new List<Order>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WarehouseAddress { get; set; }
        public virtual Account Account { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual List<Order> Orders { get; set; } 

    }
}