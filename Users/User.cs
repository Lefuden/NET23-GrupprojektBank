using System.Diagnostics;
using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Users.UserInformation;
using NET23_GrupprojektBank.Managers.Logs;

namespace NET23_GrupprojektBank.Users
{
    public enum UserType
    {
        Customer,
        Admin,
        Undeclared
    }

    internal abstract class User
    {
        protected string UserName { get; set; }
        protected Guid UserId { get; set; }
        protected string Salt { get; set; }
        protected string HashedPassword { get; set; }
        protected PersonInformation Person { get; set; }
        protected UserType UserType { get; set; }
        protected List<Log> Logs { get; set; }
        
        public User(string userName, string password, PersonInformation person)
        {
            UserName = userName;
            Person = person;
            UserId = Guid.NewGuid();
            Salt = BCrypt.Net.BCrypt.GenerateSalt();
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(password + Salt);
            Logs = new List<Log>();
            UserType = UserType.Undeclared;
        }

        internal bool CompareUserName(string userName)
        {
            return userName == UserName;
        }

        internal bool CompareUserPassword(string userPassword)
        {
            return BCrypt.Net.BCrypt.Verify(userPassword + Salt, HashedPassword);
        }

        internal void Addlog(EventStatus eventStatus)
        {
            Logs.Add(new Log(DateTime.Now, this, GetLogMessage(eventStatus)));

        }
        public void ShowLog(List<Log> list)
        {
            foreach (var o in list)
            {
                Console.WriteLine(o);
            }
        }

        private string GetLogMessage(EventStatus eventStatus)
        {
            {
                return eventStatus switch
                {
                    EventStatus.AccountCreationFailed => $"{UserName} failed to create account",
                    EventStatus.AccountCreationSuccess => $"{UserName} successfully created an account",
                    EventStatus.AdressFailed => $"{UserName} failed to add address",
                    EventStatus.AdressSuccess => $"{UserName} has added an address",
                    EventStatus.CheckingCreationFailed => $"{UserName} checking account creation failed",
                    EventStatus.CheckingCreationSuccess => $"{UserName} checking account creation is a great success",
                    EventStatus.ContactInformationFailed => $"{UserName} ContactInformationFailed",
                    EventStatus.ContactInformationSuccess => $"{UserName} ContactInformationSuccess",
                    EventStatus.CurrencyExchangeRateUpdateFailed => $"{UserName} CurrencyExchangeRateUpdateFailed",
                    EventStatus.CurrencyExchangeRateUpdateSuccess => $"{UserName} CurrencyExchangeRateUpdateSuccess",
                    EventStatus.DepositFailed => $"{UserName} DepositFailed",
                    EventStatus.DepositSuccess => $"{UserName} DepositSuccess",
                    EventStatus.EmailFailed => $"{UserName} EmailFailed",
                    EventStatus.EmailSuccess => $"{UserName} EmailSuccess",
                    EventStatus.InvalidInput => $"{UserName} InvalidInput",
                    EventStatus.LoanFailed => $"{UserName} LoanFailed",
                    EventStatus.LoanSuccess => $"{UserName} LoanSuccess",
                    EventStatus.LoginFailed => $"{UserName} LoginFailed",
                    EventStatus.LoginSuccess => $"{UserName} LoginSuccess",
                    EventStatus.LoginLocked => $"{UserName} LoginLocked",
                    EventStatus.PhoneFailed => $"{UserName}PhoneFailed",
                    EventStatus.PhoneSuccess => $"{UserName}PhoneSuccess",
                    EventStatus.SavingCreationFailed => $"{UserName}SavingCreationFailed",
                    EventStatus.SavingsCreationSuccess => $"{UserName} SavingsCreationSuccess",
                    EventStatus.TransactionFailed => $"{UserName} TransactionFailed",
                    EventStatus.TransactionSuccess => $"{UserName} TransactionSuccess",
                    EventStatus.TransferFailed => $"{UserName} TransferFailed",
                    EventStatus.TransferSuccess => $"{UserName} TransferSuccess",
                    EventStatus.WithdrawalFailed => $"{UserName} WithdrawalFailed",
                    EventStatus.WithdrawalSuccess => $"{UserName} WithdrawalSuccess",
                    _ => $"{UserName} something has gone terribly wrong"
                };
            }
        }
    }
}