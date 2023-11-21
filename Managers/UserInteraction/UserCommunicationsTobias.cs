using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers.Logic;
using NET23_GrupprojektBank.Managers.Logs;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static void ShowLogs(List<Log> logs, string username)
        {

            WriteDivider($"Display Logs | {username}");
            var table = GetLogTables(logs);
            table.Expand();
            AnsiConsole.Write(table);
            FakeBackChoice("Back");
        }

        public static void FakeBackChoice(string text)
        {
            AnsiConsole.Cursor.Show(false);
            AnsiConsole.MarkupLine($"[#bd93f9]> {text}[/]");
            Console.ReadKey();
            AnsiConsole.Cursor.Show(true);
        }
        private static Table GetLogTables(List<Log> logs)
        {
            var table = new Table();
            table.AddColumns(new TableColumn("[purple]Message[/]").Centered(), new TableColumn("[purple]Date and Time[/]").Centered(), new TableColumn("[cyan1]Log Id[/]").RightAligned());

            foreach (var log in logs)
            {
                table.AddRow(
                    $"[mediumpurple2]{log.Message}[/]",
                    $"[mediumpurple2]{log.DateAndTime}[/]",
                    $"[mediumpurple2]{log.LogId}[/]");
            }
            table.Border(TableBorder.Rounded);
            return table;
        }
        public static double DecideInterestRate(List<BankAccount> bankAccounts, bool isMakingLoan = false)
        {
            Random rng = new();
            int highestValue = 0;
            decimal totalAmountOfMoneyOnBankAccounts = 0;
            double interest = 0;
            foreach (var bankAccount in bankAccounts)
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
        private static (string SelectionPromptTitle, List<string> AccountInformationList, decimal TotalSumOnAccounts) GetBankAccountInfo(List<BankAccount> bankAccounts)
        {
            var accountInfoList = new List<string>();
            int maxName = 12, maxNumber = 10, maxBalance = 7, maxCurrency = 3, maxType = 8, maxInterest = 8;
            decimal totalSumOnAccounts = 0;
            foreach (var account in bankAccounts)
            {
                totalSumOnAccounts += account.GetBalance();
                var info = account.GetAccountInformation();
                maxName = info.Name.Length > maxName ? info.Name.Length : maxName;
                maxNumber = info.Number.Length > maxNumber ? info.Number.Length : maxNumber;
                maxBalance = info.Balance.Length > maxBalance ? info.Balance.Length : maxBalance;
            }

            int accountNamePadding = ((maxName - "Account Name".Length) / 2) - ("Account Name".Length / 2);
            int accountNumberPadding = ("Number".Length / 2) - ((maxNumber - "Number".Length) / 2);
            string questionTitel = string.Format("  {0," + accountNamePadding + "}[yellow bold]{1, " + -(maxName - accountNamePadding) + "}[/] - {0, " + (accountNumberPadding + accountNumberPadding % 2) + "}[red bold]{2, " + -(maxNumber - accountNumberPadding - accountNumberPadding % 2) + "}[/] - [green bold]{3, " + maxBalance + "}[/] - [blue bold]{4, " + maxCurrency + "}[/] - [orange1 bold]{5, " + -maxType + "}[/] - [cyan1 bold]{6, " + maxInterest + "}[/]", "", "Account Name", "Number", "Balance", "Cur", "Type", "Interest");

            foreach (var account in bankAccounts)
            {
                var info = account.GetAccountInformation();
                string text = string.Format("[yellow bold]{0, " + -maxName + "}[/] - [red bold]{1, " + maxNumber + "}[/] - [green bold]{2, " + maxBalance + "}[/] - [blue bold]{3, " + maxCurrency + "}[/] - [orange1 bold]{4, " + -maxType + "}[/] - [cyan1 bold]{5, " + maxInterest + "}[/]", info.Name, info.Number, info.Balance, info.Currency, info.Type, info.Interest);
                accountInfoList.Add(text);
            }

            return (questionTitel, accountInfoList, totalSumOnAccounts);
        }

        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum) MakeLoanMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider($"Loan Menu");

            var interest = DecideInterestRate(bankAccounts, true);
            AnsiConsole.MarkupLine($"[green]You will get an interest rate of: {interest:p}[/]");
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
            accountChoices.AccountInformationList.Add("Back");
            var selectedAccountChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .PageSize(20)
                .Title("What account would like to deposit the loan to?\n" + accountChoices.SelectionPromptTitle)
                .AddChoices(accountChoices.AccountInformationList)
             );

            string pattern = @"\[red bold\](\d+)\[/\]";
            Regex regex = new Regex(pattern);

            int chosenAccountNumber = GetSingleMatch(pattern, selectedAccountChoice.ToString());

            var selectedAccount = bankAccounts.FirstOrDefault(account =>
            {
                if (account.GetAccountNumber() == chosenAccountNumber)
                {
                    return true;
                }
                return false;
            });

            if (selectedAccount == null)
            {
                return default;
            }

            var info = selectedAccount.GetAccountInformation();
            WriteTransactionInformation(info);

            while (true)
            {
                decimal loanAmount = AnsiConsole.Ask<decimal>($"[purple]How Much Would You Like To Loan?[/] [gold1](Maximum: {accountChoices.TotalSumOnAccounts * 5})[/]");

                if (loanAmount >= 0 || loanAmount <= accountChoices.TotalSumOnAccounts * 5)
                {
                    AnsiConsole.MarkupLine($"[green]{loanAmount:c} {info.Currency}[/] [purple]has been added to {info.Name}[/]");

                    string stringChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .PageSize(3)
                            .AddChoices(new[]
                            {
                                    "Ok"
                            }
                        ));

                    if (stringChoice == "Ok")
                    {
                        CurrencyType currencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), info.Currency ?? CurrencyType.SEK.ToString());
                        return (selectedAccount, currencyTypeParsed, DateTime.UtcNow, loanAmount);
                    }
                    else
                    {
                        return default;
                    }
                }

                AnsiConsole.MarkupLine($"[red]Please enter a valid amount between 0 and {accountChoices.TotalSumOnAccounts * 5}.[/]");
            }
        }



        public static UserChoice AdminUpdateCurrencyOption()
        {
            Console.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(3)
                    .Title("Select a conversion")
                    .MoreChoicesText("Scroll down for more options")
                    .AddChoices(
                        "From File",
                        "From The Intrawebbs",
                        "Back"
                        )
            );

            return ConvertStringToUserChoice(choice);
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
