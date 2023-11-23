using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers.Transactions;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static (BankAccount SourceBankAccount, BankAccount DestinationBankAccount, CurrencyType SourceCurrencyType, CurrencyType DestinationCurrencyType, DateTime DateAndTime, decimal Sum) MakeTransferMenu(List<BankAccount> bankAccounts, List<BankAccount> allBankAccounts)
        {
            AnsiConsole.Clear();
            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Transfer Menu");
            BankAccount destinationAccount;
            List<BankAccount> transferAccountListExcludingSelected;
            decimal transferAmount = 0;
            var transferDirection = AskUserWhereToTransferTo();

            var sourceAccount = GetBankAccountFromUserPrompt(bankAccounts, TransactionType.Transfer);

            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Transfer Menu");

            if (sourceAccount == null)
            {
                return default;
            }

            var sourceAccountInfo = sourceAccount.GetAccountInformation();

            switch (transferDirection)
            {
                case TransferTo.Personal:
                    transferAccountListExcludingSelected = bankAccounts.Where(acc => acc.GetAccountNumber() != sourceAccount.GetAccountNumber()).ToList();
                    destinationAccount = GetBankAccountFromUserPrompt(transferAccountListExcludingSelected, TransactionType.Transfer, true);

                    if (destinationAccount == null)
                    {
                        return default;
                    }
                    break;

                case TransferTo.Other:
                    transferAccountListExcludingSelected = allBankAccounts.Where(acc => acc.GetAccountNumber() != sourceAccount.GetAccountNumber()).ToList();

                    var destinationTransferInfo = FindDestinationTransferAccountWithAccountNumber(transferAccountListExcludingSelected);
                    while (destinationTransferInfo.FoundAccount is not true)
                    {
                        AnsiConsole.Clear();
                        WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Transfer Menu");

                        if (AskUserYesOrNo("Did not find an account with given account number, try again?") is not true)
                        {
                            return default;
                        }
                        destinationTransferInfo = FindDestinationTransferAccountWithAccountNumber(transferAccountListExcludingSelected);
                    }
                    if (destinationTransferInfo.DestinationAccount == null)
                    {
                        return default;
                    }
                    destinationAccount = destinationTransferInfo.DestinationAccount;
                    break;

                default:
                    return default;
            }

            while (true)
            {
                AnsiConsole.Clear();
                WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Transfer Menu");
                transferAmount = AnsiConsole.Ask<decimal>($"[{BankAccountColors["Title"]}]Enter the amount to transfer (Maximum:[/][{BankAccountColors["Balance"]}] {sourceAccount.GetBalance()}[/][{BankAccountColors["Title"]}])[/]:");

                if (transferAmount > 0 && transferAmount <= sourceAccount.GetBalance())
                {
                    break;
                }
                AnsiConsole.MarkupLine($"[{BankAccountColors["Warning"]}]Please enter a valid transfer amount between 0 and {sourceAccount.GetBalance()}.[/]");
                if (AskUserYesOrNo("Try again?") is not true)
                {
                    return default;
                }
            }

            var destinationAccountInfo = destinationAccount.GetAccountInformation();


            CurrencyType sourceCurrencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), sourceAccountInfo.CurrencyType ?? CurrencyType.SEK.ToString());
            CurrencyType destinationCurrencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), destinationAccountInfo.CurrencyType ?? CurrencyType.SEK.ToString());

            AnsiConsole.Clear();
            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Transfer Menu");

            if (AskUserYesOrNo($"Do you want to transfer[/] [{BankAccountColors["Balance"]}]{transferAmount:0.##} {sourceAccountInfo.CurrencyType}[/]{MenuColors["Title"]} to {destinationAccount.GetAccountInformation().BankAccountName}?") is not true)
            {
                return default;
            }
            AnsiConsole.Clear();
            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Transfer Menu");
            sourceAccountInfo.BankAccountName = destinationAccountInfo.BankAccountName;
            WriteTransactionInformation(sourceAccountInfo, TransactionType.Transfer, transferAmount, sourceAccount.GetBalance());
            AddThisAmountOfNewLines(1);
            FakeBackChoice("Ok");

            return (sourceAccount, destinationAccount, sourceCurrencyTypeParsed, destinationCurrencyTypeParsed, DateTime.UtcNow, transferAmount);

        }
        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum) MakeDepositMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Deposit Menu");
            var selectedAccount = GetBankAccountFromUserPrompt(bankAccounts, TransactionType.Deposit);

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
                    // Vi skriver ut ett table just innan med denna sourceAccountInfo fint!
                    //AnsiConsole.MarkupLine($"[{BankAccountColors["Balance"]}]{depositAmount:c} {sourceAccountInfo.Currency}[/] [{BankAccountColors["Choice"]}]has been deposited to:[/] [{BankAccountColors["AccountName"]}] {sourceAccountInfo.Name}[/]\n");

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
            if (AskUserYesOrNo("Do you want to continue?") is not true)
            {
                return default;
            }

            AnsiConsole.Clear();


            var selectedAccount = GetBankAccountFromUserPrompt(bankAccounts, TransactionType.Loan);

            AnsiConsole.Clear();
            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Loan Menu");

            if (selectedAccount is null)
            {
                return default;
            }

            var allAccountInformation = GetBankAccountInfo(bankAccounts);
            var info = selectedAccount.GetAccountInformation();
            var currentLoanWithInterest = GetMaximumLoanAllowedWithInterestCalculated(bankAccounts);
            var maximumAmountAllowedToLoan = (allAccountInformation.TotalSumOnAccounts * 5) - currentLoanWithInterest;
            if (maximumAmountAllowedToLoan <= 0)
            {

                // Vi skriver ut ett table just innan med denna sourceAccountInfo fint!
                //AnsiConsole.MarkupLine($"[{BankAccountColors["Warning"]}]You have too many loans and too little balance on your accounts to make a new loan.\nYour current maximum allowed loan limit is:[/][{BankAccountColors["Balance"]} {maximumAmountAllowedToLoan:.##} {sourceAccountInfo.Currency}.[/]\n");

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
                    WriteTransactionInformation(info, TransactionType.Loan, loanAmount, selectedAccount.GetBalance());
                    // Vi skriver ut ett table just innan med denna sourceAccountInfo fint!
                    //AnsiConsole.MarkupLine($"[{BankAccountColors["Success"]}]You have successfully applied for a loan for:[/] [{BankAccountColors["Balance"]}]{loanAmount:.##} {sourceAccountInfo.Currency}[/]");

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
            AnsiConsole.Clear();
            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Withdrawal Menu");

            var selectedAccount = GetBankAccountFromUserPrompt(bankAccounts, TransactionType.Withdrawal);


            if (selectedAccount == null)
            {
                return default;
            }

            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Withdrawal Menu");

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
            // Vi skriver ut ett table just innan med denna sourceAccountInfo fint!
            //AnsiConsole.MarkupLine($"[{BankAccountColors["Balance"]}]{withdrawalAmount:c} {sourceAccountInfo.Currency}[/] [{BankAccountColors["Title"]}]has been withdrawn from {sourceAccountInfo.Name}[/]\n");

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
