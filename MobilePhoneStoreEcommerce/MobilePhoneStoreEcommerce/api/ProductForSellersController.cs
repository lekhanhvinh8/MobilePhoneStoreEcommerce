using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Data.Entity.Core.Objects;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Core.Services;
using MobilePhoneStoreEcommerce.Persistence.Consts;

namespace MobilePhoneStoreEcommerce.api
{
    public class ProductForSellersController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private IAccountAuthentication _accountAuthentication;
        public ProductForSellersController(IUnitOfWork unitOfWork, IAccountAuthentication accountAuthentication)
        {
            this._unitOfWork = unitOfWork;
            this._accountAuthentication = accountAuthentication;
        }

        public List<ProductForSellerDto> GetAll(int sellerID)
        {
            if (!IsAuthorized(sellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var productForSellersDto = new List<ProductForSellerDto>();

            foreach (var product in this._unitOfWork.Products.Find(p => p.SellerID == sellerID))
            {
                //Load related objects
                this._unitOfWork.Categories.Load(c => c.ID == product.CategoryID);
                this._unitOfWork.Producers.Load(p => p.ID == product.ProducerID);

                productForSellersDto.Add(new ProductForSellerDto(product));
            }

            return productForSellersDto;
        }

        public ProductForSellerDto Get(int productID)
        {
            var product = this._unitOfWork.Products.Get(productID);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!IsAuthorized(product.SellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return new ProductForSellerDto(product);
        }

        [System.Web.Http.HttpGet]
        public bool ModifyStatus(int productID, bool status)
        {
            var product = this._unitOfWork.Products.Get(productID);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!IsAuthorized(product.SellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            product.Status = status;
            this._unitOfWork.Complete();

            return status;
        }

        [System.Web.Http.HttpPost]
        public ProductForSellerDto Create()
        {
            var productForSellerDto = new ProductForSellerDto();

            var form = HttpContext.Current.Request.Form;

            //Get data
            productForSellerDto.Name = form["name"].ToString();
            productForSellerDto.Description = form["description"].ToString();
            productForSellerDto.ProducerID = int.Parse(form["producerID"].ToString());
            productForSellerDto.CategoryID = int.Parse(form["categoryID"].ToString());
            productForSellerDto.Price = int.Parse(form["price"].ToString());
            productForSellerDto.Quantity = int.Parse(form["quantity"].ToString());
            productForSellerDto.Status = Boolean.Parse(form["status"]);
            productForSellerDto.SellerID = int.Parse(form["sellerID"]);
            productForSellerDto.SpecificationValuesDtos = JsonConvert.DeserializeObject<List<SpecificationValueDto>>(form.Get("specificationValuesDtos"));

            //Get image file
            var httpPostedFile = HttpContext.Current.Request.Files["imageFile"];
            BinaryReader reader = new BinaryReader(httpPostedFile.InputStream);
            var imageFile = reader.ReadBytes(httpPostedFile.ContentLength);

            if (!IsAuthorized(productForSellerDto.SellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            this._unitOfWork.Products.Create(productForSellerDto, imageFile);
            this._unitOfWork.Complete();

            var product = this._unitOfWork.Products.SingleOrDefault(p => p.Name == productForSellerDto.Name);

            //Load related objects
            this._unitOfWork.Categories.Load(c => c.ID == product.CategoryID);
            this._unitOfWork.Producers.Load(p => p.ID == product.ProducerID);
            

            return new ProductForSellerDto(product);
        }

        [System.Web.Http.HttpPut]
        public ProductForSellerDto Update(ProductForSellerDto productForSellerDto)
        {
            var product = this._unitOfWork.Products.Get(productForSellerDto.ProductID);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!IsAuthorized(product.SellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            productForSellerDto.UpdateModel(product);

            product.SpecificationValues.Clear();
            
            foreach (var specificationValueDto in productForSellerDto.SpecificationValuesDtos)
            {
                var specificationValue = this._unitOfWork.SpecificationValues.SingleOrDefault(s => s.ProductSpecificationID == specificationValueDto.SpecificationID
                                                                                          && s.Value == specificationValueDto.Value);
                if (specificationValue == null)
                    throw new Exception("Can not find this specification or value");

                product.SpecificationValues.Add(specificationValue);
            }

            this._unitOfWork.Complete();

            var productInDb = this._unitOfWork.Products.SingleOrDefault(p => p.ID == product.ID);

            //Load related objects
            this._unitOfWork.Categories.Load(c => c.ID == productInDb.CategoryID);
            this._unitOfWork.Producers.Load(p => p.ID == productInDb.ProducerID);

            return new ProductForSellerDto(productInDb);
        }

        [System.Web.Http.HttpDelete]
        public ProductForSellerDto Delete(int productID)
        {
            var product = this._unitOfWork.Products.SingleOrDefault(p => p.ID == productID);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!IsAuthorized(product.SellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var productDto = new ProductForSellerDto(product);

            product.SpecificationValues.Clear();

            this._unitOfWork.AvatarOfProducts.Remove(product.AvatarOfProduct);
            this._unitOfWork.Products.Remove(product); // after deletion, this product no longer has category and producer
            this._unitOfWork.Complete();

            return productDto;
        }
        [HttpGet]
        public bool IsExist(string productName)
        {
            var product = this._unitOfWork.Products.SingleOrDefault(p => p.Name == productName);

            if (product == null)
                return false;

            return true;
        }
        [HttpGet]
        public bool IsExistExcept(string productName, int productID)
        {
            var product = this._unitOfWork.Products.SingleOrDefault(p => p.Name == productName);

            if (product == null)
                return false;

            if (product.ID == productID)
                return false;

            return true;
        }
        
        private bool IsAuthorized(int sellerID)
        {
            var sessionSellerID = HttpContext.Current.Session[SessionNames.SellerID];

            if (!this._accountAuthentication.IsAuthentic(sellerID, sessionSellerID))
                return false;

            return true;
        }
    }
}
