using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Domain;
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
        public AvatarOfProductsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public HttpResponseMessage Get(int productID)
        {
            var avatarOfProduct = this._unitOfWork.AvatarOfProducts.Get(productID);

            if (avatarOfProduct == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            return GetResponseMessage(avatarOfProduct);
        }

        [HttpGet]
        public byte[] GetAvatarOfProductAsByte(int productID)
        {
            var avatarOfProduct = this._unitOfWork.AvatarOfProducts.Get(productID);

            if (avatarOfProduct == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

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
    }
}
