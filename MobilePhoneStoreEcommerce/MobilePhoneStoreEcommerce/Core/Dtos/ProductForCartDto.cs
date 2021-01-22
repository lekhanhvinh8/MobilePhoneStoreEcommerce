using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Core.Dtos
{
    public class ProductForCartDto
    {
        public ProductForCartDto(Product product)
        {
            this.ID = product.ID;
            this.Name = product.Name;
            this.Price = product.Price;
            this.Quantity = product.Quantity;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
    }
}