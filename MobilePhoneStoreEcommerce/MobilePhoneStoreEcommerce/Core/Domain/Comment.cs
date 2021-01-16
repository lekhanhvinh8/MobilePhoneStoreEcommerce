using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Core.Domain
{
    public class Comment
    {
        public int ID { get; set; }
        public DateTime CommentTime { get; set; }
        public string Content { get; set; }

        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }

    }
}