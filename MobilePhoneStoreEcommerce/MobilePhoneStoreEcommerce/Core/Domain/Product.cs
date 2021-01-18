
namespace MobilePhoneStoreEcommerce.Core.Domain
{
    using System;
    using System.Collections.Generic;

    public partial class Product
    {
        public Product()
        {
            this.Carts = new List<Cart>();
            this.ProductsOfOrders = new List<ProductsOfOrder>();
            this.SpecificationValues = new List<SpecificationValue>();
            this.Wishlists = new List<Customer>();
            this.Comments = new List<Comment>();
            this.StarRatings = new List<StarRating>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int Price { get; set; }
        public int ProducerID { get; set; }
        public int CategoryID { get; set; }
        public int SellerID { get; set; }

        public virtual AvatarOfProduct AvatarOfProduct { get; set; }
        public virtual List<Cart> Carts { get; set; }
        public virtual Category Category { get; set; }
        public virtual Producer Producer { get; set; }
        public virtual List<ProductsOfOrder> ProductsOfOrders { get; set; }
        public virtual List<SpecificationValue> SpecificationValues { get; set; }
        public virtual List<Customer> Wishlists { get; set; }
        public virtual List<StarRating> StarRatings { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
