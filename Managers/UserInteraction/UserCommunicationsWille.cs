using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {//10 bokstäver på konto, Gör om till grid, minska från ett konto till ett annat kan inte vara mer än vad man har på kontot och de kan inte vara mindre än vad man har.
        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum) MakeTransferMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider("Transfer Menu");

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
                    .Title("Select an Account to Transfer from")
                    .MoreChoicesText("Scroll down for more options")
                    .AddChoices(accountChoices)
            );

            int chosenAccountNumber = GetSingleMatch(pattern, selectedAccountChoice);

            var selectedAccount = bankAccounts.FirstOrDefault(account =>
            {
                if (account.GetAccountNumber() == chosenAccountNumber)
                {
                    return true;
                }
                return false;
            });

            if (selectedAccount != null)
            {
                var sourceAccountInfo = selectedAccount.GetAccountInformation();
                string sourceAccountName = sourceAccountInfo.Name;
                string sourceBalance = sourceAccountInfo.Balance;
                string sourceCurrencyType = sourceAccountInfo.Currency;
                decimal maxTransferAmount = decimal.Parse(sourceBalance);

                AnsiConsole.MarkupLine($"[purple]Account Name:[/] [green]{sourceAccountName}[/]");
                AnsiConsole.MarkupLine($"[purple]Balance:[/] [blue]{sourceBalance}[/] [gold1]{sourceCurrencyType}[/]");
                AnsiConsole.MarkupLine($"[purple]Transfer from[/]  [green]{sourceAccountName}[/]");

                while (true)
                {
                    string inputAccountNumber = AnsiConsole.Ask<string>("Enter the destination account number (10 letters long):");

                    if (inputAccountNumber.Length != 10)
                    {
                        AnsiConsole.MarkupLine("[red]Destination account number must be exactly 10 letters long.[/]");
                        continue;
                    }

                    int destinationAccountNumber;

                    if (!int.TryParse(inputAccountNumber, out destinationAccountNumber))
                    {
                        AnsiConsole.MarkupLine("[red]Invalid destination account number.[/]");
                        continue;
                    }

                    var destinationAccount = bankAccounts.FirstOrDefault(account => account.GetAccountNumber() == destinationAccountNumber);

                    if (destinationAccount == null)
                    {
                        AnsiConsole.MarkupLine("[red]Destination account not found.[/]");
                        continue;
                    }

                    string inputAmount = AnsiConsole.Ask<string>($"Enter the amount to transfer (Maximum: {maxTransferAmount}):");

                    decimal transferAmount;

                    if (!decimal.TryParse(inputAmount, out transferAmount) || transferAmount <= 0 || transferAmount > maxTransferAmount)
                    {
                        AnsiConsole.MarkupLine($"[red]Please enter a valid transfer amount between 0 and {maxTransferAmount}.[/]");
                        continue;
                    }

                    decimal sourceCurrentBalance = decimal.Parse(sourceBalance);
                    sourceCurrentBalance -= transferAmount;
                    sourceBalance = sourceCurrentBalance.ToString();

                    AnsiConsole.MarkupLine($"[green]{transferAmount:c} {sourceCurrencyType}[/] [purple]has been transferred from {sourceAccountName} to {destinationAccount.GetAccountInformation().Name}[/]");

                    CurrencyType sourceCurrencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), sourceCurrencyType ?? CurrencyType.SEK.ToString());
                    return (selectedAccount, sourceCurrencyTypeParsed, DateTime.UtcNow, transferAmount);
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Selected account type is not supported for transfer.[/]");
                return default;
            }
        }
    }
}
