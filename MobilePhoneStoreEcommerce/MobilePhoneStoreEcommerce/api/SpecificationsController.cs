using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
namespace MobilePhoneStoreEcommerce.api
{
    public class SpecificationsController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        public SpecificationsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public List<ProductSpecificationDto> GetAll()
        {
            var specifications = new List<ProductSpecificationDto>();

            foreach (var specification in this._unitOfWork.ProductSpecifications.GetAll())
            {
                specifications.Add(new ProductSpecificationDto(specification));
            }
            return specifications;
        }
        public ProductSpecificationDto Get(int id)
        {
            var specification = this._unitOfWork.ProductSpecifications.Get(id);

            if (specification == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new ProductSpecificationDto(specification);
        }
        [HttpPost]
        public ProductSpecificationDto Create(ProductSpecificationDto specificationDto)
        {
            var specification = specificationDto.ToSpecification();

            this._unitOfWork.ProductSpecifications.Add(specification);
            this._unitOfWork.Complete();

            return new ProductSpecificationDto(specification);
        }

        [HttpPut]
        public ProductSpecificationDto Update(ProductSpecificationDto specificationDto)
        {
            var specification = this._unitOfWork.ProductSpecifications.Get(specificationDto.SpecificationID);

            if (specification == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            specification.Name = specificationDto.Name;
            specification.Description = specificationDto.Description;

            this._unitOfWork.Complete();

            return new ProductSpecificationDto(specification);
        }

        [HttpDelete]
        public ProductSpecificationDto Delete(int iD)
        {
            var specification = this._unitOfWork.ProductSpecifications.Get(iD);

            if (specification == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            foreach (var specificationValue in specification.SpecificationValues.ToList())
            {
                this._unitOfWork.SpecificationValues.Remove(specificationValue);
            }
            this._unitOfWork.ProductSpecifications.Remove(specification);

            this._unitOfWork.Complete();

            return new ProductSpecificationDto(specification);
        }
    }
}
