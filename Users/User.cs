using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Managers.Logs;
using NET23_GrupprojektBank.Users.UserInformation;

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
        protected PersonInformation PersonInformation { get; set; }
        protected UserType UserType { get; set; }
        protected List<Log> Logs { get; set; }

        public User(string userName, string password, PersonInformation personInformation)
        {
            UserName = userName;
            PersonInformation = personInformation;
            UserId = Guid.NewGuid();
            Salt = BCrypt.Net.BCrypt.GenerateSalt();
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(password + Salt);
            Logs = new List<Log>();
            UserType = UserType.Undeclared;
        }

        internal bool CompareUserName(string userName) => userName == UserName;
        internal bool CompareUserPassword(string userPassword) => BCrypt.Net.BCrypt.Verify(userPassword + Salt, HashedPassword);
        internal void Addlog(EventStatus eventStatus) => Logs.Add(new Log(DateTime.Now, this, GetLogMessage(eventStatus)));
        
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
                    EventStatus.CheckingCreationSuccess => $"{UserName} successfully created a checking account",
                    EventStatus.ContactInformationFailed => $"{UserName} failed to add contact information",
                    EventStatus.ContactInformationSuccess => $"{UserName} successfully added contact information",
                    EventStatus.CurrencyExchangeRateUpdateFailed => $"{UserName} currency exchange rate update failed",
                    EventStatus.CurrencyExchangeRateUpdateSuccess => $"{UserName} currency exchange rate update success",
                    EventStatus.DepositFailed => $"{UserName} deposit failed",
                    EventStatus.DepositSuccess => $"{UserName} deposit success",
                    EventStatus.EmailFailed => $"{UserName} failed to add email",
                    EventStatus.EmailSuccess => $"{UserName} email has been added",
                    EventStatus.InvalidInput => $"{UserName} invalid input",
                    EventStatus.LoanFailed => $"{UserName} bank loan failed",
                    EventStatus.LoanSuccess => $"{UserName} bank loan success",
                    EventStatus.LoginFailed => $"{UserName} login failed",
                    EventStatus.LoginSuccess => $"{UserName} successfully logged in",
                    EventStatus.LoginLocked => $"{UserName} login locked",
                    EventStatus.PhoneFailed => $"{UserName} failed to add phone number",
                    EventStatus.PhoneSuccess => $"{UserName} phone number has been added",
                    EventStatus.SavingCreationFailed => $"{UserName} savings account creation failed",
                    EventStatus.SavingsCreationSuccess => $"{UserName} savings account creation success",
                    EventStatus.TransactionFailed => $"{UserName} transaction failed",
                    EventStatus.TransactionSuccess => $"{UserName} transaction success",
                    EventStatus.TransferFailed => $"{UserName} transfer failed",
                    EventStatus.TransferSuccess => $"{UserName} transfer success",
                    EventStatus.WithdrawalFailed => $"{UserName} withdrawal failed, outside of balance bounds",
                    EventStatus.WithdrawalSuccess => $"{UserName} withdrawal approved",
                    _ => $"{UserName} something has gone terribly wrong"
                };
            }
        }
    }
}