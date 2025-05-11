using HotelReservation;
using System;
using MediaStore;

namespace MediaStore.Tests
{
    public class ProgramTest
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting tests from ProgramTest...");
            Console.WriteLine("-------------------------------------");

            Console.WriteLine("\n--- Running Account Tests ---");
            RegisterEvent();

            // This separation should prevent conflicts as long as AccountValidater doesn't modify static MediaContainer.
            // var accountDataContainer = new DataContainer(); 

            try
            {
                // Ensure the CSV path is correct for your environment if you uncomment and run this.
                DataContainer.Accounts = CsvSerializer.LoadFromCsv(@"D:\ISD.VN.20242-21\Samples\AccountData.csv");
                // Console.WriteLine("Account loading from CSV is currently commented out in ProgramTest.cs. Update path if needed.");

                Function.AccountValidater.SignAccount(AccountSignType.SignUp, AccountVerificationType.PhoneNumber, "6750443676", "mwU6cwgS9zeN", "mwU6cwgS9zeN");
                Function.AccountValidater.SignAccount(AccountSignType.SignUp, AccountVerificationType.PhoneNumber, "6754483676", "mwU6cwgS9zeN", "mwU6cwgS9zeN");
                Function.AccountValidater.SignAccount(AccountSignType.SignIn, AccountVerificationType.PhoneNumber, "6750443676", "mwU6cwgS9zeN", "mwU6cwgS9zeN");
                Function.AccountValidater.SignAccount(AccountSignType.SignIn, AccountVerificationType.PhoneNumber, "6754483676", "mwU6cwgS9zeN", "mwU6cwgS9zeN");
                Console.WriteLine("Account SignAccount calls completed (ensure accounts are loaded for full validation).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Account tests: {ex.Message}");
            }
            Console.WriteLine("--- Account Tests Finished ---");
            

            Console.WriteLine("\n--- Running ProductModifier Tests ---");
            ProductModifierTest productModifierTests = new ProductModifierTest();
            productModifierTests.RunTests();
            Console.WriteLine("--- ProductModifier Tests Finished ---");

            Console.WriteLine("\n-------------------------------------");
            Console.WriteLine("All tests from ProgramTest completed. Press any key to exit...");
            Console.ReadKey();
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
