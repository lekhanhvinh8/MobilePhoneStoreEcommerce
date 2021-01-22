using MobilePhoneStoreEcommerce.Core.Domain;
using System;

namespace MobilePhoneStoreEcommerce.Core.Dtos
{
    public class ProductsOfOrderDto
    {
        public ProductsOfOrderDto()
        {
        }

        public ProductsOfOrderDto(ProductsOfOrder productsOfOrder)
        {
            this.OrderID = productsOfOrder.OrderID;
            this.ProductID = productsOfOrder.ProductID;
            this.Amount = productsOfOrder.Amount;
            this.SellerID = productsOfOrder.Product.SellerID;
            this.SellerName = productsOfOrder.Product.Seller.Name;
        }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> Amount { get; set; }
        public int SellerID { get; set; }
        public string SellerName { get; set; }


    }
}