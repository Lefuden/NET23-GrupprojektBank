using NET23_GrupprojektBank.Currency;
using Newtonsoft.Json;
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
        [JsonProperty]
        protected int BankAccountNumber { get; set; }
        [JsonProperty]
        protected string BankAccountName { get; set; }
        [JsonProperty]
        protected BankAccountType BankAccountType { get; set; }
        [JsonProperty]
        protected CurrencyType CurrencyType { get; set; }
        [JsonProperty]
        protected decimal Balance { get; set; }
        [JsonProperty]
        public Guid OwnerUserId { get; set; }

        public abstract void MakeTransaction(Transaction transaction);
        public virtual decimal GetBalance() => Balance;
        public int GetAccountNumber() => BankAccountNumber;
        protected virtual decimal ConvertToCurrencyRate(CurrencyType currencyType, decimal sum)
        {
            var convertRate = CurrencyExchangeRate.GetCurrentCurrencyExchangeRate(currencyType);

            return (decimal)convertRate[currencyType] * sum;
        }
        public void Add(decimal sum)
        {
            if (sum > 0)
            {
                Balance += sum;
            }
        }
        public void Remove(decimal sum)
        {
            if (Balance >= sum)
            {
                Balance -= sum;
            }
        }

        private int BankAccountNumberGenerator()
        {
            var existingBankAccountNumbers = GetBankAccountNumbers();
            var nrGenerator = new Random();
            var accountNr = nrGenerator.Next(1000000000, 2000000001);    
            
            while (existingBankAccountNumbers.Contains(accountNr))
            {
                nrGenerator = new Random();
                accountNr = nrGenerator.Next(1000000000, 2000000001);    
            }
            return accountNr;
        }
    }
}
