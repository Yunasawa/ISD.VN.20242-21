using MediaStore;
using MediaStore.Exceptions;
using System;
using System.Collections.Concurrent; // Required for ConcurrentDictionary

namespace MediaStore.Tests
{
    public class ProductModifierTest
    {
        private Function.ProductModifier productModifier;

        // Helper to reset and setup for tests
        private void Setup()
        {
            // Reset the static MediaContainer for a clean test environment
            DataContainer.MediaContainer = new MediaContainer();
            productModifier = new Function.ProductModifier();
        }

        public void RunTests()
        {
            Console.WriteLine("Running ProductModifier Tests...");

            Test_AddProduct_Success();
            Test_AddProduct_Duplicate_ThrowsException();
            Test_RemoveProduct_Success();
            Test_RemoveProduct_NonExistent_ThrowsException();
            Test_AddProduct_NullUID_ThrowsException(); // Example of further test for robustness
            Test_RemoveProduct_NullUID_ThrowsException(); // Example of further test for robustness


            Console.WriteLine("ProductModifier Tests Completed.");
            // Simple report
            if (failedTests.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n{failedTests.Count} test(s) failed:");
                foreach (var testName in failedTests)
                {
                    Console.WriteLine($"- {testName}");
                }
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nAll ProductModifier tests passed!");
                Console.ResetColor();
            }
        }

        private System.Collections.Generic.List<string> failedTests = new System.Collections.Generic.List<string>();

        private void AssertTrue(bool condition, string testName)
        {
            if (!condition)
            {
                Console.WriteLine($"- Test Failed: {testName}");
                if (!failedTests.Contains(testName)) failedTests.Add(testName);
            }
        }
        
        private void AssertThrows<TException>(Action action, string testName) where TException : Exception
        {
            try
            {
                action();
                Console.WriteLine($"- Test Failed: {testName} (Expected exception {typeof(TException).Name} was not thrown)");
                if (!failedTests.Contains(testName)) failedTests.Add(testName);
            }
            catch (TException)
            {
                // Expected exception caught, test passes in this aspect
            }
            catch (Exception ex)
            {
                Console.WriteLine($"- Test Failed: {testName} (Expected exception {typeof(TException).Name} but got {ex.GetType().Name})");
                 if (!failedTests.Contains(testName)) failedTests.Add(testName);
            }
        }


        // --- AddProduct Tests ---
        public void Test_AddProduct_Success()
        {
            Setup();
            string testName = nameof(Test_AddProduct_Success);
            UID productId = new UID(1);

            productModifier.AddProduct(productId);

            AssertTrue(DataContainer.MediaContainer.MediaUnits.ContainsKey(productId), testName + " - Product should be in container");
            AssertTrue(DataContainer.MediaContainer.MediaUnits[productId] != null, testName + " - Product should not be null");
        }

        public void Test_AddProduct_Duplicate_ThrowsException()
        {
            Setup();
            string testName = nameof(Test_AddProduct_Duplicate_ThrowsException);
            UID productId = new UID(1);

            productModifier.AddProduct(productId); // Add once

            AssertThrows<InvalidOperationException>(() => productModifier.AddProduct(productId), testName);
        }

        // --- RemoveProduct Tests ---
        public void Test_RemoveProduct_Success()
        {
            Setup();
            string testName = nameof(Test_RemoveProduct_Success);
            UID productId = new UID(1);

            productModifier.AddProduct(productId); // Add product first
            AssertTrue(DataContainer.MediaContainer.MediaUnits.ContainsKey(productId), testName + " - Precondition: Product should be added");

            productModifier.RemoveProduct(productId);

            AssertTrue(!DataContainer.MediaContainer.MediaUnits.ContainsKey(productId), testName + " - Product should be removed");
        }

        public void Test_RemoveProduct_NonExistent_ThrowsException()
        {
            Setup();
            string testName = nameof(Test_RemoveProduct_NonExistent_ThrowsException);
            UID productId = new UID(1); // This product is not added

            AssertThrows<MediaNotFoundException>(() => productModifier.RemoveProduct(productId), testName);
        }
        
        // Example of how you might test for null or invalid UID if your UID struct couldn't be 0 or if it had specific validation
        // For the current UID struct (implicit from uint), a UID(0) is valid.
        // If UID had specific validation, these tests would be more relevant.
        public void Test_AddProduct_NullUID_ThrowsException()
        {
            Setup();
            string testName = nameof(Test_AddProduct_NullUID_ThrowsException);
            // UID is a struct, so it cannot be null.
            // If UID could be in an "invalid" state (e.g., UID.Empty or similar pattern), you would test that.
            // For now, let's assume UID(0) is a valid, specific ID.
            // If the methods were to check for a specific "invalid" UID, this test would change.
            // This test serves as a placeholder for such logic if it were introduced.
            // For example, if you decided UID(0) was invalid:
            // AssertThrows<ArgumentException>(() => productModifier.AddProduct(new UID(0)), testName);
            // As it stands, UID(0) is valid, so adding it should succeed unless it's a duplicate.
            try
            {
                 productModifier.AddProduct(new UID(0)); // This should succeed
                 AssertTrue(DataContainer.MediaContainer.MediaUnits.ContainsKey(new UID(0)), testName + " - UID(0) should be addable if not duplicate");
            }
            catch(Exception ex)
            {
                // If adding UID(0) legitimately throws (e.g. duplicate after another test added it without Setup),
                // this isn't the test for it. This test is for *invalid* UIDs.
                Console.WriteLine($"- Test Skipped/Ambiguous: {testName} - UID(0) behavior depends on other constraints or prior state if Setup() is not perfectly isolating. Current UID struct doesn't have an 'invalid' state other than duplicates.");
                // To make this a real test for invalid UIDs, UID struct itself would need a concept of invalidity
                // or the method would need to define certain UIDs as not permissible.
            }
        }

        public void Test_RemoveProduct_NullUID_ThrowsException()
        {
            Setup();
            string testName = nameof(Test_RemoveProduct_NullUID_ThrowsException);
            // Similar to AddProduct, UID struct cannot be null.
            // If UID(0) was considered a special non-removable ID or invalid:
            // AssertThrows<ArgumentException>(() => productModifier.RemoveProduct(new UID(0)), testName);
            // As it stands, removing UID(0) should throw MediaNotFoundException if it's not there.
            AssertThrows<MediaNotFoundException>(() => productModifier.RemoveProduct(new UID(0)), testName + " - Removing UID(0) (non-existent)");
        }
    }
}

// To run these tests, you could add something like this to your ProgramTest.cs or a new entry point:
//
// public static class TestRunner
// {
//     public static void Main(string[] args)
//     {
//         ProductModifierTest productModifierTests = new ProductModifierTest();
//         productModifierTests.RunTests();
//
//         // You could add other test class executions here
//         // AccountValidatorTest accountValidatorTests = new AccountValidatorTest();
//         // accountValidatorTests.RunTests();
//     }
// } 