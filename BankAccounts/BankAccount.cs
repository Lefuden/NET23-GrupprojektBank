using NET23_GrupprojektBank.Currency;
using System.Transactions;

namespace NET23_GrupprojektBank.BankAccounts
{
    public enum BankAccountType
    {
        Checking,
        Savings,
        Undeclared
    }
    internal abstract class BankAccount
    {
        protected string BankAccountNumber { get; set; }
        protected string BankAccountName { get; set; }
        protected BankAccountType BankAccountType { get; set; }
        protected CurrencyType CurrencyType { get; set; }
        protected decimal Balance { get; set; }

        public abstract void MakeTransaction(Transaction transaction);
        public virtual decimal GetBalance()
        {
            return Balance;
        }
        protected virtual decimal ConvertToCurrencyRate(CurrencyType currencyType, decimal sum)
        {
            var convertRate = CurrencyExchangeRate.GetCurrentCurrencyExchangeRate();

            return (decimal)convertRate[currencyType] * sum;
        }

    }
}
