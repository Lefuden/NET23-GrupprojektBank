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
            var accountChoices = GetBankAccountInfo(bankAccounts);
            accountChoices.AccountInformationList.Add("Back");
            var selectedAccountChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(15)
                    .Title($"Select an account to deposit to {accountChoices.SelectionPromtTitel}")
                    .AddChoices(accountChoices.AccountInformationList)
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

            if (selectedAccount == null)
            {
                AnsiConsole.MarkupLine("[red]Selected account type is not supported for deposits.[/]");
                return default;
            }

            var info = selectedAccount.GetAccountInformation();
            var accountName = info.Name;
            var balance = info.Balance;
            var currencyType = info.Currency;

            AnsiConsole.MarkupLine($"[purple]Account name:[/] [green]{accountName}[/]");
            AnsiConsole.MarkupLine($"[purple]Balance:[/] [blue]{balance}[/] [gold1]{currencyType}[/]");
            AnsiConsole.MarkupLine($"[purple]Deposit to:[/]  [green]{accountName}[/]");

            while (true)
            {
                string input = AnsiConsole.Ask<string>($"[purple]How much would you like to deposit?[/]");

                if (!decimal.TryParse(input, out decimal depositAmount) || depositAmount <= 0)
                {
                    AnsiConsole.MarkupLine($"[red]Please enter a valid deposit amount[/]");
                }
                else
                {
                    decimal currentBalance = decimal.Parse(balance);
                    currentBalance += depositAmount;
                    balance = currentBalance.ToString();

                    AnsiConsole.MarkupLine(
                        $"[green]{depositAmount:c} {currencyType}[/] [purple]has been deposited to {accountName}[/]");

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
                        CurrencyType currencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType),
                            currencyType ?? CurrencyType.SEK.ToString());
                        return (selectedAccount, currencyTypeParsed, DateTime.UtcNow, depositAmount);
                    }
                    else
                    {
                        return default;
                    }
                }
            }
        }
    }
}