namespace MobilePhoneStoreEcommerce.Core.Domain
{
    public partial class Account
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public int RoleID { get; set; }

        public virtual Role Role { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
