using HotelReservation;

namespace MediaStore.Tests
{
    public class ProgramTest
    {
        public static void Main(string[] args)
        {
            RegisterEvent();

            var DataContainer = new DataContainer();

            DataContainer.Accounts = CsvSerializer.LoadFromCsv(@"C:\Users\Yunasawa\Documents\Projects\ISD.VN.20242-21\Samples\AccountData.csv");
            Function.AccountValidater.SignAccount(AccountSignType.SignUp, AccountVerificationType.PhoneNumber, "6750443676", "mwU6cwgS9zeN", "mwU6cwgS9zeN");
            Function.AccountValidater.SignAccount(AccountSignType.SignUp, AccountVerificationType.PhoneNumber, "6754483676", "mwU6cwgS9zeN", "mwU6cwgS9zeN");
            Function.AccountValidater.SignAccount(AccountSignType.SignIn, AccountVerificationType.PhoneNumber, "6750443676", "mwU6cwgS9zeN", "mwU6cwgS9zeN");
            Function.AccountValidater.SignAccount(AccountSignType.SignIn, AccountVerificationType.PhoneNumber, "6754483676", "mwU6cwgS9zeN", "mwU6cwgS9zeN");
        }

        private static void RegisterEvent()
        {
            Event.OnAccountVerificated += (result) =>
            {
                Console.WriteLine($"Account verification result: {result}");
            };
        }
    }
}
