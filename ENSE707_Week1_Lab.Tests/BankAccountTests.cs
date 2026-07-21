using System;
using ENSE707_Week1_Lab;
using Microsoft.VisualStudio.TestTools.UnitTesting;
 
namespace ENSE707_Week1_Lab.Tests
{
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
            decimal fee = account.CalculateTransactionFee(decimal.MaxValue);
            Assert.AreEqual(decimal.MaxValue / 2, fee);
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
    }
}