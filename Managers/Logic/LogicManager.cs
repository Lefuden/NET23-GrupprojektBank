using NET23_GrupprojektBank.Managers.Database;
using NET23_GrupprojektBank.Managers.Login;
using NET23_GrupprojektBank.Managers.Transactions;
using NET23_GrupprojektBank.Managers.UserInteraction;
using NET23_GrupprojektBank.Users;
using Newtonsoft.Json;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.Logic
{
    internal class LogicManager
    {
        private List<User> Users { get; set; }
        private LoginManager? LoginManager { get; set; }
        private Customer? CurrentCustomer { get; set; }
        private Admin? CurrentAdmin { get; set; }
        private TransactionsManager TransactionsManager { get; set; }
        private UserChoice Choice { get; set; }
        private UserChoice PreviousChoice { get; set; }
        private bool KeepRunning { get; set; }

        public LogicManager(bool usingDatabase = false)
        {

            TransactionsManager = new();
            Choice = UserChoice.ViewWelcomeMenu;
            PreviousChoice = UserChoice.ViewWelcomeMenu;
            KeepRunning = true;
            CurrentCustomer = default;
            CurrentAdmin = default;
            if (usingDatabase)
            {
                if (File.Exists(@"..\..\..\UriAdress.txt"))
                {
                    DatabaseManager.SetUriAddress(File.ReadAllText(@"..\..\..\UriAdress.txt"));
                }
            }
            else
            {
                //Users = new()
                //{
                //    new Customer("Tobias", "password",new PersonInformation("Tobias", "Skog",    new DateTime(1991, 10, 28), new ContactInformation(new Email("tobias@edugrade.com")))),
                //    new Customer("Daniel", "password",new PersonInformation("Daniel", "Frykman", new DateTime(1985, 05, 13), new ContactInformation(new Email("daniel@edugrade.com")))),
                //    new Customer("Wille",  "password",new PersonInformation("Wille",  "Skog",    new DateTime(1994, 03, 22), new ContactInformation(new Email("wille@edugrade.com")))),
                //    new Customer("Efrem",  "password",new PersonInformation("Efrem",  "Ghebre",  new DateTime(1979, 03, 22), new ContactInformation(new Email("efrem@edugrade.com"))))
                //};

            }
        }



        public async Task GetUserChoice()
        {
            Users = await DatabaseManager.GetAllUsersFromDB();
            LoginManager = new(Users);

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
                        var info = LoginManager.Login(loginInfo.Username, loginInfo.Password);

                        switch (info.EventStatus)
                        {
                            case EventStatus.LoginSuccess:
                                if (info.User != null)
                                {
                                    if (info.User is Customer customer)
                                    {
                                        CurrentCustomer = customer;
                                        await CurrentCustomer.AddLog(info.EventStatus);
                                        Choice = UserChoice.ViewCustomerMenu;
                                    }
                                    else if (info.User is Admin admin)
                                    {
                                        CurrentAdmin = admin;
                                        await CurrentAdmin.AddLog(info.EventStatus);
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
                        await Logout();
                        break;

                    case UserChoice.Exit:
                        await Exit();
                        break;

                    case UserChoice.ViewAdminMenu:
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        //Choice = UserCommunications.AdminMenu();
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
                            //Transaction newTransaction = UserCommunications.MakeTransferMenu(customerBankAccounts);
                            //if (newTransaction is not null)
                            //{
                            //    TransactionsManager.AddTransaction(newTransaction);
                            //}
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
                            //Transaction newTransaction = UserCommunications.MakeDepositMenu(customerBankAccounts);
                            //if (newTransaction is not null)
                            //{
                            //    TransactionsManager.AddTransaction(newTransaction);
                            //}
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
                            var customerBankAccounts = await DatabaseManager.GetAllUserBankAccountsFromDB(CurrentCustomer);
                            // TransactionManager in AddTransaction() add a method call to dequeue!!!
                            //Transaction newTransaction = UserCommunications.MakeWithdrawalMenu(customerBankAccounts);
                            //if (newTransaction is not null)
                            //{
                            //    TransactionsManager.AddTransaction(newTransaction);
                            //}
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
                            var customerBankAccounts = await DatabaseManager.GetAllUserBankAccountsFromDB(CurrentCustomer);
                            //Transaction newTransaction = UserCommunications.MakeLoanMenu(customerBankAccounts);
                            //if (newTransaction is not null)
                            //{
                            //    TransactionsManager.AddTransaction(newTransaction);
                            //}
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
                            var userLogs = await DatabaseManager.GetAllUserLogsFromDB(CurrentCustomer);
                            CurrentCustomer.ShowLogs(userLogs);
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        else if (CurrentAdmin is not null)
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            var userLogs = await DatabaseManager.GetAllUserLogsFromDB(CurrentAdmin);
                            CurrentAdmin.ShowLogs(userLogs);
                            Choice = UserChoice.ViewAdminMenu;
                        }
                        break;

                    case UserChoice.ViewBalance:
                        if (CurrentCustomer is not null)
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            var customerBankAccounts = await DatabaseManager.GetAllUserBankAccountsFromDB(CurrentCustomer);
                            UserCommunications.ViewBankAccounts(customerBankAccounts);
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
                            CurrentCustomer.CreateCheckingAccount();
                            await DatabaseManager.UpdateSpecificUserInDB(CurrentCustomer);
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
                            CurrentCustomer.CreateSavingsAccount();
                            await DatabaseManager.UpdateSpecificUserInDB(CurrentCustomer);
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
                                await AddNewUser(newCustomer);
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
                                //DatabaseManager.AddSpecificUserToDB(URI, newAdmin).Wait();
                                string jsonUser = JsonConvert.SerializeObject(newAdmin, Formatting.Indented);
                                Console.WriteLine(jsonUser);
                                Console.ReadKey();
                                await AddNewUser(newAdmin);
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

        private async Task AddNewUser(User newUser)
        {
            if (Users is not null && newUser is not null)
            {
                if (Users.Exists(user => user.Equals(newUser)))
                {
                    AnsiConsole.Write("User already exists!");
                }
                else
                {
                    await DatabaseManager.AddSpecificUserToDB(newUser);
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

        private async Task Logout()
        {
            Users = await DatabaseManager.GetAllUsersFromDB();
            LoginManager = new(Users);
            CurrentAdmin = null;
            CurrentCustomer = null;

        }
        private async Task Exit()
        {
            Users = await DatabaseManager.GetAllUsersFromDB();
            await DatabaseManager.UpdateAllUsers(Users);
            KeepRunning = false;
            Choice = UserChoice.Exit;
        }
    }
}

