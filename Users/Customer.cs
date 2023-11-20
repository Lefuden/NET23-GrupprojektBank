using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Managers.Transactions;
using NET23_GrupprojektBank.Managers.UserInteraction;
using NET23_GrupprojektBank.Users.UserInformation;
using Newtonsoft.Json;
using Spectre.Console;

namespace NET23_GrupprojektBank.Users
{
    internal class Customer : User
    {
        [JsonProperty]
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

                double interest = bankAccountTypeToBeCreated == BankAccountType.Savings ? DecideInterestRate() : 0;

                AnsiConsole.Write(new Markup($"[green]Account type[/]: {bankAccountTypeToBeCreated}\n[green]Account number[/]: {bankAccountNr}\n[green]Account name[/]: {bankAccountName}\n[green]Account currency type[/]: {currencyType}{(bankAccountTypeToBeCreated == BankAccountType.Savings ? $"\n[green]Interest[/]: {interest:p}" : "")}\n\n", Style.WithDecoration(Decoration.RapidBlink)).LeftJustified());


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
        private double DecideInterestRate(bool isMakingLoan = false)
        {
            Random rng = new();
            int highestValue = 0;
            decimal totalAmountOfMoneyOnBankAccounts = 0;
            double interest = 0;
            foreach (var bankAccount in BankAccounts)
            {
                totalAmountOfMoneyOnBankAccounts += bankAccount.GetBalance();
            }
            if (isMakingLoan)
            {
                highestValue = totalAmountOfMoneyOnBankAccounts <= 0 ? 100 : totalAmountOfMoneyOnBankAccounts <= 10000 ? 80 : totalAmountOfMoneyOnBankAccounts <= 25000 ? 60 : totalAmountOfMoneyOnBankAccounts <= 50000 ? 40 : totalAmountOfMoneyOnBankAccounts <= 100000 ? 30 : totalAmountOfMoneyOnBankAccounts <= 500000 ? 20 : 10;
                interest = rng.Next(0, highestValue + 1);
                interest *= 0.01;
            }
            else
            {
                interest = rng.NextDouble() / 100;
            }

            return interest;
        }
        public Transaction MakeLoan()
        {
            //BankAccount.GetBalance();
            //logic to create a loan
            //send to createtransaction for completion
            AddLog(EventStatus.LoanCreated);
            while (true)
            {
                Console.WriteLine("make loan.");
                CurrencyType currencyType = UserCommunications.ChooseCurrencyType();
                Console.WriteLine($"{currencyType}\n\n");
                //switch (Admin.AskUserYesOrNo("is this information correct?"))
                //{
                //    case true:
                //        Console.Clear();
                //        break;
                //    case false:
                //        Console.Clear();
                //        break;
                //}
            }
            return CreateTransaction(); //return transaction. return null if cancelled.
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

            return new Transaction(this, info.SourceBankAccount, info.SourceCurrencyType, TransactionType.Withdrawal, info.Sum);
        }

        public Transaction MakeTransfer()
        {
            var info = UserCommunications.MakeTransferMenu(BankAccounts);
            AddLog(EventStatus.TransferCreated);

            return new Transaction(this, info.SourceBankAccount, info.SourceCurrencyType, TransactionType.Withdrawal, info.Sum);
        }
        private Transaction CreateTransaction()
        {
            throw new NotImplementedException();
        }



    }
}
