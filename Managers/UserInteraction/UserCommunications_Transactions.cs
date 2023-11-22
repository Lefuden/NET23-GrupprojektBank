using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers.Transactions;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum) MakeDepositMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Deposit Menu");
            var accountChoices = GetBankAccountInfo(bankAccounts);
            var selectedAccountChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .PageSize(15)
                    .HighlightStyle(HHStyle)
                    .Title($"[{BankAccountColors["Title"]}]Select an account to deposit to[/]\n\n{accountChoices.SelectionPromptTitle}")
                    .AddChoices(accountChoices.AccountInformationList));

            int chosenAccountNumber = GetSingleMatch(pattern, selectedAccountChoice);
            var selectedAccount = bankAccounts.FirstOrDefault(account => account.GetAccountNumber() == chosenAccountNumber);

            if (selectedAccount == null)
            {
                AnsiConsole.MarkupLine($"[{BankAccountColors["Warning"]}]Selected account type is not supported for deposits.[/]");
                return default;
            }

            var info = selectedAccount.GetAccountInformation();


            while (true)
            {
                decimal depositAmount = AnsiConsole.Ask<decimal>($"[{BankAccountColors["Choice"]}]How much would you like to deposit?[/]");

                if (depositAmount >= 0)
                {
                    decimal currentBalance = decimal.Parse(info.Balance);
                    AnsiConsole.Clear();
                    WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Deposit Menu");
                    WriteTransactionInformation(info, TransactionType.Deposit, depositAmount, currentBalance);
                    // Vi skriver ut ett table just innan med denna info fint!
                    //AnsiConsole.MarkupLine($"[{BankAccountColors["Balance"]}]{depositAmount:c} {info.Currency}[/] [{BankAccountColors["Choice"]}]has been deposited to:[/] [{BankAccountColors["AccountName"]}] {info.Name}[/]\n");

                    AddThisAmountOfNewLines(1);
                    FakeBackChoice("Ok");

                    CurrencyType currencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), info.CurrencyType ?? CurrencyType.SEK.ToString());
                    return (selectedAccount, currencyTypeParsed, DateTime.UtcNow, depositAmount);
                }
                AnsiConsole.MarkupLine($"[{BankAccountColors["Warning"]}]Please enter a valid deposit amount[/]");
            }
        }

        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum, double InterestRate) MakeLoanMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Loan Menu");
            AddThisAmountOfNewLines(1);
            var interest = DecideInterestRate(bankAccounts, true);
            if (interest < 0.3)
            {
                AnsiConsole.MarkupLine($"[{BankAccountColors["Choice"]}]You will get an interest rate of:[/] [{BankAccountColors["Balance"]}]{interest:p}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[{BankAccountColors["Choice"]}]You will get an interest rate of:[/] [{BankAccountColors["Warning"]}]{interest:p}[/]");
            }
            switch (AskUserYesOrNo("Do you want to continue?"))
            {
                case false:
                    return default;

                case true:
                    break;
            }

            AnsiConsole.Clear();

            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Loan Menu");
            var accountChoices = GetBankAccountInfo(bankAccounts);

            var selectedAccountChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(HHStyle)
                .PageSize(20)
                .Title($"[{BankAccountColors["Title"]}]What account would like to deposit the loan to?[/]\n\n" + accountChoices.SelectionPromptTitle)
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

                // Vi skriver ut ett table just innan med denna info fint!
                //AnsiConsole.MarkupLine($"[{BankAccountColors["Warning"]}]You have too many loans and too little balance on your accounts to make a new loan.\nYour current maximum allowed loan limit is:[/][{BankAccountColors["Balance"]} {maximumAmountAllowedToLoan:.##} {info.Currency}.[/]\n");

                FakeBackChoice("Ok");
                return default;
            }

            while (true)
            {
                decimal loanAmount = AnsiConsole.Ask<decimal>($"[{BankAccountColors["Choice"]}]How Much Would You Like To Loan?[/] [{BankAccountColors["Balance"]}] (Maximum: {maximumAmountAllowedToLoan:.##})[/]");
                decimal controlValueForMaximumAmountAllowedToLoan = maximumAmountAllowedToLoan < 0 ? maximumAmountAllowedToLoan * -1 : maximumAmountAllowedToLoan;
                if (loanAmount >= 0 && loanAmount <= controlValueForMaximumAmountAllowedToLoan)
                {
                    AnsiConsole.Clear();
                    WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Loan Menu");
                    WriteTransactionInformation(info, TransactionType.Loan, loanAmount, loanAmount);
                    // Vi skriver ut ett table just innan med denna info fint!
                    //AnsiConsole.MarkupLine($"[{BankAccountColors["Success"]}]You have successfully applied for a loan for:[/] [{BankAccountColors["Balance"]}]{loanAmount:.##} {info.Currency}[/]");

                    AddThisAmountOfNewLines(1);
                    FakeBackChoice("Ok");
                    CurrencyType currencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), info.CurrencyType ?? CurrencyType.SEK.ToString());
                    return (selectedAccount, currencyTypeParsed, DateTime.UtcNow, loanAmount, interest);

                }

                AnsiConsole.MarkupLine($"[{BankAccountColors["Warning"]}]Please enter a valid amount between:[/] [{BankAccountColors["Balance"]}]0 - {maximumAmountAllowedToLoan}.[/]");
            }
        }
        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum) MakeWithdrawalMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Withdrawal Menu");

            var accountChoices = GetBankAccountInfo(bankAccounts);

            var selectedAccountChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(20)
                    .HighlightStyle(HHStyle)
                    .Title($"[{BankAccountColors["Title"]}]Select an Account to Withdraw from[/]\n\n" + accountChoices.SelectionPromptTitle)
                    .AddChoices(accountChoices.AccountInformationList)
                );

            int chosenAccountNumber = GetSingleMatch(pattern, selectedAccountChoice);

            var selectedAccount = bankAccounts.FirstOrDefault(account => account.GetAccountNumber() == chosenAccountNumber);

            if (selectedAccount == null)
            {
                return default;
            }

            var info = selectedAccount.GetAccountInformation();

            if (decimal.TryParse(info.Balance, out decimal accountBalance) is not true)
            {
                return default;
            }


            decimal withdrawalAmount = AnsiConsole.Ask<decimal>($"[{BankAccountColors["Title"]}]How Much Would You Like To Withdraw?[/] [{BankAccountColors["Balance"]}](Maximum: {accountBalance})[/]");

            while (withdrawalAmount < 0 || withdrawalAmount > accountBalance)
            {
                AnsiConsole.MarkupLine($"[{BankAccountColors["Warning"]}]Invalid withdrawal amount. Please enter a valid amount between 0 and {accountBalance}.[/]");
                withdrawalAmount = AnsiConsole.Ask<decimal>($"[{BankAccountColors["Title"]}]How Much Would You Like To Withdraw?[/] [{BankAccountColors["Balance"]}](Maximum: {accountBalance})[/]");
            }
            AnsiConsole.Clear();
            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Withdrawal Menu");
            WriteTransactionInformation(info, TransactionType.Withdrawal, withdrawalAmount, accountBalance);
            // Vi skriver ut ett table just innan med denna info fint!
            //AnsiConsole.MarkupLine($"[{BankAccountColors["Balance"]}]{withdrawalAmount:c} {info.Currency}[/] [{BankAccountColors["Title"]}]has been withdrawn from {info.Name}[/]\n");

            AddThisAmountOfNewLines(1);
            FakeBackChoice("Ok");
            CurrencyType currencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), info.CurrencyType ?? CurrencyType.SEK.ToString());
            return (selectedAccount, currencyTypeParsed, DateTime.UtcNow, withdrawalAmount);
        }






        public static void TestCurrencyExchangeRate()
        {
            Console.Clear();
            while (true)
            {
                var selectedAccountChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(7)
                        .Title("Select a conversion")
                        .MoreChoicesText("Scroll down for more options")
                        .AddChoices(
                            "Back",
                            "Convert From SEK to EUR",
                            "Convert from SEK to USD",
                            "Convert from EUR to SEK",
                            "Convert from EUR to USD",
                            "Convert from USD to SEK",
                            "Convert from USD to EUR"
                            )
                );
                var SEK = CurrencyType.SEK;
                var EUR = CurrencyType.EUR;
                var USD = CurrencyType.USD;

                var sekExchange = CurrencyExchangeRate.GetCurrentCurrencyExchangeRate(SEK);
                var eurExchange = CurrencyExchangeRate.GetCurrentCurrencyExchangeRate(EUR);
                var usdExchange = CurrencyExchangeRate.GetCurrentCurrencyExchangeRate(USD);
                decimal amountToConvert = 0;
                decimal sum = 0;
                switch (selectedAccountChoice)
                {
                    case "Convert From SEK to EUR":
                        amountToConvert = AnsiConsole.Ask<decimal>("[purple]How much SEK would you like to convert to EUR[/]: ");
                        sum = (decimal)sekExchange[EUR] * amountToConvert;
                        AnsiConsole.Write(new Markup($"[orange1 bold]Original Amount: {amountToConvert} SEK[/]\t[green bold]Converted Sum: {sum} EUR[/]\t[cyan1 bold]ExchangeRate: {sekExchange[EUR]}[/]"));
                        AnsiConsole.WriteLine();
                        break;

                    case "Convert from SEK to USD":
                        amountToConvert = AnsiConsole.Ask<decimal>("[purple]How much SEK would you like to convert to USD[/]: ");
                        sum = (decimal)sekExchange[USD] * amountToConvert;
                        AnsiConsole.Write(new Markup($"[orange1 bold]Original Amount: {amountToConvert} SEK[/]\t[green bold]Converted Sum: {sum} USD[/]\t[cyan1 bold]ExchangeRate: {sekExchange[USD]}[/]"));
                        AnsiConsole.WriteLine();
                        break;

                    case "Convert from EUR to SEK":
                        amountToConvert = AnsiConsole.Ask<decimal>("[purple]How much EUR would you like to convert to SEK[/]: ");
                        sum = (decimal)eurExchange[SEK] * amountToConvert;
                        AnsiConsole.Write(new Markup($"[orange1 bold]Original Amount: {amountToConvert} EUR[/]\t[green bold]Converted Sum: {sum} SEK[/]\t[cyan1 bold]ExchangeRate: {eurExchange[SEK]}[/]"));
                        AnsiConsole.WriteLine();
                        break;

                    case "Convert from EUR to USD":
                        amountToConvert = AnsiConsole.Ask<decimal>("[purple]How much EUR would you like to convert to USD[/]: ");
                        sum = (decimal)eurExchange[USD] * amountToConvert;
                        AnsiConsole.Write(new Markup($"[orange1 bold]Original Amount: {amountToConvert} EUR[/]\t[green bold]Converted Sum: {sum} USD[/]\t[cyan1 bold]ExchangeRate: {eurExchange[USD]}[/]"));
                        AnsiConsole.WriteLine();
                        break;

                    case "Convert from USD to SEK":
                        amountToConvert = AnsiConsole.Ask<decimal>("[purple]How much USD would you like to convert to SEK[/]: ");
                        sum = (decimal)usdExchange[SEK] * amountToConvert;
                        AnsiConsole.Write(new Markup($"[orange1 bold]Original Amount: {amountToConvert} USD[/]\t[green bold]Converted Sum: {sum} SEK[/]\t[cyan1 bold]ExchangeRate: {usdExchange[SEK]}[/]"));
                        AnsiConsole.WriteLine();
                        break;

                    case "Convert from USD to EUR":
                        amountToConvert = AnsiConsole.Ask<decimal>("[purple]How much USD would you like to convert to EUR[/]: ");
                        sum = (decimal)usdExchange[EUR] * amountToConvert;
                        AnsiConsole.Write(new Markup($"[orange1 bold]Original Amount:{amountToConvert} USD[/]\t[green bold]Converted Sum: {sum} EUR[/]\t[cyan1 bold]ExchangeRate: {usdExchange[EUR]}[/]"));
                        AnsiConsole.WriteLine();
                        break;

                    default:
                        return;
                }
            }
        }
    }
}
