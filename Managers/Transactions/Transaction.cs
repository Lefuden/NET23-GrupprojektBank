using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Users;

namespace NET23_GrupprojektBank.Managers.Transactions
{
    public enum TransactionType
    {
        Withdrawal,
        Loan,
        Transfer,
        Deposit
    }
    internal class Transaction
    {
        public User SourceUser { get; set; }
        public User DestinationUser { get; set; }
        public BankAccount SourceBankAccount { get; set; }
        public BankAccount DestinationBankAccount { get; set; }
        public CurrencyType SourceCurrencyType { get; set; }
        public CurrencyType DestinationCurrencyType { get; set; }
        public double InterestRate { get; set; }
        public DateTime DateAndTime { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Sum { get; set; }
        public Transaction(User sourceUser, User destinationUser, BankAccount sourceBankAccount, BankAccount destinationBankAccount, CurrencyType sourceCurrencyType, CurrencyType destinationCurrencyType, DateTime dateAndTime, TransactionType transactionType, decimal sum, double interestRate = 0)
        {
            SourceUser = sourceUser;
            DestinationUser = destinationUser;
            SourceBankAccount = sourceBankAccount;
            DestinationBankAccount = destinationBankAccount;
            SourceCurrencyType = sourceCurrencyType;
            DestinationCurrencyType = destinationCurrencyType;
            DateAndTime = dateAndTime;
            TransactionType = transactionType;
            Sum = sum;
            InterestRate = interestRate;
        }
        public Transaction(User sourceAndDestinationUser, BankAccount sourceBankAccount, BankAccount destinationBankAccount, CurrencyType sourceCurrencyType, CurrencyType destinationCurrencyType, TransactionType transactionType, decimal sum, double interestRate = 0)
        {
            SourceUser = sourceAndDestinationUser;
            DestinationUser = sourceAndDestinationUser;
            SourceBankAccount = sourceBankAccount;
            DestinationBankAccount = destinationBankAccount;
            SourceCurrencyType = sourceCurrencyType;
            DestinationCurrencyType = destinationCurrencyType;
            DateAndTime = DateTime.UtcNow;
            TransactionType = transactionType;
            Sum = sum;
            InterestRate = interestRate;
        }
        public Transaction(User sourceAndDestinationUser, BankAccount sourceAndDestinationAccount, CurrencyType sourceAndDestinationCurrencyType, TransactionType transactionType, decimal sum, double interestRate = 0)
        {
            SourceUser = sourceAndDestinationUser;
            DestinationUser = sourceAndDestinationUser;
            SourceBankAccount = sourceAndDestinationAccount;
            DestinationBankAccount = sourceAndDestinationAccount;
            SourceCurrencyType = sourceAndDestinationCurrencyType;
            DestinationCurrencyType = sourceAndDestinationCurrencyType;
            DateAndTime = DateTime.UtcNow;
            TransactionType = transactionType;
            Sum = sum;
            InterestRate = interestRate;
        }
    }
}
