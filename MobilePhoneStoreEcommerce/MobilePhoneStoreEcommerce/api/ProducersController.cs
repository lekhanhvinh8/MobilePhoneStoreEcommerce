using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace MobilePhoneStoreEcommerce.api
{
    public class ProducersController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        public ProducersController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
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
            var producer = producerDto.ToProducer();

            this._unitOfWork.Producers.Add(producer);
            this._unitOfWork.Complete();

            return new ProducerDto(producer);
        }

        [HttpPut]
        public ProducerDto Update(ProducerDto producerDto)
        {
            var producer = this._unitOfWork.Producers.Get(producerDto.ProducerID);

            if (producer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            producer.Name = producerDto.Name;

            this._unitOfWork.Complete();

            return new ProducerDto(producer);
        }

        public ProducerDto Delete(int iD)
        {
            var producer = this._unitOfWork.Producers.Get(iD);

            if (producer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._unitOfWork.Producers.Remove(producer);
            this._unitOfWork.Complete();

            return new ProducerDto(producer);
        }
    }
}
