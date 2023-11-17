using NET23_GrupprojektBank.Managers.Database;
using NET23_GrupprojektBank.Managers.Login;
using NET23_GrupprojektBank.Managers.Transactions;
using NET23_GrupprojektBank.Managers.UserInteraction;
using NET23_GrupprojektBank.Users;
using NET23_GrupprojektBank.Users.UserContactInformation;
using NET23_GrupprojektBank.Users.UserInformation;
using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;
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
        private string URI { get; set; }

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
                    URI = File.ReadAllText(@"..\..\..\UriAdress.txt");
                    Console.WriteLine(URI);
                    Users = DatabaseManager.GetAllUsersFromDB(URI).Result;
                }
            }
            else
            {
                Users = new()
                {
                    new Customer("Tobias", "password",new PersonInformation("Tobias", "Skog",    new DateTime(1991, 10, 28), new ContactInformation(new Email("tobias@edugrade.com")))),
                    new Customer("Daniel", "password",new PersonInformation("Daniel", "Frykman", new DateTime(1985, 05, 13), new ContactInformation(new Email("daniel@edugrade.com")))),
                    new Customer("Wille",  "password",new PersonInformation("Wille",  "Skog",    new DateTime(1994, 03, 22), new ContactInformation(new Email("wille@edugrade.com")))),
                    new Customer("Efrem",  "password",new PersonInformation("Efrem",  "Ghebre",  new DateTime(1979, 03, 22), new ContactInformation(new Email("efrem@edugrade.com"))))
                };
            }
            LoginManager = new(Users);
        }

        public void GetUserChoice()
        {
            EventStatus eventStatus;
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
                                        CurrentCustomer.Addlog(info.EventStatus);
                                        Choice = UserChoice.ViewCustomerMenu;
                                    }
                                    else if (info.User is Admin admin)
                                    {
                                        CurrentAdmin = admin;
                                        CurrentAdmin.Addlog(info.EventStatus);
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

                    case UserChoice.ViewLockedMenu:
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        //UserCommunications.LockedMenu();
                        Choice = UserChoice.ViewWelcomeMenu;
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
                        Exit();
                        break;

                    case UserChoice.ViewAdminMenu:
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        //Choice = UserCommunications.AdminMenu();
                        Choice = UserCommunications.CustomerMenu();
                        break;

                    case UserChoice.ViewCustomerMenu:
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        Choice = UserCommunications.CustomerMenu();
                        break;

                    case UserChoice.MakeTransfer:
                        if (CurrentCustomer is not null)
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            //Transaction newTransaction = CurrentCustomer.MakeTransfer();
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
                            //Transaction newTransaction = CurrentCustomer.MakeDeposit();
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
                            //Transaction newTransaction = CurrentCustomer.MakeWithdrawal();
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
                            //Transaction newTransaction = CurrentCustomer.MakeLoan();
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
                            CurrentCustomer.ShowLogs();
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        if (CurrentAdmin is not null)
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
                            // Choice = CurrentCustomer.CreateBankAccount();
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

        private void AddNewUser(User newUser)
        {
            if (Users is not null && newUser is not null)
            {
                if (Users.Exists(user => user.Equals(newUser)))
                {
                    AnsiConsole.Write("User already exists!");
                }

                Users.Add(newUser);
                DatabaseManager.PostSpecificUser(URI, newUser).Wait();
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
            LoginManager = new(Users);
            CurrentAdmin = null;
            CurrentCustomer = null;
        }
        private void Exit()
        {
            KeepRunning = false;
            Choice = UserChoice.Exit;
        }
    }
}

