using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Core.Dtos
{
    public class CartDto
    {
        public CartDto(Cart cart)
        {
            this.ProductID = cart.ProductID;
            this.CustomerID = cart.CustomerID;
            this.Amounts = cart.Amount;
            this.ProductName = cart.Product.Name;
            this.SellerName = cart.Product.Seller.Name;
            this.UnitPrice = cart.Product.Price;
            this.QuantityOfProduct = cart.Product.Quantity;
            this.Amount = this.Amounts * this.UnitPrice;
        }
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public int Amounts { get; set; }
        public string ProductName { get; set; }
        public string SellerName { get; set; }
        public float UnitPrice { get; set; }
        public int QuantityOfProduct { get; set; }
        public float Amount { get; set; } // Amount mean total cost
    }
}