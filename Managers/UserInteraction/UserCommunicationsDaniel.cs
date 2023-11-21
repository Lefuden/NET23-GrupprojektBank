using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum) MakeDepositMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider($"Deposit Menu");
            var accountChoices = new List<string>();

            foreach (var account in bankAccounts)
            {
                if (account is Checking checkingAccount)
                {
                    var info = checkingAccount.GetAccountInformation();
                    accountChoices.Add($"[orange1 bold]{info.Type}[/] - [yellow bold]{info.Name}[/] - [red bold]{info.Number}[/] - [green bold]{info.Balance}[/] - [blue bold]{info.Currency}[/]");
                }
                else if (account is Savings savingsAccount)
                {
                    var info = savingsAccount.GetAccountInformation();
                    accountChoices.Add($"[orange1 bold]{info.Type}[/] - [yellow bold]{info.Name}[/] - [red bold]{info.Number}[/] - [green bold]{info.Balance}[/] - [blue bold]{info.Currency}[/] - [cyan1 bold]{info.Interest}[/]");
                }
            }

            accountChoices.Add("Exit");
            var selectedAccountChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(5)
                    .Title("Select an account to deposit to")
                    .MoreChoicesText("Scroll down for more options")
                    .AddChoices(accountChoices)
            );

            string pattern = @"\[red bold\](\d+)\[/\]";
            Regex regex = new Regex(pattern);

            int choosenAccountNumber = GetSingleMatch(pattern, selectedAccountChoice);
            var selectedAccount = bankAccounts.FirstOrDefault(account =>
            {
                if (account.GetAccountNumber() == choosenAccountNumber)
                {
                    return true;
                }
                return false;
            });

            if (selectedAccount != null)
            {
                string accountName;
                string balance;
                string currencyType;

                switch (selectedAccount)
                {
                    case Checking checkingAccount:
                    {
                        var info = checkingAccount.GetAccountInformation();
                        accountName = info.Name;
                        balance = info.Balance;
                        currencyType = info.Currency;
                        break;
                    }
                    case Savings savingsAccount:
                    {
                        var info = savingsAccount.GetAccountInformation();
                        accountName = info.Name;
                        balance = info.Balance;
                        currencyType = info.Currency;
                        break;
                    }
                    default:
                        AnsiConsole.MarkupLine("[red]Selected account type is not supported for deposits.[/]");
                        return default;
                }

                AnsiConsole.MarkupLine($"[purple]Account name:[/] [green]{accountName}[/]");
                AnsiConsole.MarkupLine($"[purple]Balance:[/] [blue]{balance}[/] [gold1]{currencyType}[/]");
                AnsiConsole.MarkupLine($"[purple]Deposit to:[/]  [green]{accountName}[/]");

                while (true)
                {
                    string input = AnsiConsole.Ask<string>($"[purple]How much would you like to deposit?[/]");

                    if (!decimal.TryParse(input, out decimal depositAmount) || depositAmount <= 0)
                    {
                        AnsiConsole.MarkupLine($"[red]Please enter a valid desposit amount[/]");
                    }
                    else
                    {
                        decimal currentBalance = decimal.Parse(balance);
                        currentBalance += depositAmount;
                        balance = currentBalance.ToString();

                        AnsiConsole.MarkupLine($"[green]{depositAmount:c} {currencyType}[/] [purple]has been deposited to {accountName}[/]");

                        string stringChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .PageSize(3)
                                .AddChoices(new[]
                                {
                            "Back",
                            "Exit"
                                }
                            ));

                        if (stringChoice == "Back")
                        {
                            CurrencyType currencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), currencyType ?? CurrencyType.SEK.ToString());
                            return (selectedAccount, currencyTypeParsed, DateTime.UtcNow, depositAmount);
                        }
                        else if (stringChoice == "Exit")
                        {
                            Environment.Exit(0);
                        }
                    }
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Selected account type is not supported for deposit.[/]");
                return default;
            }
        }
    }
}
