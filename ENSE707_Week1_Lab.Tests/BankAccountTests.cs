using System;
using ENSE707_Week1_Lab;
using Microsoft.VisualStudio.TestTools.UnitTesting;
 
namespace ENSE707_Week1_Lab.Tests
{

    // ------ Activity 6: Create a Test Unit Project ------ //

    [TestClass]
    public class BankAccountTests
    {
        // ------ Deposit ------ //

        [TestMethod]
        public void Deposit_PositiveAmount_IncreasesBalance()
        {

            var account = new BankAccount("Christian", 100m);
            account.Deposit(50m);
            Assert.AreEqual(150m, account.Balance);

        }

        [TestMethod]
        public void Deposit_BoundaryValue_OneCent_IncreaseBalanceCorrectly()
        {

            // Smallest possible valid deposit (boundary just above zero)
            var account = new BankAccount("Christian", 100m);
            account.Deposit(0.01m);
            Assert.AreEqual(100.01m, account.Balance);

        }
        
        [TestMethod]
        public void Deposit_ZeroAmount_ShouldThrowOrBeRejected()
        {

            // Boundary case: is 0 a valid deposit? Business rule says no.
            var account = new BankAccount("Christian", 100m);
            Assert.ThrowsExactly<ArgumentException>(() => account.Deposit(0m)); 

        }

        [TestMethod]
        public void Deposit_NegativeAmount_ShouldThrowArgumentException()
        {

            var account = new BankAccount("Christian", 100m);
            Assert.ThrowsExactly<ArgumentException>(() => account.Deposit(-50m));

        }

        [TestMethod]
        public void Deposit_NegativeOneCent_BoundaryJustBelowZero_ShouldThrow()
        {
            
            // Boundary just below the zero threshold
            var account = new BankAccount("Christian", 100m);
            Assert.ThrowsExactly<ArgumentException>(() => account.Deposit(-0.01m));

        }

        // ------ Withdrawal ------ // 

        [TestMethod]
        public void Withdraw_AmountLessThanBalance_ReturnsTrueAndReducesBalance()
        {

            var account = new BankAccount("Christian", 100m);
            bool result = account.Withdraw(50);
            Assert.IsTrue(result);
            Assert.AreEqual(50m, account.Balance);

        }

        [TestMethod]
        public void Withdraw_AmountEqualToBalance_BoundaryValue_ReturnsTrueandZeroBalance()
        {

            var account = new BankAccount("Christian", 100m);
            bool result = account.Withdraw(100);
            Assert.IsTrue(result);
            Assert.AreEqual(0m, account.Balance);

        }

        [TestMethod]
        public void Withdraw_AmountOneCentMoreThanBalance_BoundaryValue_ReturnsFalseAndDoesNotChangeBalance()
        {

            var account = new BankAccount("Christian", 100m);
            bool result = account.Withdraw(100.01m);
            Assert.IsFalse(result);
            Assert.AreEqual(100m, account.Balance); // balance should be unchanged

        }

        [TestMethod]
        public void Withdraw_AmountGreaterThanBalance_ReturnsFalseAndPreventsOverdraft()
        {

            var account = new BankAccount("Christian", 100m);
            bool result = account.Withdraw(150m);
            Assert.IsFalse(result);
            Assert.AreEqual(100m, account.Balance);

        }
        
        [TestMethod]
        public void Withdraw_ZeroAmount_ShouldThrowOrBeRejected()
        {
            
            var account = new BankAccount("Christian", 100m);
            Assert.ThrowsExactly<ArgumentException>(() => account.Withdraw(0m));

        }

        [TestMethod]
        public void Withdraw_FromZeroBalance_ReturnsFalse()
        {   

            // Boundary: staring balance is exactly zero
            var account = new BankAccount("Christian", 0m);
            bool result = account.Withdraw(50m);
            Assert.IsFalse(result);
            Assert.AreEqual(0m, account.Balance);
            
        }

        // ------ Transaction Fee ------ // 

        [TestMethod]
        public void CalculateTransactionFee_TypicalAmount_ReturnsTwoPercent()
        {
            var account = new BankAccount("Christian", 100m);
            decimal fee = account.CalculateTransactionFee(100m);
            Assert.AreEqual(2.00m, fee);
        }

        [TestMethod]
        public void CalculateTransactionFee_ZeroAmount_BoundaryValue_ReturnsZeroFee()
        {
            var account = new BankAccount("Christian", 100m);
            decimal fee = account.CalculateTransactionFee(0m);
            Assert.AreEqual(0m, fee);
        }

        [TestMethod] 
        public void CalculateTransactionFee_NegativeAmount_ShouldThrowArgumentException()
        {
            
            var account = new BankAccount("Christian", 100m);
            Assert.ThrowsExactly<ArgumentException>(() => account.CalculateTransactionFee(-100m));

        }


        // ------ Activity 8: Add More Tests ------ //

        [TestMethod]
        public void Deposit_VeryLargeAmount_DoesNotOverflowOrCorruptBalance()
        {

            var account = new BankAccount("Christian", 0m);
            account.Deposit(decimal.MaxValue / 2);
            Assert.AreEqual(decimal.MaxValue / 2, account.Balance);

        }

        [TestMethod]
        public void CalculateTransactionFee_LargeAmount_DoesNotOverFlow()
        {
            var account = new BankAccount("Christian", 100m);
            decimal fee = account.CalculateTransactionFee(1000000m);
            Assert.AreEqual(20000m, fee);
        }

        [TestMethod]
        public void CalculateTransactionFee_SmallestUnitAmount_BoundaryValue_RoundsCorrectly()
        {
            
            // 1 cent * 2% = 0.0002, which is sub-cent - tests rounding behavior
            var account = new BankAccount("Christian", 100m);
            decimal fee = account.CalculateTransactionFee(0.01m);
            Assert.AreEqual(0.0002m, fee); // adjust expected value once rounding rule is defined

        }

        [TestMethod]
        public void Withdraw_NegativeAmount_ShouldThrowArgumentException()
        {

            // Without this guard, negative withdrawal would incorrectly increases balance
            var account = new BankAccount("Christian", 100m);
            Assert.ThrowsExactly<ArgumentException>(() => account.Withdraw(-10m));
            
        }

        // ------ Activity 9: Stakeholder and Quality Reflection Questions ------ //

        /*
        
        1. Who are the stakeholders for this small banking system?
           - The stakeholders for this small banking system are the following:
             Internal:
             * Community Bank 
             * Staff Member
             * Developer
             * Testers
            External:
             * Customer
        
        2. What does quality mean to each stakeholder?
           -
           * Community Bank - Software protects bank's core interests
           * Staff Member - The tool is usable, reliable, and predictable in daily operations
           * Developer - codes are high maintainable, easy to test, easy to understand.
           * Testers - the software is verifiable, means requirements can be traced through test cases
           * Customer - quality means that their money is safe, they can withdraw and deposit whenever they want, and accuracy.
        
        3. Which defects were detected through testing?
           - The defects got detected through testing and before improvement is that mostly of the test cases for negative amount, and zero amounts 
             failed and didn't pass as they are not handled properly.
        
        4. Which defects could have been prevented through QA activities?
           - The defects that could have been prevented through QA activies are the failed test for negative amounts and overdrafts which
             is highly forbidden based on the given requirements.
        
        5. How did copilot help?
           - It did help a lot on writing my test cases as it did on point on my test cases. Which is also based on the requirements given.

        6. What copilot suggestion did you reject or modify? Why?
           - Every copilot's suggestion is correct as I lead it to the given requirements which it gives an accurate results.
        
        7. What is the difference between QA and QC in this lab?
           - The difference between QA and QC in this lab is that QA wasn't implement properly based on the given requirements there are no 
             standardized calculations and practices on how to handles business rules such as withraw and deposit. While QC has occured when 
             test cases are being written which where bugs occur and help me identified problems and eventually fix it. 


        */
    }
}