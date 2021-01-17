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
            this.amount = productsOfOrder.Amount;
        }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> amount { get; set; }

    }
}