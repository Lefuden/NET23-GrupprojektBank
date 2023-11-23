using NET23_GrupprojektBank.Currency;

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
        protected int BankAccountNumber { get; set; }
        protected string BankAccountName { get; set; }
        protected BankAccountType BankAccountType { get; set; }
        protected CurrencyType CurrencyType { get; set; }
        protected decimal Balance { get; set; }
        protected decimal LoanAmount { get; set; }
        protected double LoanInterestRate { get; set; }

        public decimal GetBalance() => Balance;
        public decimal GetLoanAmount() => LoanAmount;
        public double GetLoanInterestRate() => LoanInterestRate;
        public int GetAccountNumber() => BankAccountNumber;
        public decimal ConvertToCurrencyRate(CurrencyType currencyType, decimal sum)
        {
            var convertRateDictionary = CurrencyExchangeRate.GetCurrentCurrencyExchangeRate(CurrencyType);
            var convertRateValue = (decimal)convertRateDictionary[currencyType];
            var convertSum = convertRateValue * sum;
            return convertSum;
        }
        public void AddLoanAndInterest(decimal loanAmount, double loanInterestRate)
        {
            if (loanAmount >= 0 && loanInterestRate >= 0)
            {
                LoanAmount = loanAmount;
                LoanInterestRate = loanInterestRate;
            }
        }
        public void AddBalance(decimal sum)
        {
            if (sum > 0)
            {
                Balance += sum;
            }
        }
        public void RemoveBalance(decimal sum)
        {
            if (Balance >= sum)
            {
                Balance -= sum;
            }
        }
        public virtual (string BankAccountType, string BankAccountName, string BankAccountNumber, string Balance, string CurrencyType, string Interest) GetAccountInformation()
        {
            //.PadLeft(BankAccountType.ToString().Length)
            return (BankAccountType.ToString(), BankAccountName, BankAccountNumber.ToString(), $"{Balance:0.00}", CurrencyType.ToString(), "");
        }
        public static int BankAccountNumberGenerator(List<int> existingBankAccountNumbers)
        {
            var nrGenerator = new Random();
            var accountNr = nrGenerator.Next(1000000000, 2000000001);

            if (existingBankAccountNumbers is null || existingBankAccountNumbers.Count == 0)
            {
                return accountNr;
            }

            while (existingBankAccountNumbers.Contains(accountNr))
            {
                nrGenerator = new Random();
                accountNr = nrGenerator.Next(1000000000, 2000000001);
            }
            return accountNr;
        }
    }
}
