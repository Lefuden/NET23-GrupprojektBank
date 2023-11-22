using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum) MakeWithdrawalMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider($"Withdrawal Menu");

            var accountChoices = GetBankAccountInfo(bankAccounts);


            var selectedAccountChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(20)
                    .Title("Select an Account to Withdraw from\n" + accountChoices.SelectionPromptTitle)
                    .AddChoices(accountChoices.AccountInformationList)
                );

            int chosenAccountNumber = GetSingleMatch(pattern, selectedAccountChoice);

            var selectedAccount = bankAccounts.FirstOrDefault(account => account.GetAccountNumber() == chosenAccountNumber);

            if (selectedAccount == null)
            {
                return default;
            }

            var info = selectedAccount.GetAccountInformation();
            WriteTransactionInformation(info);

            if (decimal.TryParse(info.Balance, out decimal accountBalance) is not true)
            {
                return default;
            }


            decimal withdrawalAmount = AnsiConsole.Ask<decimal>($"[purple]How Much Would You Like To Withdraw?[/] [gold1](Maximum: {accountBalance})[/]");

            while (withdrawalAmount < 0 || withdrawalAmount > accountBalance)
            {
                AnsiConsole.MarkupLine($"[red]Invalid withdrawal amount. Please enter a valid amount between 0 and {accountBalance}.[/]");
                withdrawalAmount = AnsiConsole.Ask<decimal>($"[purple]How Much Would You Like To Withdraw?[/] [gold1](Maximum: {accountBalance})[/]");
            }

            AnsiConsole.MarkupLine($"[green]{withdrawalAmount:c} {info.Currency}[/] [purple]has been withdrawn from {info.Name}[/]\n");
            FakeBackChoice("Ok");
            CurrencyType currencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), info.Currency ?? CurrencyType.SEK.ToString());
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
