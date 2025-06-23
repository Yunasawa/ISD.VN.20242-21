namespace YNL.JAMOS
{
    public enum AccountType : byte
    { 
        Customer, Manager
    }

    [System.Serializable]
    public class AccountAddress
    {
        public string Address;
        public string City;
    }

    [System.Serializable]
    public class Account
    {
        public UID ID;
        public string Name = string.Empty;
        public string Email = string.Empty;
        public string PhoneNumber = string.Empty;
        public string Password = string.Empty;
        public AccountType Type = AccountType.Customer;
        public AccountAddress Address = new();

        public Account()
        {
            Name = $"User{ID}";
        }
    }
}
