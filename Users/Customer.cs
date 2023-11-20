using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Managers.Logs;
using NET23_GrupprojektBank.Managers.Transactions;
using NET23_GrupprojektBank.Users.UserInformation;
using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;
using Newtonsoft.Json;
using Spectre.Console;
using System.Diagnostics.Metrics;
using System.IO;

namespace NET23_GrupprojektBank.Users
{
    internal class Customer : User
    {
        [JsonProperty]
        protected List<BankAccount> BankAccounts { get; set; }

        public Customer(string userName, string password, PersonInformation person) : base(userName, password, person)
        {
            UserType = UserType.Customer;
            BankAccounts = new List<BankAccount>();
        }

        [JsonConstructor]      

        public Customer(string username, Guid userId, string salt, string hashedPassword, PersonInformation personInformation, UserType userType, List<Log> logs, List<BankAccount> bankAccounts) : base(username, userId, salt, hashedPassword, personInformation, userType, logs)
        {
            if (BankAccounts is null)
            {
                BankAccounts = new();
            }
            if (bankAccounts is not null)
            {
                BankAccounts = bankAccounts;
            }
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
                switch (Admin.AskUserYesOrNo("no account found. would you like to open a new account?"))
                {
                    case true:
                        CreateBankAccount(BankAccounts);
                        break;
                    case false:
                        break;
                }
            }
        }

        public EventStatus CreateCheckingAccount()
        {
            //logic to create account goes here
            AddLog(EventStatus.CheckingCreationSuccess); //or failed
            throw new NotImplementedException();
        }

        public BankAccount CreateBankAccount(List<BankAccount> existingBankAccountNumbers)
        {
            while (true)
            {
                Console.WriteLine("Enter bank account details.");
                var bankAccountType = BankAccountType.Undeclared;
                switch (ChooseAccountType())
                {
                    case true:
                        Console.Clear();
                        bankAccountType = BankAccountType.Checking;
                        break;
                    case false:
                        Console.Clear();
                        bankAccountType = BankAccountType.Savings;
                        break;
                }

                var bankAccountName = AnsiConsole.Ask<string>("[green]Account name[/]:");

                var currencyType = ChooseCurrencyType();
                
                var bankAccountNr = BankAccountNumberGenerator();
                
                Console.WriteLine($"Account type: {bankAccountType}Account number: {bankAccountNr}\nAccount name: {bankAccountName}\nAccount currency type: {currencyType}\n\n");
                switch (Admin.AskUserYesOrNo("is this information correct?"))
                {
                    case true:
                        Console.Clear();
                        switch (bankAccountType)
                        {
                            case BankAccountType.Checking:
                                AddLog(EventStatus.CheckingCreationSuccess);
                                return new Checking(1234, bankAccountName, currencyType, 0.0M);
                            case BankAccountType.Savings:
                                AddLog(EventStatus.SavingsCreationSuccess);
                                return new Savings(1234, bankAccountName, currencyType, 0.0M, interest: 0.0);
                            //case BankAccountType.Undeclared:
                            //    AddLog(EventStatus.AccountCreationFailed);
                            //    return null;
                        }
                        break;
                    case false:
                        Console.Clear();
                        break;
                }
            }
        }

        public EventStatus CreateSavingsAccount()
        {
            //logic to create account goes here
            AddLog(EventStatus.SavingsCreationFailed); //or success
            throw new NotImplementedException();
        }

        private Transaction CreateTransaction() //: Transaction input parameters for various transactions/loans/withdrawals
        {
            //take input from various Make-methods to then create the transaction of corresponding type
            //return new Transaction();
            AddLog(EventStatus.TransactionCreated);
            throw new NotImplementedException();
        }
        
        public Transaction MakeLoan()
        {
            //BankAccount.GetBalance();
            //logic to create a loan
            //send to createtransaction for completion
            AddLog(EventStatus.LoanCreated);
            while (true)
            {
                Console.WriteLine("make loan.");
                CurrencyType currencyType = ChooseCurrencyType();
                Console.WriteLine($"{currencyType}\n\n");
                switch (Admin.AskUserYesOrNo("is this information correct?"))
                {
                    case true:
                        Console.Clear();
                        break;
                    case false:
                        Console.Clear();
                        break;
                }
            }
            return CreateTransaction(); //return transaction. return null if cancelled.
        }

        public Transaction MakeWithdrawal()
        {
            Console.WriteLine("Withdrawal");
            
            var accountChoice = GetBankAccountDetails();
            foreach (var account in BankAccounts)
            {
                account.GetBalance();
            }

            //logic to create a withdrawal
            //send to createtransaction for completion
            AddLog(EventStatus.WithdrawalCreated);
            return CreateTransaction(); //return transaction. return null if cancelled.
        }

        public Transaction MakeDeposit()
        {
            //BankAccount.GetBalance();
            //logic to create a deposit
            //send to createtransaction for completion
            AddLog(EventStatus.DepositCreated);
            return CreateTransaction(); //return transaction. return null if cancelled.
        }

        public Transaction MakeTransfer()
        {
            //BankAccount.GetBalance();
            //logic to create a transfer
            //send to createtransaction for completion
            AddLog(EventStatus.TransferCreated);

            return CreateTransaction(); //return transaction. return null if cancelled.
        }

        public bool ChooseAccountType()
        {
            var stringChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"[purple]Select bank account type[/]")
                .PageSize(3)
                .AddChoices(new[]
                    {
                        "Checking account",
                        "Savings account"
                    }
                ));
            return stringChoice == "Checking account";
        }
        
        public string GetBankAccountDetails()
        {
            var accounts = new SelectionPrompt<string>()
                .Title("Choose account")
                .PageSize(5);

            foreach (var account in BankAccounts)
            {
                switch (account)
                {
                    case Savings savingsAccount:
                    {
                        var accountDetails = savingsAccount.GetAccountInformation();
                        accounts.AddChoice($"{accountDetails.Number} {accountDetails.Name} {accountDetails.Balance} {accountDetails.Currency} {accountDetails.Interest}");
                        break;
                    }
                    case Checking checkingAccount:
                    {
                        var accountDetails = checkingAccount.GetAccountInformation();
                        accounts.AddChoice($"{accountDetails.Number} {accountDetails.Name} {accountDetails.Balance} {accountDetails.Currency}");
                        break;
                    }
                }
            }

            var choice = AnsiConsole.Prompt(accounts);
            return choice;
        }

        public CurrencyType ChooseCurrencyType()
        {
            var currencyChoices = new SelectionPrompt<string>()
                .Title("Choose currency type")
                .PageSize(5);

            foreach (CurrencyType type in CurrencyType.GetValuesAsUnderlyingType<CurrencyType>())
            {
                currencyChoices.AddChoice($"{type}");
            }

            var choice = AnsiConsole.Prompt(currencyChoices);
            return Enum.TryParse(choice, out CurrencyType test) ? test : CurrencyType.AED;
        }
    }
}
