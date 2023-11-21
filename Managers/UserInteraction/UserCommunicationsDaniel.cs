using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum) MakeDepositMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider($"Deposit Menu");
            var accountChoices = GetBankAccountInfo(bankAccounts);
            accountChoices.AccountInformationList.Add("Back");
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
    }
}