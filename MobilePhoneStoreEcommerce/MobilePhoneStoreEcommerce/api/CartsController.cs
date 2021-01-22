using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MobilePhoneStoreEcommerce.api
{
    
    public class CartsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountAuthentication _accountAuthentication;

        public CartsController(IUnitOfWork unitOfWork, IAccountAuthentication accountAuthentication)
        {
            this._unitOfWork = unitOfWork;
            this._accountAuthentication = accountAuthentication;
        }
        [HttpGet]
        public List<CartDto> GetAll(int customerID)
        {
            var cartDtos = new List<CartDto>();

            foreach (var cart in this._unitOfWork.Carts.Find(c => c.CustomerID == customerID))
            {
                //Load related objects
                this._unitOfWork.Products.Load(p => p.ProducerID == cart.ProductID);
                this._unitOfWork.Sellers.Load(s => s.ID == cart.Product.SellerID);

                cartDtos.Add(new CartDto(cart));
            }

            return cartDtos;
        }

        public int GetAmountOfCart(int customerID, int productID)
        {
            var cart = this._unitOfWork.Carts.SingleOrDefault(c => c.CustomerID == customerID && c.ProductID == productID);

            if (cart == null)
                return 0;

            return cart.Amount;
        }

        public ProductForCartDto GetProductForCart(int productID)
        {
            var product = this._unitOfWork.Products.Get(productID);
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new ProductForCartDto(product);
        }
        [HttpGet]
        public void Add(int customerID, int productID)
        {
            var cartInDb = this._unitOfWork.Carts.SingleOrDefault(c => c.CustomerID == customerID && c.ProductID == productID);

            var product = this._unitOfWork.Products.Get(productID);
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (cartInDb == null)
            {
                if (product.Quantity < 1)
                    throw new Exception("The product is out of stock");

                var cart = new Cart();
                cart.CustomerID = customerID;
                cart.ProductID = productID;
                cart.Amount = 1;

                this._unitOfWork.Carts.Add(cart);
                this._unitOfWork.Complete();

                return;
            }

            if (product.Quantity < cartInDb.Amount + 1)
                throw new Exception("The product is out of stock");

            cartInDb.Amount += 1;
            this._unitOfWork.Complete();
        }

        [HttpGet]
        public int UpdateAmount(int customerID, int productID, int amount)
        {
            //return quantity of product if required amount is greater than it
            // or return amount if the quantity of product is sufficient

            var cartInDb = this._unitOfWork.Carts.SingleOrDefault(c => c.CustomerID == customerID && c.ProductID == productID);
            if (cartInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var product = this._unitOfWork.Products.Get(productID);
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (product.Quantity < amount)
            {
                cartInDb.Amount = product.Quantity;

                if (cartInDb.Amount == 0)
                    this._unitOfWork.Carts.Remove(cartInDb);
                this._unitOfWork.Complete();

                return product.Quantity;
            }

            cartInDb.Amount = amount;
            this._unitOfWork.Complete();

            return cartInDb.Amount;
        }

        [HttpDelete]
        public void Delete(int customerID, int productID)
        {
            Cart cart = this._unitOfWork.Carts.SingleOrDefault(c => c.CustomerID == customerID && c.ProductID == productID);

            if (cart == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._unitOfWork.Carts.Remove(cart);
            this._unitOfWork.Complete();

            return;
        }
    }
}
