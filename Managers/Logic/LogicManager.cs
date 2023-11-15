using NET23_GrupprojektBank.Managers.Login;
using NET23_GrupprojektBank.Managers.Transactions;
using NET23_GrupprojektBank.Managers.UserInteraction;
using NET23_GrupprojektBank.Users;

namespace NET23_GrupprojektBank.Managers.Logic
{
    internal class LogicManager
    {
        private LoginManager LoginManager { get; set; }
        private Customer? CurrentCustomer { get; set; }
        private Admin? CurrentAdmin { get; set; }
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
            CurrentCustomer = default;
            CurrentAdmin = default;
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

                    case UserChoice.Back:
                        Choice = PreviousChoice;
                        PreviousChoice = UserChoice.ViewWelcomeMenu;
                        break;

                    case UserChoice.Exit:
                        Exit();
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
                            CurrentCustomer.MakeTransfer();
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
                            CurrentCustomer.MakeDeposit();
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
                            CurrentCustomer.MakeWithdrawal();
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
                            Choice = UserChoice.ViewCustomerMenu;
                            CurrentCustomer.MakeLoan();
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
                            CurrentCustomer.CreateChecking();
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
                            CurrentCustomer.CreateSavings();
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                        }
                        break;

                    case UserChoice.CreateUser:
                        if (CurrentAdmin is not null)
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = CurrentAdmin.CreateUserAccount();
                        }
                        else
                        {
                            PreviousChoice = UserChoice.ViewCustomerMenu;
                            Choice = UserChoice.ViewCustomerMenu;
                        }
                        break;

                    case UserChoice.CreateCustomer:
                        if (CurrentAdmin is not null)
                        {
                            PreviousChoice = UserChoice.ViewAdminMenu;
                            Choice = UserChoice.ViewAdminMenu;
                            Customer newCustomer = CurrentAdmin.CreateCustomerAccount();
                            if (newCustomer is not null)
                            {
                                LoginManager.AddNewUser(newCustomer);
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
                            Admin newAdmin = CurrentAdmin.CreateAdminAccount();
                            if (newAdmin is not null)
                            {
                                LoginManager.AddNewUser(newAdmin);
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

        private void Exit()
        {
            KeepRunning = false;
            Choice = UserChoice.Exit;
        }

        // DETTA SKA BORT! GÖR BARA LITE TESTER HÄR!

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

