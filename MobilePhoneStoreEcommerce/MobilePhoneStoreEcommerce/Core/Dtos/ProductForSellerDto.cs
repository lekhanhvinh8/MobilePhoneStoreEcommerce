using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobilePhoneStoreEcommerce.Core.Dtos
{
    public class ProductForSellerDto
    {
        public ProductForSellerDto()
        {
            this.SpecificationValuesDtos = new List<SpecificationValueDto>();
        }

        public ProductForSellerDto(Product product)
        {
            if (product == null)
                throw new Exception("product object is null");

            this.ProductID = product.ID;
            this.Name = product.Name;
            this.Description = product.Description;
            this.CategoryID = product.CategoryID;
            this.CategoryName = product.Category.Name;
            this.Price = product.Price;
            this.ProducerID = product.ProducerID;
            this.ProducerName = product.Producer.Name;
            this.Quantity = product.Quantity;
            this.Status = product.Status;
            this.SellerID = product.SellerID;
            this.SpecificationValuesDtos = new List<SpecificationValueDto>();

            foreach (var specificationValue in product.SpecificationValues)
            {
                this.SpecificationValuesDtos.Add(new SpecificationValueDto(specificationValue));
            }
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        [Required]
        public int ProducerID { get; set; }
        public string ProducerName { get; set; }

        [Required]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<SpecificationValueDto> SpecificationValuesDtos { get; set; }

        [Required]
        public int SellerID { get; set; }
        public Product CreateModel()
        {
            Product product = new Product();

            product.Name = this.Name;
            product.Description = this.Description;
            product.CategoryID = this.CategoryID;
            product.Price = this.Price;
            product.ProducerID = this.ProducerID;
            product.Quantity = this.Quantity;
            product.Status = this.Status;
            product.SellerID = this.SellerID;

            return product;
        }
        public void UpdateModel(Product product)
        {
            product.Name = this.Name;
            product.Description = this.Description;
            product.CategoryID = this.CategoryID;
            product.Price = this.Price;
            product.ProducerID = this.ProducerID;
            product.Quantity = this.Quantity;
            product.Status = this.Status;
        }
    }
}