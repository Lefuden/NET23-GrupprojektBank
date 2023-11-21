﻿using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Managers.Logs;
using NET23_GrupprojektBank.Managers.UserInteraction;
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
        protected string Username { get; set; }
        protected Guid UserId { get; set; }
        protected string Salt { get; set; }
        protected string HashedPassword { get; set; }
        protected PersonInformation PersonInformation { get; set; }
        protected UserType UserType { get; set; }
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

        internal bool CompareUsername(string userName)
        {
            if (userName == Username)
            {
                AddLog(EventStatus.ValidUsername);
                return true;
            }
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

        internal void AddLog(EventStatus eventStatus) => Logs.Add(new Log(DateTime.Now, GetLogMessage(eventStatus)));

        public void ShowLogs()
        {
            if (Logs is null)
            {
                Logs = new List<Log>();
            }
            UserCommunications.ShowLogs(Logs, Username);
        }

        public string GetLogMessage(EventStatus eventStatus)
        {
            {
                return eventStatus switch
                {
                    EventStatus.AccountCreationFailed => $"{Username} failed to create account",
                    EventStatus.AccountCreationSuccess => $"{Username} created an account",
                    EventStatus.AdressFailed => $"{Username} failed to add address",
                    EventStatus.AdressSuccess => $"{Username} added an address",
                    EventStatus.CheckingCreationFailed => $"{Username} failed to create a checking account",
                    EventStatus.CheckingCreationSuccess => $"{Username} created a checking account",
                    EventStatus.ContactInformationFailed => $"{Username} failed to add contact information",
                    EventStatus.ContactInformationSuccess => $"{Username} added contact information",
                    EventStatus.CurrencyExchangeRateUpdateFailed => $"{Username} failed to update currency exchange rate",
                    EventStatus.CurrencyExchangeRateUpdateSuccess => $"{Username} updated currency exchange rate",
                    EventStatus.DepositFailed => $"{Username} failed to make a deposit",
                    EventStatus.DepositFailedNegativeOrZeroSum => $"{Username} failed to make a deposit, negative or zero input",
                    EventStatus.DepositSuccess => $"{Username} deposit approved",
                    EventStatus.DepositCreated => $"{Username} made a deposit",
                    EventStatus.EmailFailed => $"{Username} failed to add email",
                    EventStatus.EmailSuccess => $"{Username} added email",
                    EventStatus.InvalidInput => $"{Username} invalid input",
                    EventStatus.LoanFailed => $"{Username} failed to make a bank loan",
                    EventStatus.LoanFailedNegativeOrZeroSum => $"{Username} failed to make a bank loan, negative or zero input",
                    EventStatus.LoanSuccess => $"{Username} bank loan approved",
                    EventStatus.LoanCreated => $"{Username} made a bank loan",
                    EventStatus.LoginFailed => $"{Username} failed to login",
                    EventStatus.LoginSuccess => $"{Username} successfully logged in",
                    EventStatus.LoginLocked => $"{Username} has been locked out",
                    EventStatus.PhoneFailed => $"{Username} failed to add phone number",
                    EventStatus.PhoneSuccess => $"{Username} added a phone number",
                    EventStatus.SavingsCreationFailed => $"{Username} failed to create a savings account",
                    EventStatus.SavingsCreationSuccess => $"{Username} created a savings account",
                    EventStatus.TransactionFailed => $"{Username} failed to make a transaction",
                    EventStatus.TransactionSuccess => $"{Username} transaction approved",
                    EventStatus.TransactionCreated => $"{Username} made a transaction",
                    EventStatus.TransactionManagerAddedToQueueSuccess => $"{Username} added transaction to transaction manager queue",
                    EventStatus.TransactionManagerAddedToQueueFailed => $"{Username} failed to add transaction to transaction manager queue",
                    EventStatus.TransferFailed => $"{Username} failed to make a transfer",
                    EventStatus.TransferFailedInsufficientFunds => $"{Username} failed to make a transfer, insufficient funds",
                    EventStatus.TransferFailedNegativeOrZeroSum => $"{Username} failed to make a transfer, negative or zero input",
                    EventStatus.TransferSuccess => $"{Username} transfer approved",
                    EventStatus.TransferCreated => $"{Username} made a transfer",
                    EventStatus.WithdrawalCreated => $"{Username} made a withdrawal",
                    EventStatus.WithdrawalFailed => $"{Username} failed to make a withdrawal",
                    EventStatus.WithdrawalFailedInsufficientFunds => $"{Username} failed to make a withdrawal, insufficient funds",
                    EventStatus.WithdrawalSuccess => $"{Username} withdrawal approved",
                    EventStatus.ValidUsername => $"{Username} entered a valid username",
                    EventStatus.InvalidUsername => $"{Username} entered an invalid username",
                    EventStatus.ValidPassword => $"{Username} entered a valid password",
                    EventStatus.InvalidPassword => $"{Username} entered an invalid password",
                    _ => $"{Username} something has gone terribly wrong"
                };
            }
        }
    }
}