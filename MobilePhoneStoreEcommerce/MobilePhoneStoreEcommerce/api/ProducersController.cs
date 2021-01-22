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
    public class ProducersController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private readonly IAccountAuthentication _accountAuthentication;

        public ProducersController(IUnitOfWork unitOfWork, IAccountAuthentication accountAuthentication)
        {
            this._unitOfWork = unitOfWork;
            this._accountAuthentication = accountAuthentication;
        }

        public List<ProducerDto> GetAll()
        {
            var producersDto = new List<ProducerDto>();

            foreach (var producer in this._unitOfWork.Producers.GetAll())
            {
                producersDto.Add(new ProducerDto(producer));
            }

            return producersDto;
        }

        public ProducerDto Get(int iD)
        {
            var producer = this._unitOfWork.Producers.Get(iD);

            if (producer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new ProducerDto(producer);
        }

        [HttpPost]
        public ProducerDto Create(ProducerDto producerDto)
        {
            if (!IsAuthorized())
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var producer = producerDto.ToProducer();

            this._unitOfWork.Producers.Add(producer);
            this._unitOfWork.Complete();

            return new ProducerDto(producer);
        }

        [HttpPut]
        public ProducerDto Update(ProducerDto producerDto)
        {
            if (!IsAuthorized())
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var producer = this._unitOfWork.Producers.Get(producerDto.ProducerID);

            if (producer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            producer.Name = producerDto.Name;

            this._unitOfWork.Complete();

            return new ProducerDto(producer);
        }

        public ProducerDto Delete(int iD)
        {
            if (!IsAuthorized())
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var producer = this._unitOfWork.Producers.Get(iD);

            if (producer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._unitOfWork.Producers.Remove(producer);
            this._unitOfWork.Complete();

            return new ProducerDto(producer);
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
