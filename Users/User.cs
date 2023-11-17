using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Managers.Logs;
using NET23_GrupprojektBank.Users.UserInformation;
using Newtonsoft.Json;

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
        [JsonProperty]
        protected string Username { get; set; }
        [JsonProperty]
        protected Guid UserId { get; set; }
        [JsonProperty]
        protected string Salt { get; set; }
        [JsonProperty]
        protected string HashedPassword { get; set; }
        [JsonProperty]
        protected PersonInformation PersonInformation { get; set; }
        [JsonProperty]
        protected UserType UserType { get; set; }
        [JsonProperty]
        protected List<Log> Logs { get; set; }

        public User(string userName, string password, PersonInformation personInformation)
        {
            Username = userName;
            PersonInformation = personInformation;
            UserId = Guid.NewGuid();
            Salt = BCrypt.Net.BCrypt.GenerateSalt();
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(password + Salt);
            Logs = new List<Log>();
            UserType = UserType.Undeclared;

        }
        [JsonConstructor]
        public User(string username, Guid userId, string salt, string hashedPassword, PersonInformation personInformation, UserType userType, List<Log> logs)
        {
            Username = username;
            UserId = userId;
            Salt = salt;
            HashedPassword = hashedPassword;
            PersonInformation = personInformation;
            if (Logs is null)
            {
                Logs = new();
            }
            if (logs is not null)
            {
                Logs = logs;
            }
            UserType = userType;
        }

        internal bool CompareUsername(string userName)
        {
            if (userName == Username)
            {
                AddLog(EventStatus.ValidUsername);
                return true;
            }
            AddLog(EventStatus.InvalidUsername);
            return false;
        }

        public Guid GetUserId() => UserId;
        internal string GetUsername() => Username;

        internal bool CompareUserPassword(string userPassword)
        {
            if (BCrypt.Net.BCrypt.Verify(userPassword + Salt, HashedPassword))
            {
                AddLog(EventStatus.ValidPassword);
                return true;
            }
            AddLog(EventStatus.InvalidPassword);
            return false;
        }

        internal void AddLog(EventStatus eventStatus) => Logs.Add(new Log(DateTime.Now, this, GetLogMessage(eventStatus)));

        public void ShowLogs()
        {
            if (Logs.Count <= 0)
                Console.WriteLine("No activity has been logged.");
            else
            {
                foreach (var log in Logs)
                {
                    Console.WriteLine(log);
                }
            }
        }

        //make better log messages, humans need to read it
        public string GetLogMessage(EventStatus eventStatus)
        {
            {
                return eventStatus switch
                {
                    EventStatus.AccountCreationFailed => $"{Username} failed to create account",
                    EventStatus.AccountCreationSuccess => $"{Username} successfully created an account",
                    EventStatus.AdressFailed => $"{Username} failed to add address",
                    EventStatus.AdressSuccess => $"{Username} has added an address",
                    EventStatus.CheckingCreationFailed => $"{Username} checking account creation failed",
                    EventStatus.CheckingCreationSuccess => $"{Username} successfully created a checking account",
                    EventStatus.ContactInformationFailed => $"{Username} failed to add contact information",
                    EventStatus.ContactInformationSuccess => $"{Username} successfully added contact information",
                    EventStatus.CurrencyExchangeRateUpdateFailed => $"{Username} currency exchange rate update failed",
                    EventStatus.CurrencyExchangeRateUpdateSuccess => $"{Username} currency exchange rate update success",
                    EventStatus.DepositFailed => $"{Username} deposit failed",
                    EventStatus.DepositFailedNegativeOrZeroSum => $"",
                    EventStatus.DepositSuccess => $"{Username} deposit success",
                    EventStatus.DepositCreated => $"{Username} deposit success",
                    EventStatus.EmailFailed => $"{Username} failed to add email",
                    EventStatus.EmailSuccess => $"{Username} email has been added",
                    EventStatus.InvalidInput => $"{Username} invalid input",
                    EventStatus.LoanFailed => $"{Username} bank loan failed",
                    EventStatus.LoanFailedNegativeOrZeroSum => $"",
                    EventStatus.LoanSuccess => $"{Username} bank loan success",
                    EventStatus.LoanCreated => $"{Username} bank loan created",
                    EventStatus.LoginFailed => $"{Username} login failed",
                    EventStatus.LoginSuccess => $"{Username} successfully logged in",
                    EventStatus.LoginLocked => $"{Username} login locked",
                    EventStatus.PhoneFailed => $"{Username} failed to add phone number",
                    EventStatus.PhoneSuccess => $"{Username} phone number has been added",
                    EventStatus.SavingsCreationFailed => $"{Username} savings account creation failed",
                    EventStatus.SavingsCreationSuccess => $"{Username} savings account creation success",
                    EventStatus.TransactionFailed => $"{Username} transaction failed",
                    EventStatus.TransactionSuccess => $"{Username} transaction success",
                    EventStatus.TransactionCreated => $"{Username} transaction created",
                    EventStatus.TransferFailed => $"{Username} transfer failed",
                    EventStatus.TransferFailedInsufficientFunds => $"",
                    EventStatus.TransferFailedNegativeOrZeroSum => $"",
                    EventStatus.TransferSuccess => $"{Username} transfer success",
                    EventStatus.TransferCreated => $"{Username} transfer created",
                    EventStatus.WithdrawalCreated => $"{Username} withdrawal created",
                    EventStatus.WithdrawalFailed => $"{Username} withdrawal failed, outside of balance bounds",
                    EventStatus.WithdrawalFailedInsufficientFunds => $"",
                    EventStatus.WithdrawalSuccess => $"{Username} withdrawal approved",
                    EventStatus.ValidUsername => $"{Username} valid username",
                    EventStatus.InvalidUsername => $"{Username} invalid username",
                    EventStatus.ValidPassword => $"{Username} valid password",
                    EventStatus.InvalidPassword => $"{Username} invalid password",
                    _ => $"{Username} something has gone terribly wrong"
                };
            }
        }
    }
}