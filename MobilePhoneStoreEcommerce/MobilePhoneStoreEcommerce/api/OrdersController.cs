using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Core.Services;
using MobilePhoneStoreEcommerce.Persistence.Consts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace MobilePhoneStoreEcommerce.api
{
    public class OrdersController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private readonly IAccountAuthentication _accountAuthentication;

        public OrdersController(IUnitOfWork unitOfWork, IAccountAuthentication accountAuthentication)
        {
            this._unitOfWork = unitOfWork;
            this._accountAuthentication = accountAuthentication;
        }

        [HttpGet]
        public List<OrderDto> GetAll(int sellerID, int status)
        {
            if (!IsSellerAuthorized(sellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var orderDtos = new List<OrderDto>();

            foreach (var order in this._unitOfWork.Orders.GetAllThenOrderByDate(sellerID, status))
            {
                orderDtos.Add(new OrderDto(order));
            }

            return orderDtos;
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


        private bool IsSellerAuthorized(int sellerID)
        {
            var sessionSellerID = HttpContext.Current.Session[SessionNames.SellerID];

            if (!this._accountAuthentication.IsAuthentic(sellerID, sessionSellerID))
                return false;

            return true;
        }

        private bool IsCustomerAuthorized(int customerID)
        {
            var sessionCustomerID = HttpContext.Current.Session[SessionNames.CustomerID];

            if (!this._accountAuthentication.IsAuthentic(customerID, sessionCustomerID))
                return false;

            return true;
        }
    }
}
