using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Services;
using MobilePhoneStoreEcommerce.Persistence.Consts;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MobilePhoneStoreEcommerce.api
{
    public class AvatarOfProductsController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private IAccountAuthentication _accountAuthentication;
        public AvatarOfProductsController(IUnitOfWork unitOfWork, IAccountAuthentication accountAuthentication)
        {
            this._unitOfWork = unitOfWork;
            this._accountAuthentication = accountAuthentication;
        }

        [HttpGet]
        public HttpResponseMessage Get(int productID)
        {
            var avatarOfProduct = this._unitOfWork.AvatarOfProducts.Get(productID);

            if (avatarOfProduct == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            this._unitOfWork.Products.Load(p => p.ID == avatarOfProduct.ProductID);

            return GetResponseMessage(avatarOfProduct);
        }

        [HttpGet]
        public byte[] GetAvatarOfProductAsByte(int productID)
        {
            var avatarOfProduct = this._unitOfWork.AvatarOfProducts.Get(productID);

            if (avatarOfProduct == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            this._unitOfWork.Products.Load(p => p.ID == avatarOfProduct.ProductID);

            if (!IsAuthorized(avatarOfProduct.Product.SellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return avatarOfProduct.Avatar;
        }

        [HttpPost]
        public HttpResponseMessage Upload()
        {
            var httpPostedFile = HttpContext.Current.Request.Files["imageFile"];
            var productID = HttpContext.Current.Request.Form[0];

            BinaryReader reader = new BinaryReader(httpPostedFile.InputStream);

            var image = reader.ReadBytes(httpPostedFile.ContentLength);

            var avatarOfProduct = new AvatarOfProduct();
            avatarOfProduct.ProductID = int.Parse(productID);
            avatarOfProduct.Avatar = image;

            var product = this._unitOfWork.Products.Get(avatarOfProduct.ProductID);
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!IsAuthorized(product.SellerID))
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._unitOfWork.AvatarOfProducts.Add(avatarOfProduct);
            this._unitOfWork.Complete();

            return GetResponseMessage(avatarOfProduct);
        }

        [HttpPut]
        public HttpResponseMessage Update()
        {
            var httpPostedFile = HttpContext.Current.Request.Files["imageFile"];
            var productID = HttpContext.Current.Request.Form[0];

            BinaryReader reader = new BinaryReader(httpPostedFile.InputStream);

            var image = reader.ReadBytes(httpPostedFile.ContentLength);

            var avatarOfProductInDb = this._unitOfWork.AvatarOfProducts.Get(int.Parse(productID));
            if (avatarOfProductInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._unitOfWork.Products.Load(p => p.ID == avatarOfProductInDb.ProductID);

            if (!IsAuthorized(avatarOfProductInDb.Product.SellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            avatarOfProductInDb.Avatar = image;
            this._unitOfWork.Complete();

            return GetResponseMessage(avatarOfProductInDb);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int productID)
        {
            var avatarOfProduct = this._unitOfWork.AvatarOfProducts.Get(productID);

            if (avatarOfProduct == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._unitOfWork.Products.Load(p => p.ID == avatarOfProduct.ProductID);

            if (!IsAuthorized(avatarOfProduct.Product.SellerID))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            this._unitOfWork.AvatarOfProducts.Remove(avatarOfProduct);
            this._unitOfWork.Complete();

            return GetResponseMessage(avatarOfProduct);
        }

        private HttpResponseMessage GetResponseMessage(AvatarOfProduct avatarOfProduct)
        {
            MemoryStream ms = new MemoryStream(avatarOfProduct.Avatar);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = new StreamContent(ms);

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

            return response;
        }

        private bool IsAuthorized(int sellerID)
        {
            var sessionSellerID = HttpContext.Current.Session[SessionNames.SellerID];

            if (!this._accountAuthentication.IsAuthentic(sellerID, sessionSellerID))
                return false;

            return true;
        }
    }
}
