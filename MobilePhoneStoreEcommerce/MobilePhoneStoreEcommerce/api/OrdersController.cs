using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Persistence.Consts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace MobilePhoneStoreEcommerce.api
{
    public class OrdersController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        [HttpGet]
        public List<OrderDto> GetListByStatus(int status)
        {
            var orders = this._unitOfWork.Orders.Find(o => o.Status == status).ToList();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                orderDtos.Add(new OrderDto(order));
            }

            return orderDtos;
        }

        [HttpGet]
        public List<OrderDto> GetList(int customerID)
        {
            var orders = this._unitOfWork.Orders.Find(o => o.CustomerID == customerID).ToList();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                orderDtos.Add(new OrderDto(order));
            }

            return orderDtos;
        }
        [HttpGet]
        public List<OrderDto> GetList(int customerID, DateTime orderTime)
        {
            var orders = this._unitOfWork.Orders.Find(o => o.CustomerID == customerID && o.OrderTime == orderTime).ToList();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                orderDtos.Add(new OrderDto(order));
            }

            return orderDtos;
        }
        [HttpGet]
        public OrderDto Get(int orderID)
        {
            var order = this._unitOfWork.Orders.Get(orderID);

            if (order == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new OrderDto(order);
        }
        [HttpGet]
        public void ConfirmOrder(int orderID)
        {
            var order = this._unitOfWork.Orders.Get(orderID);

            if (order == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (order.Status != OrderStates.Pending)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            order.Status = OrderStates.Confirmed;

            this._unitOfWork.Complete();
        }

        [HttpGet]
        public void CancelOrder(int orderID)
        {
            var order = this._unitOfWork.Orders.Get(orderID);

            if (order == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (order.Status != OrderStates.Pending)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var isSuccess = new ObjectParameter("isSuccess", typeof(bool));

            //this._unitOfWork.CancelAnOrder(orderID, OrderStates.Canceled, isSuccess);

            if (!(bool)isSuccess.Value)
                throw new Exception("Failure canceling an order");
        }
        [HttpGet]
        public void DeleteOrder(int orderID)
        {
            var order = this._unitOfWork.Orders.Get(orderID);

            if (order == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (order.Status != OrderStates.Canceled)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var isSuccess = new ObjectParameter("isSuccess", typeof(bool));

            //this._unitOfWork.DeleteAnOrder(orderID, OrderStates.Canceled, isSuccess);

            if (!(bool)isSuccess.Value)
                throw new Exception("Failure deleting an order");
        }
    }
}
