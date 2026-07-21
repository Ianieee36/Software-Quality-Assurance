namespace ENSE707_Week1_Lab
{

    // ------ Activity 7: Improve Code ------ //
    public class BankAccount
    {
        public string AccountHolder { get; }
        public decimal Balance { get; private set; }

        public BankAccount(string accountHolder, decimal openingBalance)
        {   
            if(string.IsNullOrEmpty(accountHolder))
                throw new ArgumentException("AccountHolder is required.");

            if(openingBalance < 0)
                throw new ArgumentException("Openingbalance cannot be negative");

            AccountHolder = accountHolder;
            Balance = openingBalance;
        }

        public void Deposit(decimal amount)
        {   
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit must be greater than zero");
            }

            Balance += amount;
               
        }

        public bool Withdraw(decimal amount)
        {

            if(amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero");

            if(amount > Balance)
                return false;

            Balance -= amount;    
            return true;
        }

        public decimal CalculateTransactionFee(decimal amount)
        {
            
            if (amount < 0)
            {
                throw new ArgumentException("Amount must be greater than zero");
            }

            if(amount == 0)
            {
                return 0;
            } 

            return amount * 0.02m;

        }
    }
}

