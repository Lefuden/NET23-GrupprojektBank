using NET23_GrupprojektBank.Currency;
using System.Transactions;

namespace NET23_GrupprojektBank.BankAccounts
{
    internal class Checking : BankAccount
    {
        //protected string BankAccountNumber { get; set; }
        //protected string BankAccountName { get; set; }
        //protected BankAccountType BankAccountType { get; set; }
        //protected CurrencyType CurrencyType { get; set; }
        //protected decimal Balance { get; set; }
        public Checking(string bankAccountNumber, string bankAccountName, CurrencyType currencyType, decimal balance = 0)
        {
            BankAccountNumber = bankAccountNumber;
            BankAccountName = bankAccountName;
            BankAccountType = BankAccountType.Checking;
            CurrencyType = currencyType;
            Balance = balance;
        }
        public override void MakeTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
