using NET23_GrupprojektBank.Currency;

namespace NET23_GrupprojektBank.BankAccounts
{
    internal class Savings : BankAccount
    {
        private double Interest { get; set; }
        public Savings(int bankAccountNumber, string bankAccountName, CurrencyType currencyType, decimal balance = 0, double interest = 0)
        {
            BankAccountNumber = bankAccountNumber;
            BankAccountName = bankAccountName;
            BankAccountType = BankAccountType.Savings;
            CurrencyType = currencyType;
            Balance = balance;
            Interest = interest;
            LoanAmount = 0;
            LoanInterestRate = 0;
        }

        public override (string Type, string Name, string Number, string Balance, string Currency, string Interest) GetAccountInformation()
        {
            return (BankAccountType.ToString(), BankAccountName, BankAccountNumber.ToString(), $"{Balance:.00}", CurrencyType.ToString(), $"{Interest:p}".ToString());
        }
    }
}
