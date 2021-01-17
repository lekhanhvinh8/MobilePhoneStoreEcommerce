using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MobilePhoneStoreEcommerce.Core.Dtos
{
    public class OrderDto
    {
        public OrderDto()
        {
            this.ProductOfOrderDtos = new List<ProductsOfOrderDto>();
        }
        public OrderDto(Order order)
        {
            this.OrderID = order.ID;
            this.CustomerID = order.CustomerID;
            this.OrderTime = order.OrderTime;
            this.Status = order.Status;

            this.ProductOfOrderDtos = new List<ProductsOfOrderDto>();
            foreach (var product in order.ProductsOfOrders)
            {
                this.ProductOfOrderDtos.Add(new ProductsOfOrderDto(product));
            }

            //var totalOrderCost = new MobilePhoneStoreDBMSEntities().SPGetTotalOrderCost(order.OrderID);

            //this.TotalOrderCost = totalOrderCost.ToArray()[0];

        }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public System.DateTime OrderTime { get; set; }
        public Nullable<int> Status { get; set; }
        public List<ProductsOfOrderDto> ProductOfOrderDtos { get; set; }
        public int? TotalOrderCost { get; set; }
    }
}