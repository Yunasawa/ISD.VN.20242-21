using System.Collections.Generic;

namespace YNL.Checkotel
{
    public class DataContainer
    {
        public UID AccountID = new();
        public static List<Account> Accounts = new();

        public static HotelContainer Hotel = new();
        public static VoucherContainer Voucher = new();
    }
}
