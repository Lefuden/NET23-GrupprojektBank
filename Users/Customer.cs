using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Users.UserInformation;

namespace NET23_GrupprojektBank.Users
{
    internal class Customer : User
    {
        protected List<BankAccount> BankAccounts { get; set; }

        public Customer(string userName, string password, PersonInformation person) : base(userName, password, person)
        {
            UserType = UserType.Customer;
            BankAccounts = new List<BankAccount>();
        }

        public void ViewBankAccount()
        {
            if (BankAccounts.Count != 0)
            {
                foreach (var account in BankAccounts)
                {
                    //amazing interface
                    Console.WriteLine(account);
                    Console.ReadKey();
                    //back to menu
                }
            }
            else
            {
                //amazing interface
                Console.WriteLine($"no accounts found\nwould you like to open a new account?\n1. yes\n2. no");
                var input = int.TryParse(Console.ReadLine(), out int choice);
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("enter account name, account type: checking or savings (1,2), balance, currencytype (0-50)");
                        Console.ReadLine();
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }
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
