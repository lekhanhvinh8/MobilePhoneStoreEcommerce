
namespace MobilePhoneStoreEcommerce.Core.Domain
{
    using System;
    using System.Collections.Generic;

    public partial class Customer
    {
        public Customer()
        {
            this.Carts = new List<Cart>();
            this.Orders = new List<Order>();
            this.Wishlists = new List<Product>();
            this.StarRatings = new List<StarRating>();
            this.Comments = new List<Comment>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string DeliveryAddress { get; set; }

        public virtual Account Account { get; set; }
        public virtual List<Cart> Carts { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual List<Product> Wishlists { get; set; }
        public virtual List<StarRating> StarRatings { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}
