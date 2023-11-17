using NET23_GrupprojektBank.Currency;
using Newtonsoft.Json;
using System.Transactions;

namespace NET23_GrupprojektBank.BankAccounts
{
    internal class Savings : BankAccount
    {
        [JsonProperty]
        private double Interest { get; set; }
        public Savings(int bankAccountNumber, string bankAccountName, CurrencyType currencyType, decimal balance = 0, double interest = 0)
        {
            BankAccountNumber = bankAccountNumber;
            BankAccountName = bankAccountName;
            BankAccountType = BankAccountType.Checking;
            CurrencyType = currencyType;
            Balance = balance;
            Interest = interest;
        }

        public Savings(int bankAccountNumber, string bankAccountName, BankAccountType bankAccountType, CurrencyType currencyType, decimal balance, Guid ownerUserId, double interest)
        {
            BankAccountNumber = bankAccountNumber;
            BankAccountName = bankAccountName;
            BankAccountType = bankAccountType;
            CurrencyType = currencyType;
            Balance = balance;
            OwnerUserId = ownerUserId;
            Interest = interest;
        }
        public override void MakeTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
