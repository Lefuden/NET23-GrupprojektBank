using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Managers.Transactions;
using NET23_GrupprojektBank.Managers.UserInteraction;
using NET23_GrupprojektBank.Users.UserInformation;
using Spectre.Console;

namespace NET23_GrupprojektBank.Users
{
    internal class Customer : User
    {
        protected List<BankAccount> BankAccounts { get; set; }
        public Customer(string userName, string password, PersonInformation person) : base(userName, password, person)
        {
            UserType = UserType.Customer;
            BankAccounts = new List<BankAccount>();
        }
        public List<BankAccount> GetBankAccounts() => BankAccounts;
        public void AddBankAccount(BankAccount bankAccount) => BankAccounts.Add(bankAccount);

        public void ViewBankAccount()
        {
            if (BankAccounts is null)
            {
                BankAccounts = new List<BankAccount>();
            }
            UserCommunications.ViewBankAccounts(BankAccounts);
        }
        public void CreateBankAccount(List<int> existingBankAccountNumbers, BankAccountType bankAccountTypeToBeCreated)
        {
            while (true)
            {
                Console.Clear();
                var bankAccountName = AnsiConsole.Ask<string>("[green]Account name[/]:");
                var currencyType = UserCommunications.ChooseCurrencyType();
                var bankAccountNr = BankAccount.BankAccountNumberGenerator(existingBankAccountNumbers);
                Console.Clear();

                double interest = bankAccountTypeToBeCreated == BankAccountType.Savings ? UserCommunications.DecideInterestRate(GetBankAccounts()) : 0;

                AnsiConsole.Write(new Markup($"[green]Account type[/]: {bankAccountTypeToBeCreated}\n[green]Account number[/]: {bankAccountNr}\n[green]Account name[/]: {bankAccountName}\n[green]Account currency type[/]: {currencyType}{(bankAccountTypeToBeCreated == BankAccountType.Savings ? $"\n[green]Interest[/]: {interest:p}" : "")}\n\n").LeftJustified());

                if (UserCommunications.AskUserYesOrNo("is this information correct?"))
                {
                    switch (bankAccountTypeToBeCreated)
                    {
                        case BankAccountType.Checking:
                            AddLog(EventStatus.CheckingCreationSuccess);
                            BankAccounts.Add(new Checking(bankAccountNr, bankAccountName, currencyType, 0.0M));
                            break;

                        case BankAccountType.Savings:
                            AddLog(EventStatus.SavingsCreationSuccess);
                            BankAccounts.Add(new Savings(bankAccountNr, bankAccountName, currencyType, 0.0M, interest));
                            break;
                    }
                    return;
                }
            }
        }

        public Transaction MakeLoan()
        {
            if (BankAccounts is not null && BankAccounts.Count <= 0)
            {
                AnsiConsole.MarkupLine("[bold red]You do not currently have any accounts with the bank and cannot take a loan![/]");
                UserCommunications.FakeBackChoice("Back");
                return null;
            }
            decimal totalSum = 0;
            foreach (var bankAccount in BankAccounts)
            {
                totalSum += bankAccount.GetBalance();
            }
            if (totalSum <= 0)
            {
                AnsiConsole.MarkupLine("[bold red]You do not currently have any balance in the bank and cannot take a loan![/]");
                UserCommunications.FakeBackChoice("Back");
                return null;
            }

            var info = UserCommunications.MakeLoanMenu(BankAccounts);
            if (info.SourceBankAccount is null)
            {
                AddLog(EventStatus.LoanFailed);
                return null;
            }
            else
            {
                AddLog(EventStatus.LoanCreated);
                return new Transaction(this, info.SourceBankAccount, info.SourceCurrencyType, TransactionType.Loan, info.Sum, info.InterestRate);
            }
        }

        public Transaction MakeWithdrawal()
        {
            var info = UserCommunications.MakeWithdrawalMenu(BankAccounts);
            AddLog(EventStatus.WithdrawalCreated);

            return new Transaction(this, info.SourceBankAccount, info.SourceCurrencyType, TransactionType.Withdrawal, info.Sum);
        }

        public Transaction MakeDeposit()
        {
            var info = UserCommunications.MakeDepositMenu(BankAccounts);
            AddLog(EventStatus.DepositCreated);

            return new Transaction(this, info.SourceBankAccount, info.SourceCurrencyType, TransactionType.Deposit, info.Sum);
        }

        public (BankAccount SourceBankAccount, BankAccount DestinationBankAccount, CurrencyType SourceCurrencyType, CurrencyType DestinationCurrencyType, DateTime DateAndTime, decimal Sum) MakeTransfer(List<BankAccount> allBankAccounts)
        {
            var info = UserCommunications.MakeTransferMenu(BankAccounts, allBankAccounts);
            if (info.SourceBankAccount is not null)
            {
                AddLog(EventStatus.TransferCreated);
                return info;
            }
            else
            {
                AddLog(EventStatus.TransferFailed);
                return default;
            }
        }
    }
}
