using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MobilePhoneStoreEcommerce.Core;

namespace MobilePhoneStoreEcommerce.api
{
    public class SpecificationValuesController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public SpecificationValuesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public List<SpecificationValueDto> GetAll()
        {
            var specificationvaluesDto = new List<SpecificationValueDto>();

            foreach (var specificationvalue in this._unitOfWork.SpecificationValues.GetAll())
            {
                specificationvaluesDto.Add(new SpecificationValueDto(specificationvalue));
            }

            return specificationvaluesDto;
        }

        public SpecificationValueDto Get(int specificationID, string value)
        {
            var specification = this._unitOfWork.SpecificationValues.SingleOrDefault(s => s.ProductSpecificationID == specificationID && s.Value == value);

            if (specification == null)
                return null;

            return new SpecificationValueDto(specification);
        }

        [HttpPost]
        public SpecificationValueDto Create(SpecificationValueDto specificationValueDto)
        {
            var specificationValue = specificationValueDto.CreateModel();

            this._unitOfWork.SpecificationValues.Add(specificationValue);
            this._unitOfWork.Complete();

            return new SpecificationValueDto(specificationValue);
        }

        [HttpDelete]
        public SpecificationValueDto Delete(SpecificationValueDto specificationValueDto)
        {
            var specificationValue =  this._unitOfWork.SpecificationValues.SingleOrDefault(s => s.ProductSpecificationID == specificationValueDto.SpecificationID && s.Value == specificationValueDto.Value);

            if (specificationValue == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._unitOfWork.SpecificationValues.Remove(specificationValue);
            this._unitOfWork.Complete();

            return new SpecificationValueDto(specificationValue);
        }
    }
}
