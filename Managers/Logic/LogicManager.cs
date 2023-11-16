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
            if (File.Exists(@"..\..\..\UriAdress.txt"))
            {
                URI = File.ReadAllText(@"..\..\..\UriAdress.txt");
                Console.WriteLine(URI);
            }

            if (usingDatabase)
            {
                var dbResponse = DatabaseManager.GetAllUsersFromDB(URI);
                Users = dbResponse.Result;
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
                                    if (info.User is Admin admin)
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

                            case EventStatus.LoginLocked:
                                Choice = UserChoice.ViewLockedMenu;
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
                            Choice = CurrentCustomer.CreateBankAccount();
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

        private void AddNewUser(User user)
        {
            if (Users is not null && user is not null)
            {
                Users.Add(user);
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


        public EventStatus DisplayLockoutScreen(DateTime lockoutTimeStart, int lockoutDuration)
        {
            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    // Define tasks
                    var task1 = ctx.AddTask("[green]Reticulating splines[/]", true, lockoutDuration);

                    while (!ctx.IsFinished)
                    {
                        while (DateTime.UtcNow.Subtract(lockoutTimeStart).TotalSeconds < lockoutDuration)
                        {
                            int remainingTime = lockoutDuration - (int)DateTime.UtcNow.Subtract(lockoutTimeStart).TotalSeconds;
                            Console.CursorVisible = false;
                            Console.Clear();
                            Console.WriteLine($"You are Locked. Remaining time {remainingTime} seconds.");
                            Thread.Sleep(1000);
                            task1.Increment(1);
                        }
                    }
                });


            return EventStatus.LoginUnlocked;
        }

    }
}


// DETTA SKA BORT! GÖR BARA LITE TESTER HÄR!

//    private static UserChoice ConvertEventStatusToUserChoice(EventStatus eventStatus)
//    {
//        return eventStatus switch
//        {
//            EventStatus.LoginSuccess => UserChoice.ViewCustomerMenu,
//            EventStatus.LoginFailed => /* Handle LoginFailed */,
//            EventStatus.LoginLocked => /* Handle LoginLocked */,
//            EventStatus.CurrencyExchangeRateUpdateSuccess => /* Handle CurrencyExchangeRateUpdateSuccess */,
//            EventStatus.CurrencyExchangeRateUpdateFailed => /* Handle CurrencyExchangeRateUpdateFailed */,
//            EventStatus.CheckingCreationSuccess => /* Handle CheckingCreationSuccess */,
//            EventStatus.CheckingCreationFailed => /* Handle CheckingCreationFailed */,
//            EventStatus.SavingsCreationSuccess => /* Handle SavingsCreationSuccess */,
//            EventStatus.SavingCreationFailed => /* Handle SavingCreationFailed */,
//            EventStatus.TransactionSuccess => /* Handle TransactionSuccess */,
//            EventStatus.TransactionFailed => /* Handle TransactionFailed */,
//            EventStatus.DepositSuccess => /* Handle DepositSuccess */,
//            EventStatus.DepositFailed => /* Handle DepositFailed */,
//            EventStatus.WithdrawalSuccess => /* Handle WithdrawalSuccess */,
//            EventStatus.WithdrawalFailed => /* Handle WithdrawalFailed */,
//            EventStatus.TransferSuccess => /* Handle TransferSuccess */,
//            EventStatus.TransferFailed => /* Handle TransferFailed */,
//            EventStatus.LoanSuccess => /* Handle LoanSuccess */,
//            EventStatus.LoanFailed => /* Handle LoanFailed */,
//            EventStatus.AccountCreationSuccess => /* Handle AccountCreationSuccess */,
//            EventStatus.AccountCreationFailed => /* Handle AccountCreationFailed */,
//            EventStatus.AdressSuccess => /* Handle AdressSuccess */,
//            EventStatus.AdressFailed => /* Handle AdressFailed */,
//            EventStatus.EmailSuccess => /* Handle EmailSuccess */,
//            EventStatus.EmailFailed => /* Handle EmailFailed */,
//            EventStatus.PhoneSuccess => /* Handle PhoneSuccess */,
//            EventStatus.PhoneFailed => /* Handle PhoneFailed */,
//            EventStatus.ContactInformationSuccess => /* Handle ContactInformationSuccess */,
//            EventStatus.ContactInformationFailed => /* Handle ContactInformationFailed */,
//            EventStatus.InvalidInput => /* Handle InvalidInput */,
//            EventStatus.TransactionManagerAddedToQueueSuccess => /* Handle TransactionManagerAddedToQueueSuccess */,
//            EventStatus.TransactionManagerAddedToQueueFailed => /* Handle TransactionManagerAddedToQueueFailed */,
//            EventStatus.NonAdminUser => /* Handle NonAdminUser */,
//            EventStatus.AdminUpdatedCurrencyFromFile => /* Handle AdminUpdatedCurrencyFromFile */,
//            EventStatus.AdminUpdatedCurrencyFromWebApi => /* Handle AdminUpdatedCurrencyFromWebApi */,
//            EventStatus.AdminInvalidInput => /* Handle AdminInvalidInput */,
//        }


//}

//internal static class EventStatusManager
//{
//    public static Log GetLogFromEventStatus(EventStatus eventStatus, User? user = default)
//    {
//        if (user is null)
//        {
//            return new Log(DateTime.UtcNow, GetLogMessage(eventStatus));
//        }

//        return new Log(DateTime.UtcNow, user, GetLogMessage(eventStatus));

//    }
//    public static string GetLogMessage(EventStatus eventStatus)
//    {
//        return eventStatus switch
//        {
//            EventStatus.LoginSuccess => /* Handle LoginSuccess */,
//            EventStatus.LoginFailed => /* Handle LoginFailed */,
//            EventStatus.LoginLocked => /* Handle LoginLocked */,
//            EventStatus.CurrencyExchangeRateUpdateSuccess => /* Handle CurrencyExchangeRateUpdateSuccess */,
//            EventStatus.CurrencyExchangeRateUpdateFailed => /* Handle CurrencyExchangeRateUpdateFailed */,
//            EventStatus.CheckingCreationSuccess => /* Handle CheckingCreationSuccess */,
//            EventStatus.CheckingCreationFailed => /* Handle CheckingCreationFailed */,
//            EventStatus.SavingsCreationSuccess => /* Handle SavingsCreationSuccess */,
//            EventStatus.SavingCreationFailed => /* Handle SavingCreationFailed */,
//            EventStatus.TransactionSuccess => /* Handle TransactionSuccess */,
//            EventStatus.TransactionFailed => /* Handle TransactionFailed */,
//            EventStatus.DepositSuccess => /* Handle DepositSuccess */,
//            EventStatus.DepositFailed => /* Handle DepositFailed */,
//            EventStatus.WithdrawalSuccess => /* Handle WithdrawalSuccess */,
//            EventStatus.WithdrawalFailed => /* Handle WithdrawalFailed */,
//            EventStatus.TransferSuccess => /* Handle TransferSuccess */,
//            EventStatus.TransferFailed => /* Handle TransferFailed */,
//            EventStatus.LoanSuccess => /* Handle LoanSuccess */,
//            EventStatus.LoanFailed => /* Handle LoanFailed */,
//            EventStatus.AccountCreationSuccess => /* Handle AccountCreationSuccess */,
//            EventStatus.AccountCreationFailed => /* Handle AccountCreationFailed */,
//            EventStatus.AdressSuccess => /* Handle AdressSuccess */,
//            EventStatus.AdressFailed => /* Handle AdressFailed */,
//            EventStatus.EmailSuccess => /* Handle EmailSuccess */,
//            EventStatus.EmailFailed => /* Handle EmailFailed */,
//            EventStatus.PhoneSuccess => /* Handle PhoneSuccess */,
//            EventStatus.PhoneFailed => /* Handle PhoneFailed */,
//            EventStatus.ContactInformationSuccess => /* Handle ContactInformationSuccess */,
//            EventStatus.ContactInformationFailed => /* Handle ContactInformationFailed */,
//            EventStatus.InvalidInput => /* Handle InvalidInput */,
//            EventStatus.TransactionManagerAddedToQueueSuccess => /* Handle TransactionManagerAddedToQueueSuccess */,
//            EventStatus.TransactionManagerAddedToQueueFailed => /* Handle TransactionManagerAddedToQueueFailed */,
//            EventStatus.NonAdminUser => /* Handle NonAdminUser */,
//            EventStatus.AdminUpdatedCurrencyFromFile => /* Handle AdminUpdatedCurrencyFromFile */,
//            EventStatus.AdminUpdatedCurrencyFromWebApi => /* Handle AdminUpdatedCurrencyFromWebApi */,
//            EventStatus.AdminInvalidInput => /* Handle AdminInvalidInput */,



//            EventStatus.AccountCreationFailed => $"{Username} failed to create account",
//            EventStatus.AccountCreationSuccess => $"{Username} successfully created an account",
//            EventStatus.AdressFailed => $"{Username} failed to add address",
//            EventStatus.AdressSuccess => $"{Username} has added an address",
//            EventStatus.CheckingCreationFailed => $"{Username} checking account creation failed",
//            EventStatus.CheckingCreationSuccess => $"{Username} checking account creation is a great success",
//            EventStatus.ContactInformationFailed => $"{Username} ContactInformationFailed",
//            EventStatus.ContactInformationSuccess => $"{Username} ContactInformationSuccess",
//            EventStatus.CurrencyExchangeRateUpdateFailed => $"{Username} CurrencyExchangeRateUpdateFailed",
//            EventStatus.CurrencyExchangeRateUpdateSuccess => $"{Username} CurrencyExchangeRateUpdateSuccess",
//            EventStatus.DepositFailed => $"{Username} DepositFailed",
//            EventStatus.DepositSuccess => $"{Username} DepositSuccess",
//            EventStatus.EmailFailed => $"{Username} EmailFailed",
//            EventStatus.EmailSuccess => $"{Username} EmailSuccess",
//            EventStatus.InvalidInput => $"{Username} InvalidInput",
//            EventStatus.LoanFailed => $"{Username} LoanFailed",
//            EventStatus.LoanSuccess => $"{Username} LoanSuccess",
//            EventStatus.LoginFailed => $"{Username} LoginFailed",
//            EventStatus.LoginSuccess => $"{Username} LoginSuccess",
//            EventStatus.LoginLocked => $"{Username} LoginLocked",
//            EventStatus.PhoneFailed => $"{Username}PhoneFailed",
//            EventStatus.PhoneSuccess => $"{Username}PhoneSuccess",
//            EventStatus.SavingCreationFailed => $"{Username}SavingCreationFailed",
//            EventStatus.SavingsCreationSuccess => $"{Username} SavingsCreationSuccess",
//            EventStatus.TransactionFailed => $"{Username} TransactionFailed",
//            EventStatus.TransactionSuccess => $"{Username} TransactionSuccess",
//            EventStatus.TransferFailed => $"{Username} TransferFailed",
//            EventStatus.TransferSuccess => $"{Username} TransferSuccess",
//            EventStatus.WithdrawalFailed => $"{Username} WithdrawalFailed",
//            EventStatus.WithdrawalSuccess => $"{Username} WithdrawalSuccess",
//            _ => $"{Username} something has gone terribly wrong"
//        };
//    }
//}

