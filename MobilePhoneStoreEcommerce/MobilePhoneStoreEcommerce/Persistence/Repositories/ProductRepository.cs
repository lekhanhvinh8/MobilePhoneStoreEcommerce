using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.Repositories
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context)
            :base(context)
        {

        }

        public void Create(ProductForSellerDto productForSellerDto, byte[] avatar)
        {
            var product = new Product();

            product.Name = productForSellerDto.Name;
            product.Description = productForSellerDto.Description;
            product.ProducerID = productForSellerDto.ProducerID;
            product.CategoryID = productForSellerDto.CategoryID;
            product.Price = productForSellerDto.Price;
            product.Quantity = productForSellerDto.Quantity;
            product.Status = productForSellerDto.Status;
            product.SellerID = productForSellerDto.SellerID;
            foreach (var specificationValueDto in productForSellerDto.SpecificationValuesDtos)
            {
                var specificationValue = this.Context.Set<SpecificationValue>().SingleOrDefault(s => s.ProductSpecificationID == specificationValueDto.SpecificationID
                                                                                                  && s.Value == specificationValueDto.Value);

                if (specificationValue == null)
                    throw new Exception("specification value not found");

                product.SpecificationValues.Add(specificationValue);
            }

            product.AvatarOfProduct = new AvatarOfProduct();
            product.AvatarOfProduct.Avatar = avatar;

            this.Context.Set<Product>().Add(product);
        }
    }
}