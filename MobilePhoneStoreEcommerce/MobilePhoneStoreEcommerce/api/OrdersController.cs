using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Domain;
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
        public List<OrderDto> GetAllShippingOrder(int sellerID)
        {
            if (!IsSellerAuthorized(sellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var orderDtos = new List<OrderDto>();

            foreach (var order in this._unitOfWork.Orders.GetAllThenOrderByDate(s => s.SellerID == sellerID && (s.Status == OrderStates.Confirmed || s.Status == OrderStates.Paid || s.Status == OrderStates.Success)))
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
            var orders = this._unitOfWork.Orders.GetAllThenOrderByDate(customerID);

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

            foreach (var productsOfOrder in order.ProductsOfOrders)
            {
                this._unitOfWork.Products.Load(p => p.ID == productsOfOrder.ProductID);
            }
            this._unitOfWork.Sellers.Load(s => s.ID == order.SellerID);
            this._unitOfWork.Customers.Load(c => c.ID == order.CustomerID);

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
            {
                if(order.Status == OrderStates.Paid || order.DeliveryDate <= DateTime.Now)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            foreach (var productOfOrder in order.ProductsOfOrders)
            {
                this._unitOfWork.Products.Load(p => p.ID == productOfOrder.ProductID);

                productOfOrder.Product.Quantity += productOfOrder.Amount;
            }
            
            order.Status = OrderStates.Canceled;
            this._unitOfWork.Complete();
            
        }
        [HttpGet]
        public void DeleteOrder(int orderID)
        {
            var order = this._unitOfWork.Orders.Get(orderID);

            if (order == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (order.Status != OrderStates.Canceled)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            order.ProductsOfOrders.Clear(); //Delete all products in order
            this._unitOfWork.Orders.Remove(order);
            this._unitOfWork.Complete();
        }

        [HttpGet]
        public List<OrderDto> CreateOrdersAndDeleteCart(int customerID, string deliveryAddress)
        {
            //If a cart of customer has products of different sellers --> create many orders

            var carts = this._unitOfWork.Carts.Find(c => c.CustomerID == customerID);
            if (carts.Count() == 0)
                throw new Exception("Empty Cart !!!");

            var orders = new List<Order>();
            var timeNow = DateTime.Now;
            var timeDelivery = timeNow.AddDays(7);

            foreach (var cart in carts)
            {
                if (cart.Amount == 0)
                    throw new Exception("Amount of product is 0");

                this._unitOfWork.Products.Load(p => p.ID == cart.ProductID);
                this._unitOfWork.Sellers.Load(s => s.ID == cart.Product.SellerID);

                var productsOfOrder = new ProductsOfOrder();

                if (cart.Amount > cart.Product.Quantity)
                    throw new Exception("The product is out of stock");

                productsOfOrder.ProductID = cart.ProductID;
                productsOfOrder.Amount = cart.Amount;
                cart.Product.Quantity -= cart.Amount;

                if (IsExistSellerID(orders, cart.Product))
                {
                    foreach (var order in orders)
                    {
                        if(order.SellerID == cart.Product.SellerID)
                        {
                            order.ProductsOfOrders.Add(productsOfOrder);
                            break;
                        }
                    }
                }
                else
                {
                    var order = new Order();
                    order.CustomerID = customerID;
                    order.DeliveryAddress = deliveryAddress;
                    order.OrderTime = timeNow;
                    order.DeliveryDate = timeDelivery;
                    order.ShippingCost = Shipping.ShippingCost;
                    order.SellerID = cart.Product.SellerID;
                    order.ProductsOfOrders.Add(productsOfOrder);

                    orders.Add(order);
                }
            }
 
            this._unitOfWork.Orders.AddRange(orders);
            this._unitOfWork.Carts.RemoveRange(carts);
            this._unitOfWork.Complete();

            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                orderDtos.Add(new OrderDto(order));
            }

            return orderDtos;
        }
        [HttpGet]
        public void Delivery(int orderID)
        {
            var order = this._unitOfWork.Orders.Get(orderID);
            if (order == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            order.Status = OrderStates.Success;
            this._unitOfWork.Complete();
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

        private bool IsExistSellerID(List<Order> orders, Product product)
        {

            foreach (var order in orders)
            {
                if (order.SellerID == product.SellerID)
                    return true;
            }

            return false;
        }
    }
}
