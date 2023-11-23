using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers.Login;
using NET23_GrupprojektBank.Managers.Transactions;
using NET23_GrupprojektBank.Managers.UserInteraction;
using NET23_GrupprojektBank.Users;
using NET23_GrupprojektBank.Users.UserContactInformation;
using NET23_GrupprojektBank.Users.UserInformation;
using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;
using Spectre.Console;
using Transaction = NET23_GrupprojektBank.Managers.Transactions.Transaction;

namespace NET23_GrupprojektBank.Managers.Logic
{
    internal class LogicManager
    {
        private List<User> Users { get; set; }
        private LoginManager LoginManager { get; set; }
        private Customer? CurrentCustomer { get; set; }
        private Admin? CurrentAdmin { get; set; }
        private TransactionsManager TransactionsManager { get; set; }
        private UserChoice Choice { get; set; }
        private UserChoice PreviousChoice { get; set; }
        private bool KeepRunning { get; set; }
        private int LoginAttempts { get; set; } = 1;


        public LogicManager()
        {
            TransactionsManager = new();
            Choice = UserChoice.ViewWelcomeMenu;
            PreviousChoice = UserChoice.ViewWelcomeMenu;
            KeepRunning = true;
            CurrentCustomer = default;
            CurrentAdmin = default;
            Users = InitializeTestUsers();
            CreateBankAccountsForTestUsers();
            LoginManager = new(Users);
            CurrencyExchangeRate.UpdateCurrencyExchangeRateAsync(UserType.Admin, true).Wait();
        }



        public void GetUserChoice()
        {
            TransactionsManager.Start();
            BankLoggo.StartUpAppLoadingScreen();

            while (KeepRunning)
            {
                Console.Clear();
                switch (Choice)
                {
                    case UserChoice.ViewWelcomeMenu:
                        Choice = UserCommunications.MainMenu();
                        break;

                    case UserChoice.Login:

                        var loginInfo = UserCommunications.GetLoginInfo();
                        var info = LoginManager.Login(loginInfo.Username, loginInfo.Password, LoginAttempts++);

                        switch (info.EventStatus)
                        {
                            case EventStatus.LoginSuccess:
                                if (info.User != null)
                                {
                                    if (info.User is Customer customer)
                                    {
                                        CurrentCustomer = customer;
                                        CurrentCustomer.AddLog(info.EventStatus);
                                        ResetLoginLockout();
                                        BankLoggo.LoginLoadingScreen();
                                        Choice = UserChoice.ViewCustomerMenu;
                                    }
                                    else if (info.User is Admin admin)
                                    {
                                        CurrentAdmin = admin;
                                        CurrentAdmin.AddLog(info.EventStatus);
                                        ResetLoginLockout();
                                        BankLoggo.LoginLoadingScreen();
                                        Choice = UserChoice.ViewAdminMenu;
                                    }
                                    else
                                    {
                                        Choice = UserChoice.ViewWelcomeMenu;
                                    }
                                }
                                break;

                            case EventStatus.LoginFailed:

                                Choice = UserChoice.Login;
                                break;

                            case EventStatus.LoginUnlocked:
                                Choice = UserChoice.ViewWelcomeMenu;
                                ResetLoginLockout();
                                //LoginManager.ResetLoginLockout();
                                break;

                            default:
                                Choice = UserChoice.ViewWelcomeMenu;
                                break;
                        }

                        break;

                    case UserChoice.Back:
                        Choice = PreviousChoice;
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        break;

                    case UserChoice.Logout:
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        Choice = UserChoice.ViewWelcomeMenu;
                        Logout();
                        break;

                    case UserChoice.Exit:
                        Exit().Wait();
                        break;

                    case UserChoice.ViewAdminMenu:
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        Choice = UserCommunications.AdminMenu();
                        break;

                    case UserChoice.ViewCustomerMenu:
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        Choice = UserCommunications.CustomerMenu();
                        break;

                    case UserChoice.MakeTransfer:
                        if (CurrentCustomer is not null)
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            var transactionInfo = CurrentCustomer.MakeTransfer(GetAllBankAccounts());
                            if (transactionInfo.SourceBankAccount is not null)
                            {
                                var destinationUser = Users.First(user =>
                                {
                                    if (user is Customer customer)
                                    {
                                        var userAccounts = customer.GetBankAccounts();
                                        if (userAccounts.Contains(transactionInfo.DestinationBankAccount))
                                        {
                                            return true;
                                        }
                                    }
                                    return false;
                                });
                                Transaction newTransaction = new(CurrentCustomer, destinationUser, transactionInfo.SourceBankAccount, transactionInfo.DestinationBankAccount, transactionInfo.SourceCurrencyType, transactionInfo.DestinationCurrencyType, transactionInfo.DateAndTime, TransactionType.Transfer, transactionInfo.Sum);
                                TransactionsManager.AddTransaction(newTransaction);
                            }
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                        }
                        break;

                    case UserChoice.MakeDeposit:
                        if (CurrentCustomer is not null)
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            Transaction newTransaction = CurrentCustomer.MakeDeposit();
                            if (newTransaction.SourceBankAccount is not null)
                            {
                                TransactionsManager.AddTransaction(newTransaction);
                            }
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                        }
                        break;

                    case UserChoice.MakeWithdrawal:
                        if (CurrentCustomer is not null)
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            Transaction newTransaction = CurrentCustomer.MakeWithdrawal();
                            if (newTransaction.SourceBankAccount is not null)
                            {
                                TransactionsManager.AddTransaction(newTransaction);
                            }
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                        }
                        break;

                    case UserChoice.MakeLoan:
                        if (CurrentCustomer is not null)
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            Transaction newTransaction = CurrentCustomer.MakeLoan();
                            if (newTransaction is not null && newTransaction.SourceBankAccount is not null)
                            {
                                TransactionsManager.AddTransaction(newTransaction);
                            }
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                        }
                        break;

                    case UserChoice.ViewLogs:
                        if (CurrentCustomer is not null)
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            CurrentCustomer.ShowLogs();
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        else if (CurrentAdmin is not null)
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            CurrentAdmin.ShowLogs();
                            Choice = UserChoice.ViewAdminMenu;
                        }
                        break;

                    case UserChoice.ViewBalance:
                        if (CurrentCustomer is not null)
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            CurrentCustomer.ViewBankAccount();
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                        }
                        break;

                    case UserChoice.CreateBankAccount:
                        if (CurrentCustomer is not null)
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            Choice = UserCommunications.CreateBankAccount();
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                        }
                        break;

                    case UserChoice.CreateChecking:
                        if (CurrentCustomer is not null)
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            Choice = UserChoice.ViewCustomerMenu;
                            var unavailableAccountNumbers = GetBankAccountNumbers();
                            CurrentCustomer.CreateBankAccount(unavailableAccountNumbers, BankAccountType.Checking);
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                        }
                        break;

                    case UserChoice.CreateSavings:
                        if (CurrentCustomer is not null)
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            Choice = UserChoice.ViewCustomerMenu;
                            var unavailableAccountNumbers = GetBankAccountNumbers();
                            CurrentCustomer.CreateBankAccount(unavailableAccountNumbers, BankAccountType.Savings);
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                        }
                        break;

                    case UserChoice.CreateCustomer:
                        if (CurrentAdmin is not null)
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                            Customer newCustomer = CurrentAdmin.CreateCustomerAccount(GetAllUsernames());
                            if (newCustomer is not null)
                            {
                                AddNewUser(newCustomer);
                            }
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        break;

                    case UserChoice.CreateAdmin:
                        if (CurrentAdmin is not null)
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                            Admin newAdmin = CurrentAdmin.CreateAdminAccount(GetAllUsernames());

                            if (newAdmin is not null)
                            {
                                AddNewUser(newAdmin);
                            }
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        break;

                    case UserChoice.UpdateCurrencyExchange:
                        if (CurrentAdmin is not null)
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                            CurrentAdmin.UpdateCurrencyExchangeRate();
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        break;

                    case UserChoice.Invalid:
                        if (CurrentAdmin is not null)
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        break;

                    default:
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        Choice = UserChoice.ViewCustomerMenu;
                        break;
                }
            }

        }
        private List<int> GetBankAccountNumbers()
        {
            List<int> bankAccountNumbers = new();
            foreach (var user in Users)
            {
                if (user is Customer customer)
                {
                    foreach (var bankAccountList in customer.GetBankAccounts())
                    {
                        bankAccountNumbers.Add(bankAccountList.GetAccountNumber());
                    }
                }
            }

            return bankAccountNumbers;
        }
        private List<BankAccount> GetAllBankAccounts()
        {

            List<BankAccount> bankAccounts = new();
            foreach (var user in Users)
            {
                if (user is Customer customer)
                {
                    bankAccounts.AddRange(customer.GetBankAccounts());
                }
            }

            return bankAccounts;

        }
        private void AddNewUser(User newUser)
        {
            if (Users is not null && newUser is not null)
            {
                if (Users.Exists(user => user.Equals(newUser)))
                {
                    AnsiConsole.Write("User already exists!");
                }
                else
                {
                    // await DatabaseManager.AddSpecificUserToDB(newUser);
                    Users.Add(newUser);
                }
            }
        }

        private List<string> GetAllUsernames()
        {
            var usernameList = new List<string>();
            foreach (var user in Users)
            {
                usernameList.Add(user.GetUsername());
            }

            return usernameList;
        }

        private void Logout()
        {
            //Users = await DatabaseManager.GetAllUsersFromDB();
            LoginManager = new(Users);
            CurrentAdmin = null;
            CurrentCustomer = null;

        }
        private async Task Exit()
        {
            //Users = await DatabaseManager.GetAllUsersFromDB();
            //await DatabaseManager.UpdateAllUsers(Users);
            //await TransactionsManager.StopAsync();
            KeepRunning = false;
            Choice = UserChoice.Exit;
        }

        private List<User> InitializeTestUsers()
        {
            List<User> users = new()
                {
                    new Admin("ATobias",   "password",new PersonInformation("Tobias", "Skog",    new DateTime(1991, 10, 28), new ContactInformation(new Email("tobias.admin@edugrade.com")))),
                    new Admin("ADaniel",   "password",new PersonInformation("Daniel", "Frykman", new DateTime(1985, 05, 13), new ContactInformation(new Email("daniel.admin@edugrade.com")))),
                    new Admin("AWille",    "password",new PersonInformation("Wille",  "Persson", new DateTime(1994, 03, 22), new ContactInformation(new Email("wille.admin@edugrade.com")))),
                    new Admin("AEfrem",    "password",new PersonInformation("Efrem",  "Ghebre",  new DateTime(1979, 03, 22), new ContactInformation(new Email("efrem.admin@edugrade.com")))),
                    new Customer("Tobias", "password",new PersonInformation("Tobias", "Skog",    new DateTime(1991, 10, 28), new ContactInformation(new Email("tobias@edugrade.com")))),
                    new Customer("Daniel", "password",new PersonInformation("Daniel", "Frykman", new DateTime(1985, 05, 13), new ContactInformation(new Email("daniel@edugrade.com")))),
                    new Customer("Wille",  "password",new PersonInformation("Wille",  "Persson", new DateTime(1994, 03, 22), new ContactInformation(new Email("wille@edugrade.com")))),
                    new Customer("Efrem",  "password",new PersonInformation("Efrem",  "Ghebre",  new DateTime(1979, 03, 22), new ContactInformation(new Email("efrem@edugrade.com"))))
                };
            foreach (var user in users)
            {
                if (user is Customer customer)
                {
                    customer.AddLog(EventStatus.CustomerAccountCreationSuccess);
                }
                if (user is Admin admin)
                {
                    admin.AddLog(EventStatus.AdminAccountCreationSuccess);

                }
            }
            return users;
        }
        private void CreateBankAccountsForTestUsers()
        {
            foreach (var user in Users)
            {
                user.AddLog(EventStatus.AdminAccountCreationSuccess);
                Random rng = new Random();
                if (user is Customer customer)
                {
                    decimal sum;
                    int amountOfAccounts = rng.Next(2, 6);
                    for (int i = 0; i < amountOfAccounts; i++)
                    {
                        sum = rng.Next(0, 1001);
                        customer.AddBankAccount(new Checking(BankAccount.BankAccountNumberGenerator(GetBankAccountNumbers()), $"{CreateAmazingAccountName(i, user.GetUsername())}", CurrencyType.SEK, sum));
                        customer.AddLog(EventStatus.CheckingCreationSuccess);

                        sum = rng.Next(0, 1001);
                        customer.AddBankAccount(new Savings(BankAccount.BankAccountNumberGenerator(GetBankAccountNumbers()), $"{CreateAmazingAccountName(i, user.GetUsername())}", CurrencyType.EUR, sum, UserCommunications.DecideInterestRate(customer.GetBankAccounts())));
                        customer.AddLog(EventStatus.SavingsCreationSuccess);
                    }

                }
            }
        }

        private void ResetLoginLockout()
        {
            LoginAttempts = 1;
        }
        private string CreateAmazingAccountName(int i, string name)
        {
            return i switch
            {
                0 => $"{name}'s Hemliga Godis Konto",
                1 => $"{name}'s Semester Konto",
                2 => $"{name}'s Mat Konto",
                3 => $"{name}'s Reservdels Konto för Ockelbon",
                4 => $"{name}'s Tomma Konto",
                5 => $"{name}'s Hemliga Honungs Konto",
                _ => $"{name}'s Något Blev Fel Konto"
            };
        }
    }
}

