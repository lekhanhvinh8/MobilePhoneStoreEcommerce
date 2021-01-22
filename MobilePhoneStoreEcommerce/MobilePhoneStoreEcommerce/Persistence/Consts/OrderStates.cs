namespace MobilePhoneStoreEcommerce.Persistence.Consts
{
    public static class OrderStates
    {
        public static int Pending = 0;

        public static int Canceled = -1;

        public static int Confirmed = 1;

        public static int Paid = 2;

        public static int Success = 10;
    }
}