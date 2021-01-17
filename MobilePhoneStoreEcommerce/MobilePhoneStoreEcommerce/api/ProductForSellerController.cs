using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Dtos;

namespace MobilePhoneStoreEcommerce.api
{
    public class ProductForSellerController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        public ProductForSellerController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public List<ProductForSellerDto> GetAll()
        {
            var productForSellersDto = new List<ProductForSellerDto>();

            foreach (var product in this._unitOfWork.Products.GetAll())
            {
                productForSellersDto.Add(new ProductForSellerDto(product));
            }

            return productForSellersDto;
        }

        public ProductForSellerDto Get(int productID)
        {
            var product = this._unitOfWork.Products.Get(productID);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new ProductForSellerDto(product);
        }

        [System.Web.Http.HttpGet]
        public bool ModifyStatus(int productID, bool status)
        {
            var product = this._unitOfWork.Products.Get(productID);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            product.Status = status;
            this._unitOfWork.Complete();

            return status;
        }

        [System.Web.Http.HttpPost]
        public ProductForSellerDto Create(ProductForSellerDto productForSellerDto)
        {
            var product = productForSellerDto.CreateModel();

            foreach (var specificationValueDto in productForSellerDto.SpecificationValuesDto)
            {
                var specificationValue = this._unitOfWork.SpecificationValues.SingleOrDefault(s => s.ProductSpecificationID == specificationValueDto.SpecificationID
                                                                                          && s.Value == specificationValueDto.Value);
                if (specificationValue == null)
                    throw new Exception("Can not find this specification or value");

                product.SpecificationValues.Add(specificationValue);
            }


            this._unitOfWork.Products.Add(product);
            this._unitOfWork.Complete();

            var productInDb = this._unitOfWork.Products.SingleOrDefault(p => p.ID == product.ID);


            return new ProductForSellerDto(productInDb);
        }

        [System.Web.Http.HttpPost]
        public ProductForSellerDto CreateProductFullInfo()
        {
            var productDto = new ProductDto();

            var form = HttpContext.Current.Request.Form;

            //Get data
            productDto.Name = form["name"].ToString();
            productDto.Description = form["description"].ToString();
            productDto.ProducerID = int.Parse(form["producerID"].ToString());
            productDto.CategoryID = int.Parse(form["categoryID"].ToString());
            productDto.Price = int.Parse(form["price"].ToString());
            productDto.Quantity = int.Parse(form["quantity"].ToString());
            productDto.Status = Boolean.Parse(form["status"]);
            productDto.SpecificationValuesDto = JsonConvert.DeserializeObject<List<SpecificationValueDto>>(form.Get("specificationValuesDto"));
            //var a = JsonConvert.DeserializeObject(form.Get("item"));

            //serialize specification values to string -- format: "1,16GB|2,128GB|3,2 Sims"
            string specificationValuesString = "";
            foreach (var specificationValueDto in productDto.SpecificationValuesDto)
            {
                string record = specificationValueDto.SpecificationID.ToString() + "," + specificationValueDto.Value;
                specificationValuesString += record + "|";
            }
            if(specificationValuesString.Length > 1)
                specificationValuesString = specificationValuesString.Substring(0, specificationValuesString.Length - 1);

            //Get image file
            var httpPostedFile = HttpContext.Current.Request.Files["imageFile"];
            BinaryReader reader = new BinaryReader(httpPostedFile.InputStream);
            var imageFile = reader.ReadBytes(httpPostedFile.ContentLength);
            productDto.ImageFile = imageFile;

            var isSuccess = new ObjectParameter("isSuccess", typeof(bool));
            /*
            this._unitOfWork.AddNewProduct(productDto.Name
                                        , productDto.Description
                                        , productDto.Quantity
                                        , productDto.Status
                                        , productDto.Price
                                        , productDto.ProducerID
                                        , productDto.CategoryID
                                        , productDto.ImageFile
                                        , specificationValuesString
                                        , isSuccess);
            */
            if (!(bool)isSuccess.Value)
                throw new Exception("Failure inserting sequence of data");

            var product = this._unitOfWork.Products.SingleOrDefault(p => p.Name == productDto.Name);

            if (product == null)
                throw new Exception("Not found");

            return new ProductForSellerDto(product);
        }

        [System.Web.Http.HttpPut]
        public ProductForSellerDto Update(ProductForSellerDto productForSellerDto)
        {
            var product = this._unitOfWork.Products.Get(productForSellerDto.ProductID);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            productForSellerDto.UpdateModel(product);

            product.SpecificationValues.Clear();
            
            foreach (var specificationValueDto in productForSellerDto.SpecificationValuesDto)
            {
                var specificationValue = this._unitOfWork.SpecificationValues.SingleOrDefault(s => s.ProductSpecificationID == specificationValueDto.SpecificationID
                                                                                          && s.Value == specificationValueDto.Value);
                if (specificationValue == null)
                    throw new Exception("Can not find this specification or value");

                product.SpecificationValues.Add(specificationValue);
            }

            this._unitOfWork.Complete();

            var productInDb = this._unitOfWork.Products.SingleOrDefault(p => p.ID == product.ID);


            return new ProductForSellerDto(productInDb);
        }

        public ProductForSellerDto DeleteFullInfo(int productID)
        {
            var product = this._unitOfWork.Products.SingleOrDefault(p => p.ID == productID);


            var isSuccess = new ObjectParameter("isSuccess", typeof(bool));

            //this._unitOfWork.DeleteProduct(productID, isSuccess);

            if (!(bool)isSuccess.Value)
                throw new Exception("Failure deleting product"); // product ID is not found in 2 tables Products and AvatarOfProduct

            return new ProductForSellerDto(product);
        }

        [System.Web.Http.HttpDelete]
        public ProductForSellerDto Delete(int productID)
        {
            var product = this._unitOfWork.Products.SingleOrDefault(p => p.ID == productID);


            var productDto = new ProductForSellerDto(product);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            product.SpecificationValues.Clear();

            this._unitOfWork.Products.Remove(product); // after deletion, this product no longer has category and producer
            this._unitOfWork.Complete();

            return productDto;
        }
    }
}
