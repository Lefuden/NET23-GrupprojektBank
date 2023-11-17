using NET23_GrupprojektBank.Currency;
using System.Transactions;

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
        }
        public Checking(int bankAccountNumber, string bankAccountName, BankAccountType bankAccountType, CurrencyType currencyType, decimal balance, Guid ownerUserId)
        {
            BankAccountNumber = bankAccountNumber;
            BankAccountName = bankAccountName;
            BankAccountType = bankAccountType;
            CurrencyType = currencyType;
            Balance = balance;
            OwnerUserId = ownerUserId;
        }
        public override void MakeTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
