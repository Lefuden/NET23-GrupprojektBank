using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static void ViewBankAccounts(List<BankAccount> bankAccounts)
        {
            if (bankAccounts.Count == 0)
            {
                AnsiConsole.MarkupLine($"{BankAccountColors[""]}" You have no accounts in the bank![/]");
                FakeBackChoice("Back");
                return;
            }
            var table = GetBankAccountsAsTable(bankAccounts);
            table.Title($"{BankAccountColors["Title"]}Bank Accounts[/]");
            table.BorderColor(Color.Purple);

            AnsiConsole.Write(table);
            FakeBackChoice("Back");
        }
        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum) MakeDepositMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider($"Deposit Menu");
            var accountChoices = GetBankAccountInfo(bankAccounts);
            var selectedAccountChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .PageSize(15)
                    .Title($"Select an account to deposit to\n{accountChoices.SelectionPromptTitle}")
                    .AddChoices(accountChoices.AccountInformationList));

            int chosenAccountNumber = GetSingleMatch(pattern, selectedAccountChoice);
            var selectedAccount = bankAccounts.FirstOrDefault(account => account.GetAccountNumber() == chosenAccountNumber);

            if (selectedAccount == null)
            {
                AnsiConsole.MarkupLine("[red]Selected account type is not supported for deposits.[/]");
                return default;
            }

            var info = selectedAccount.GetAccountInformation();
            WriteTransactionInformation(info);

            while (true)
            {
                decimal depositAmount = AnsiConsole.Ask<decimal>($"[purple]How much would you like to deposit?[/]");

                if (depositAmount >= 0)
                {
                    decimal currentBalance = decimal.Parse(info.Balance);
                    currentBalance += depositAmount;
                    info.Balance = currentBalance.ToString();

                    AnsiConsole.MarkupLine($"[green]{depositAmount:c} {info.Currency}[/] [purple]has been deposited to {info.Name}[/]\n");
                    FakeBackChoice("Ok");

                    CurrencyType currencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), info.Currency ?? CurrencyType.SEK.ToString());
                    return (selectedAccount, currencyTypeParsed, DateTime.UtcNow, depositAmount);
                }
                AnsiConsole.MarkupLine($"[red]Please enter a valid deposit amount[/]");
            }
        }

        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum, double InterestRate) MakeLoanMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider($"Loan Menu");

            var interest = DecideInterestRate(bankAccounts, true);
            AnsiConsole.MarkupLine($"You will get an interest rate of: [green]{interest:p}[/]");
            switch (AskUserYesOrNo("Do you want to continue?"))
            {
                case false:
                    return default;

                case true:
                    break;
            }

            Console.Clear();

            WriteDivider($"Loan Menu");

            var accountChoices = GetBankAccountInfo(bankAccounts);

            var selectedAccountChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .PageSize(20)
                .Title("What account would like to deposit the loan to?\n" + accountChoices.SelectionPromptTitle)
                .AddChoices(accountChoices.AccountInformationList)
             );

            int chosenAccountNumber = GetSingleMatch(pattern, selectedAccountChoice.ToString());

            var selectedAccount = bankAccounts.FirstOrDefault(account => (account.GetAccountNumber() == chosenAccountNumber));

            if (selectedAccount == null)
            {
                return default;
            }

            var info = selectedAccount.GetAccountInformation();
            var currentLoanWithInterest = GetMaximumLoanAllowedWithInterestCalculated(bankAccounts);
            var maximumAmountAllowedToLoan = (accountChoices.TotalSumOnAccounts * 5) - currentLoanWithInterest;
            if (maximumAmountAllowedToLoan <= 0)
            {

                AnsiConsole.MarkupLine($"[red]You have too many loans and too little balance on your accounts to make a new loan.\nYour current maximum allowed loan limit is: {maximumAmountAllowedToLoan:.##}-{info.Currency}.[/]\n");
                FakeBackChoice("Ok");
                return default;
            }
            WriteTransactionInformation(info);

            while (true)
            {
                decimal loanAmount = AnsiConsole.Ask<decimal>($"[purple]How Much Would You Like To Loan?[/] [gold1] (Maximum: {maximumAmountAllowedToLoan:.##})[/]");
                decimal controlValueForMaximumAmountAllowedToLoan = maximumAmountAllowedToLoan < 0 ? maximumAmountAllowedToLoan * -1 : maximumAmountAllowedToLoan;
                if (loanAmount >= 0 && loanAmount <= controlValueForMaximumAmountAllowedToLoan)
                {
                    AnsiConsole.MarkupLine($"[green]You have successfully applied for a loan for:[/] [purple]{loanAmount:.##}-{info.Currency}[/]");

                    FakeBackChoice("Ok");
                    CurrencyType currencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), info.Currency ?? CurrencyType.SEK.ToString());
                    return (selectedAccount, currencyTypeParsed, DateTime.UtcNow, loanAmount, interest);

                }

                AnsiConsole.MarkupLine($"[red]Please enter a valid amount between 0 and {maximumAmountAllowedToLoan}.[/]");
            }
        }













        public static CurrencyType ChooseCurrencyType()
        {
            WriteDivider("Choose Currency Type");
            var currencyChoices = new SelectionPrompt<string>()
                .Title("Choose currency type")
                .PageSize(3);

            foreach (CurrencyType type in CurrencyType.GetValuesAsUnderlyingType<CurrencyType>())
            {
                if (type is not CurrencyType.INVALID)
                {
                    currencyChoices.AddChoice($"{type}");
                }
            }

            var choice = AnsiConsole.Prompt(currencyChoices);
            Console.Clear();
            return Enum.TryParse(choice, out CurrencyType selectedType) ? selectedType : CurrencyType.SEK;
        }


        public static decimal GetMaximumLoanAllowedWithInterestCalculated(List<BankAccount> bankAccounts)
        {

            decimal currentLoanWithInterestRateCalculated = 0;
            foreach (var account in bankAccounts)
            {
                currentLoanWithInterestRateCalculated += account.GetLoanAmount() * (decimal)account.GetLoanInterestRate() * 100;
            }

            return currentLoanWithInterestRateCalculated;
        }
    }
}
