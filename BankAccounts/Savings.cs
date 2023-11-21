using NET23_GrupprojektBank.Currency;
using System.Transactions;

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
        }

        public override (string Type, string Name, string Number, string Balance, string Currency, string Interest) GetAccountInformation()
        {
            return (BankAccountType.ToString(), BankAccountName, BankAccountNumber.ToString(), $"{Balance:.00}", CurrencyType.ToString(), $"{Interest:p}".ToString());
        }
        public override void MakeTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
