using NET23_GrupprojektBank.Currency;

namespace NET23_GrupprojektBank.BankAccounts
{
    internal class Checking : BankAccount
    {
        public Checking(int bankAccountNumber, string bankAccountName, CurrencyType currencyType, decimal balance = 0)
        {
            BankAccountNumber = bankAccountNumber;
            BankAccountName = bankAccountName;
            BankAccountType = BankAccountType.Checking;
            CurrencyType = currencyType;
            Balance = balance;
            LoanAmount = 0;
            LoanInterestRate = 0;
        }
    }
}
