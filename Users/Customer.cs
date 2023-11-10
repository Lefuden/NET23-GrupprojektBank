using NET23_GrupprojektBank.BankAccounts;

namespace NET23_GrupprojektBank.Users
{
    internal class Customer : User
    {
        protected List<BankAccount> BankAccounts { get; set; }

        public Customer(string userName, string password) : base(userName, password)
        {
            UserType = Type.Customer;
            BankAccounts = new List<BankAccount>();
        }

        public void ViewBankAccount(List<BankAccount> bankAccount)
        {
            //call method from BankAccount?
            if (bankAccount.Count != 0)
            {
                foreach (var account in bankAccount)
                {
                    //amazing interface
                    Console.WriteLine(account);
                    //back to menu
                }
            }
            else
            {
                //amazing interface
                Console.WriteLine($"no accounts found");
                //add new? y/n -> call method CreateBankAccount() or back to menu
            }
        }

        public void CreateBankAccount(string bankAccountName, Enum bankAccountType, decimal balance, Enum currencyType)
        {

            //BankAccount.cs
            //string BankAccountNumber, BankAccountName
            //Enum BankAccountType, CurrencyType
        }
        
        public void CreateTransaction() //: Transaction
        {
            //Transaction.cs
            //User SourceUser, User DestinationUser, BankAccount SourceBankAccount
            //BankAccount DestinationBankAccoun, decimal Sum, Enum SourceCurrencyType
            //Enum DestinationCurrencyType, DateTime DateAndTime
        }

        public void MakeLoan(User user, decimal loanAmount)
        {
            //BankAccount.GetBalance();
        }
    }
}
