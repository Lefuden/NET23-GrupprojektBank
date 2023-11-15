using NET23_GrupprojektBank.Managers.Login;
using NET23_GrupprojektBank.Managers.Transactions;
using NET23_GrupprojektBank.Users;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.Logic
{
    internal class LogicManager
    {
        private LoginManager LoginManager { get; set; }
        private User? CurrentUser { get; set; }
        private TransactionsManager TransactionsManager { get; set; }
        private UserChoice Choice { get; set; }
        private UserChoice PreviousChoice { get; set; }
        private bool KeepRunning { get; set; }

        public LogicManager()
        {
            LoginManager = new();
            TransactionsManager = new();
            Choice = UserChoice.ViewWelcomeMenu;
            PreviousChoice = UserChoice.ViewWelcomeMenu;
            KeepRunning = true;
            CurrentUser = default;
        }

        public void GetUserChoice()
        {
            EventStatus eventStatus;

            while (KeepRunning)
            {
                switch (Choice)
                {
                    case UserChoice.ViewWelcomeMenu:
                        Choice = MainMenu();
                        break;

                    case UserChoice.Login:
                        var loginInfo = GetLoginInfo();
                        var info = LoginManager.Login(loginInfo.Username, loginInfo.Password);

                        switch (info.EventStatus)
                        {
                            case EventStatus.LoginSuccess:
                                if (info.User != null)
                                {
                                    CurrentUser = info.User;
                                    CurrentUser.Addlog(info.EventStatus);
                                    if (CurrentUser is Customer)
                                    {
                                        Choice = UserChoice.ViewCustomerMenu;
                                    }
                                    else if (CurrentUser is Admin)
                                    {
                                        Choice = UserChoice.ViewAdminMenu;
                                        // AdminMenu();
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

                    case UserChoice.Back:
                        Choice = PreviousChoice;
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        break;

                    case UserChoice.Exit:
                        Exit();
                        break;

                    case UserChoice.ViewAdminMenu:
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        //Choice = AdminMenu();
                        break;

                    case UserChoice.ViewCustomerMenu:
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        Choice = CustomerMenu();
                        break;

                    case UserChoice.CreateTransaction:
                        PreviousChoice = UserChoice.ViewCustomerMenu;
                        // Handle CreateTransaction case
                        break;

                    case UserChoice.MakeWithdrawal:
                        PreviousChoice = UserChoice.CreateTransaction;
                        // Handle MakeWithdrawal case
                        break;

                    case UserChoice.MakeTransfer:
                        PreviousChoice = UserChoice.CreateTransaction;
                        // Handle MakeTransfer case
                        break;

                    case UserChoice.MakeLoan:
                        PreviousChoice = UserChoice.CreateTransaction;
                        // Handle MakeLoan case
                        break;

                    case UserChoice.GetAccounts:
                        PreviousChoice = UserChoice.ViewCustomerMenu;
                        // Handle GetAccounts case
                        break;

                    case UserChoice.ViewLogs:
                        PreviousChoice = UserChoice.ViewCustomerMenu;
                        // Handle ViewLogs case
                        break;

                    case UserChoice.ViewBalance:
                        PreviousChoice = UserChoice.ViewCustomerMenu;
                        // Handle ViewBalance case
                        break;

                    case UserChoice.CreateBankAccount:
                        PreviousChoice = UserChoice.ViewCustomerMenu;
                        // Handle CreateBankAccount case
                        break;

                    case UserChoice.CreateChecking:
                        PreviousChoice = UserChoice.CreateBankAccount;
                        // Handle CreateChecking case
                        break;

                    case UserChoice.CreateSavings:
                        PreviousChoice = UserChoice.CreateBankAccount;
                        // Handle CreateSavings case
                        break;

                    case UserChoice.CreateUser:
                        PreviousChoice = UserChoice.ViewCustomerMenu;
                        // Handle CreateUser case
                        break;

                    case UserChoice.CreateCustomer:
                        PreviousChoice = UserChoice.CreateUser;
                        // Handle CreateCustomer case
                        break;

                    case UserChoice.CreateAdmin:
                        PreviousChoice = UserChoice.CreateUser;
                        // Handle CreateAdmin case
                        break;

                    case UserChoice.UpdateCurrencyExchange:
                        PreviousChoice = UserChoice.ViewAdminMenu;
                        // Handle UpdateCurrencyExchange case
                        break;

                    case UserChoice.Invalid:
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        // Handle Invalid case
                        break;

                    // Add cases for other UserChoice values as needed

                    default:
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        // Handle default case
                        break;
                }
            }

        }

        private void Exit()
        {
            KeepRunning = false;
            Choice = UserChoice.Exit;
        }

        // DETTA SKA BORT! GÖR BARA LITE TESTER HÄR!
        public static UserChoice MainMenu()
        {
            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[purple]Welcome Menu[/]")
                    .PageSize(3)
                    .AddChoices(new[]
                    {
                        "Login",
                        "Exit"
                    }
                ));

            return ConvertStringToUserChoice(stringChoice);
        }
        public static UserChoice CustomerMenu()
        {
            DrawRuler($"Bank Menu");

            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[purple]What would you like to do today?[/]")
                    .PageSize(5)
                    .AddChoices(new[]
                    {
                        "View Account Balance",
                        "Deposit",
                        "Withdraw",
                        "Logout",
                        "Exit"
                    }
                ));

            return ConvertStringToUserChoice(stringChoice);
        }
        private static void DrawRuler(string content, string markup)
        {
            AnsiConsole.Write(new Rule($"{markup}{content}[/]"));
        }
        private static void DrawRuler(string content)
        {
            AnsiConsole.Write(new Rule(content));
        }
        private static UserChoice ConvertStringToUserChoice(string input)
        {
            return input switch
            {

                "Login" => UserChoice.Login,
                "Exit" => UserChoice.Exit,
                "View Account Balance" => UserChoice.ViewBalance,
                "Deposit" => UserChoice.ViewBalance,
                "Withdraw" => UserChoice.ViewBalance,
                "Back" => UserChoice.ViewBalance,
                "Logout" => UserChoice.ViewBalance,
                _ => UserChoice.Invalid

            };
        }


        public static (string Username, string Password) GetLoginInfo()
        {
            var username = AnsiConsole.Prompt(
         new TextPrompt<string>("[orange1]Username: [/]")
                    .PromptStyle("green")
        .Validate(username =>
                        string.IsNullOrWhiteSpace(username)
                        ? ValidationResult.Error("[red]Invalid username[/]")
                        : username.Length < 5 ? ValidationResult.Error("[red]Username must be atleast 5 characters long[/]")
                        : ValidationResult.Success()
                     ));

            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("[orange1]Password: [/]")
                    .PromptStyle("green")
                    .Secret()
                    .Validate(password =>
                        string.IsNullOrEmpty(password) ? ValidationResult.Error("[red]Invalid password[/")
                        : password.Length < 2 ? ValidationResult.Error("[red]Password must be atleast 2 characters long[/]")
                        : ValidationResult.Success()
                    ));

            return (username, password);
        }
    }
}



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



//            EventStatus.AccountCreationFailed => $"{UserName} failed to create account",
//            EventStatus.AccountCreationSuccess => $"{UserName} successfully created an account",
//            EventStatus.AdressFailed => $"{UserName} failed to add address",
//            EventStatus.AdressSuccess => $"{UserName} has added an address",
//            EventStatus.CheckingCreationFailed => $"{UserName} checking account creation failed",
//            EventStatus.CheckingCreationSuccess => $"{UserName} checking account creation is a great success",
//            EventStatus.ContactInformationFailed => $"{UserName} ContactInformationFailed",
//            EventStatus.ContactInformationSuccess => $"{UserName} ContactInformationSuccess",
//            EventStatus.CurrencyExchangeRateUpdateFailed => $"{UserName} CurrencyExchangeRateUpdateFailed",
//            EventStatus.CurrencyExchangeRateUpdateSuccess => $"{UserName} CurrencyExchangeRateUpdateSuccess",
//            EventStatus.DepositFailed => $"{UserName} DepositFailed",
//            EventStatus.DepositSuccess => $"{UserName} DepositSuccess",
//            EventStatus.EmailFailed => $"{UserName} EmailFailed",
//            EventStatus.EmailSuccess => $"{UserName} EmailSuccess",
//            EventStatus.InvalidInput => $"{UserName} InvalidInput",
//            EventStatus.LoanFailed => $"{UserName} LoanFailed",
//            EventStatus.LoanSuccess => $"{UserName} LoanSuccess",
//            EventStatus.LoginFailed => $"{UserName} LoginFailed",
//            EventStatus.LoginSuccess => $"{UserName} LoginSuccess",
//            EventStatus.LoginLocked => $"{UserName} LoginLocked",
//            EventStatus.PhoneFailed => $"{UserName}PhoneFailed",
//            EventStatus.PhoneSuccess => $"{UserName}PhoneSuccess",
//            EventStatus.SavingCreationFailed => $"{UserName}SavingCreationFailed",
//            EventStatus.SavingsCreationSuccess => $"{UserName} SavingsCreationSuccess",
//            EventStatus.TransactionFailed => $"{UserName} TransactionFailed",
//            EventStatus.TransactionSuccess => $"{UserName} TransactionSuccess",
//            EventStatus.TransferFailed => $"{UserName} TransferFailed",
//            EventStatus.TransferSuccess => $"{UserName} TransferSuccess",
//            EventStatus.WithdrawalFailed => $"{UserName} WithdrawalFailed",
//            EventStatus.WithdrawalSuccess => $"{UserName} WithdrawalSuccess",
//            _ => $"{UserName} something has gone terribly wrong"
//        };
//    }
//}

