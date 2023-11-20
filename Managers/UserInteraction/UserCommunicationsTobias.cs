using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers.Logs;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static void ShowLogs(List<Log> logs)
        {
            WriteDivider("Display Logs");
            foreach (var log in logs)
            {
                Console.WriteLine(log);
            }
            Console.ReadLine();
        }

        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum) MakeLoanMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider($"Withdrawal Menu");

            var accountChoices = new List<string>();
            // (string Type, string Name, string Number, string Balance, string Currency, string Interest) info = new();

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
                    .Title("Select an Account to Withdraw from")
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

                if (selectedAccount is Checking)
                {
                    var checkingAccount = selectedAccount as Checking;
                    var info = checkingAccount.GetAccountInformation();
                    accountName = info.Name;
                    balance = info.Balance;
                    currencyType = info.Currency;
                }
                else if (selectedAccount is Savings)
                {
                    var savingsAccount = selectedAccount as Savings;
                    var info = savingsAccount.GetAccountInformation();
                    accountName = info.Name;
                    balance = info.Balance;
                    currencyType = info.Currency;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Selected account type is not supported for withdrawal.[/]");
                    return default;
                }

                AnsiConsole.MarkupLine($"[purple]Account Name:[/] [green]{accountName}[/]");
                AnsiConsole.MarkupLine($"[purple]Balance:[/] [blue]{balance}[/] [gold1]{currencyType}[/]");
                AnsiConsole.MarkupLine($"[purple]Withdraw from[/]  [green]{accountName}[/]");

                decimal withdrawalAmount;
                decimal maxWithdrawal;

                if (!decimal.TryParse(balance, out maxWithdrawal))
                {
                    AnsiConsole.MarkupLine("[red]Invalid balance value.[/]");
                }

                while (true)
                {
                    string input = AnsiConsole.Ask<string>($"[purple]How Much Would You Like To Withdraw?[/] [gold1](Maximum: {maxWithdrawal})[/]");

                    if (!decimal.TryParse(input, out withdrawalAmount) || withdrawalAmount <= 0 || withdrawalAmount > maxWithdrawal)
                    {
                        AnsiConsole.MarkupLine($"[red]Please enter a valid withdrawal amount between 0 and {maxWithdrawal}.[/]");
                    }
                    else
                    {
                        decimal currentBalance = decimal.Parse(balance);
                        currentBalance -= withdrawalAmount;
                        balance = currentBalance.ToString();

                        AnsiConsole.MarkupLine($"[green]{withdrawalAmount:c} {currencyType}[/] [purple]has been withdrawn from {accountName}[/]");

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
                            return (selectedAccount, currencyTypeParsed, DateTime.UtcNow, withdrawalAmount);
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
                AnsiConsole.MarkupLine("[red]Selected account type is not supported for withdrawal.[/]");
                return default;
            }
        }
    }
}
