
namespace MobilePhoneStoreEcommerce.Core.Domain
{
    using System;
    using System.Collections.Generic;

    public partial class Role
    {
        public Role()
        {
            this.Accounts = new List<Account>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }

        public virtual List<Account> Accounts { get; set; }
    }
}
