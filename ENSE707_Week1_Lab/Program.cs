using ENSE707_Week1_Lab;

BankAccount account = new BankAccount("Student User", 100);

account.Deposit(100);

Console.WriteLine($"Account Holder: {account.AccountHolder}");
Console.WriteLine($"Current Balance: {account.Balance}");
Console.WriteLine($"Fee on $100 transaction: {account.CalculateTransactionFee(100)}");

/* - Activity 3: Identify Quality Issues

1. What happens if the opening balance is negative?

* I test it by using a negative opening balance of -100 and try to withdraw a 100
  and what happens is that it only adds up so the current balance becomes -200.

2. What happens if a deposit amount is negative?

* I test it by using a deposit amount of -100 and with an opening balance of 100
  and what happens is that it only subtracts from the opening balance so the current
  balance becomes 0.

3. What happens if a withdrawal amount is greater than the balance?

* I test using a 101 withdrawal amount and an opening balance of 100 and it only
  goes overdraft so i'm currently have a negative balance in my account. 

4. Is the transaction fee calculation clearly documented?

* The transaction fee calculation is not clearly documented, because there is no
  specific business rule on when and where will the transaction fee will be calculated
  it's just basically amount * 0.02m

5. Is the class easy to test

* the class is easy to test because the given code is very straight forward with less
  complexities but it comes with a lot of quality issues. 

6. What functional requirements are missing?
 
* The functional requirements missing are the creation of customer account there is only 
  a hardcoded accountholder which is partially missing. Most of the given requirements are
  partially met such as deposit and withdrawals and the calculation of transaction fee. 
  The other one that is missing is the prevention of overdrafts as I test the given program 
  it does not handle and overdrafts or prevent it.

7. What non-functional quality attributes are relevant

* Based on the outcomes of the given code is the correctness which is critical in banking system
  as this is of people's money it is important to have a correct calculations. Security also important
  details or information are in public which makes it easier for some intruders to breach some sensitive
  informations of the accountholder. Usability is also one of the things that can determine if the quality
  of the software is poor. 
   
*/

/* - Activity 4: QA vs QC

* The differences between QA and QC is that QA is build quality software based on standards, practices, and procedures
  that reduces the occurrence of defects while QC is the examining the output of a software where codes, the build itself
  the running application to find defects that already exists. Though this two software development practices shares the 
  same goals which is to deliver a product that meets quality standards and achieve customer/business requirements.
  Both are subject to continuous improvement, along the way from deployment to during runtime there are triggers that
  will make developers change such as in coding practices, standards, and etc.

* Activities

* Writing coding standards for money calculations - Quality Assurance
  - It ensures that money calculations is standardize to prevent
    any miscalculations.

* Running unit tests for withdrawal behavior - Quality Control
  - The product is being tested after the development activities.

* Reviewing requirements for ambiguity - Quality Assurance
  - Making sure that requirements are well understood and prevents
    any confusion that can lead to defects.

* Testing negative deposit input - Quality Control
  - It test the deposit method for any defects. 

* Analysing repeated transaction defects - Quality Assurance
  - There are repeated transaction which comes down to quality assurance.
    Meaning that there are no standard validation, calculations and etc. 
    that handles it.

* Reporting a failed test case - Quality Control
  - Reporting a failed test case which means that the product detects a 
    defect.

* Creating a checklist for financial validation rules - Quality Assurance
  - Creating a checklist for financial validation rules helps ensures the
    product of any errors and prevents any financial loss for the customer
    and the business.

* Retesting after fixing withdrawal logic - Quality Control
  - Same with the few scenarios it detects a defect and fixed it and subjects 
    it to retesting.
 
* Discussion Questions

* Why is writing tests not enough to guarantee quality?
  - Writing tests only does not guarantee quality because writing tests does not 
    cover different defects that might occur in a software such as performance, usability,
    maintainbility and readability of the code, etc. Writing test reduces any risk of defects
    during production but not totally eliminating it.
*/

