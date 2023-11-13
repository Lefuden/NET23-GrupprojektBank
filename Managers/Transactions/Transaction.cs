using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Users;

namespace NET23_GrupprojektBank.Managers.Transactions
{
    internal class Transaction
    {
        public User SourceUser { get; set; }
        public User DestinationUser { get; set; }
        public BankAccount SourceBankAccount { get; set; }
        public BankAccount DestinationBankAccount { get; set; }
        public CurrencyType SourceCurrencyType { get; set; }
        public CurrencyType DestinationCurrencyType { get; set; }
        public DateTime DateAndTime { get; set; }
        public decimal Sum { get; set; }
        public Transaction(User sourceUser, User destinationUser, BankAccount sourceBankAccount, BankAccount destinationBankAccount, CurrencyType sourceCurrencyType, CurrencyType destinationCurrencyType, DateTime dateAndTime, decimal sum)
        {
            SourceUser = sourceUser;
            DestinationUser = destinationUser;
            SourceBankAccount = sourceBankAccount;
            DestinationBankAccount = destinationBankAccount;
            SourceCurrencyType = sourceCurrencyType;
            DestinationCurrencyType = destinationCurrencyType;
            DateAndTime = dateAndTime;
            Sum = sum;
        }
        public Transaction(User sourceUser, User destinationUser, BankAccount sourceBankAccount, BankAccount destinationBankAccount, CurrencyType sourceCurrencyType, CurrencyType destinationCurrencyType, decimal sum)
        {
            SourceUser = sourceUser;
            DestinationUser = destinationUser;
            SourceBankAccount = sourceBankAccount;
            DestinationBankAccount = destinationBankAccount;
            SourceCurrencyType = sourceCurrencyType;
            DestinationCurrencyType = destinationCurrencyType;
            DateAndTime = DateTime.UtcNow;
            Sum = sum;
        }
    }
}
