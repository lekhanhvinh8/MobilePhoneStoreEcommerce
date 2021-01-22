using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Core.Services;
using MobilePhoneStoreEcommerce.Persistence.Consts;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
namespace MobilePhoneStoreEcommerce.api
{
    public class SpecificationsController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private readonly IAccountAuthentication _accountAuthentication;

        public SpecificationsController(IUnitOfWork unitOfWork, IAccountAuthentication accountAuthentication)
        {
            this._unitOfWork = unitOfWork;
            this._accountAuthentication = accountAuthentication;
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
            if (!IsAuthorized())
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var specification = specificationDto.ToSpecification();

            this._unitOfWork.ProductSpecifications.Add(specification);
            this._unitOfWork.Complete();

            return new ProductSpecificationDto(specification);
        }

        [HttpPut]
        public ProductSpecificationDto Update(ProductSpecificationDto specificationDto)
        {
            if (!IsAuthorized())
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

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
            if (!IsAuthorized())
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

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

        private bool IsAuthorized()
        {
            var sessionAdminID = HttpContext.Current.Session[SessionNames.AdminID];

            if (!this._accountAuthentication.IsAuthentic(sessionAdminID))
                return false;

            return true;
        }
    }
}
