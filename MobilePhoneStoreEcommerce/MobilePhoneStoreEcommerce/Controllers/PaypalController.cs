using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Models;
using MobilePhoneStoreEcommerce.Persistence.Consts;
using PayPal;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;

namespace MobilePhoneStoreEcommerce.Controllers
{
    public class PaypalController : Controller
    {
        private Payment payment;
        private readonly IUnitOfWork _unitOfWork;
        public PaypalController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public ActionResult Payment(int orderID)
        {
            //getting the apiContext as earlier
            APIContext apiContext = ConfigurationPaypal.GetAPIContext();

            var ticks = DateTime.Now.Ticks;
            var guidInvoiceID = Guid.NewGuid().ToString();
            string invoiceID = ticks.ToString() + '-' + guidInvoiceID; //guid created by combining ticks and guid

            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class
                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/Payment?orderID=" + orderID + "&";
                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution
                    var guid = Convert.ToString(new Random().Next(100000));
                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid, orderID, invoiceID);
                    //get links returned from paypal in response to Create function call
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters
                    // from the previous call to the function Create
                    // Executing a payment
                    var guid = Request.Params["guid"];
                    //var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }

                    //Payment success, create invoice
                    var order = this._unitOfWork.Orders.Get(orderID);
                    if (order == null)
                        throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);


                    var invoice = new MobilePhoneStoreEcommerce.Core.Domain.Invoice();
                    invoice.DateOfInvoice = DateTime.Now;
                    invoice.ID = executedPayment.transactions[0].invoice_number;
                    invoice.TotalCost = double.Parse(executedPayment.transactions[0].amount.total);

                    order.Status = OrderStates.Paid;
                    order.Invoice = invoice;

                    this._unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error" + ex.Message);
                return View("FailureView");
            }

            return View("SuccessView");
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, int orderID, string invoiceID)
        {
            var order = this._unitOfWork.Orders.Get(orderID);
            if (order == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            var itemList = new ItemList() { items = new List<Item>() };
            var tax = 1;
            var shipping = order.ShippingCost;
            var subtotal = 0;

            foreach (var productsOfOrder in order.ProductsOfOrders)
            {
                this._unitOfWork.Products.Load(p => p.ID == productsOfOrder.ProductID);

                itemList.items.Add(new Item()
                {
                    //Thông tin đơn hàng
                    name = productsOfOrder.Product.Name,
                    currency = "USD",
                    price = (productsOfOrder.Product.Price / 23000).ToString(),
                    quantity = productsOfOrder.Amount.ToString(),
                    sku = order.SellerID.ToString(),
                });

                subtotal += (productsOfOrder.Product.Price / 23000) * productsOfOrder.Amount;
            }

            //Các giá trị bao gồm danh sách sản phẩm, thông tin đơn hàng
            //Sẽ được thay đổi bằng hành vi thao tác mua hàng trên website
            
            //Hình thức thanh toán qua paypal
            var payer = new Payer() { payment_method = "paypal" };
            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };
            //các thông tin trong đơn hàng
            var details = new Details()
            {
                tax = tax.ToString(),
                shipping = shipping.ToString(),
                subtotal = subtotal.ToString(),
            };
            //Đơn vị tiền tệ và tổng đơn hàng cần thanh toán
            var amount = new Amount()
            {
                currency = "USD",
                total = (tax + shipping + subtotal).ToString(), // Total must be equal to sum of shipping, tax and subtotal. 
                details = details,
            };
            var transactionList = new List<Transaction>();
            //Tất cả thông tin thanh toán cần đưa vào transaction
            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = invoiceID, // must be unique each transaction
                amount = amount,
                item_list = itemList,
            });
            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls,
            };

            // Create a payment using a APIContext
            return payment.Create(apiContext);
        }
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            payment = new Payment() { id = paymentId };
            try
            {
                return payment.Execute(apiContext, paymentExecution);
            }
            catch (PaymentsException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}