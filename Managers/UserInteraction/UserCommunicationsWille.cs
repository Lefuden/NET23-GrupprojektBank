using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum) MakeTransferMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider("Transfer Menu");

            var accountChoices = GetBankAccountInfo(bankAccounts);
            accountChoices.AccountInformationList.Add("Back");
            var selectedAccountChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"What Account Would you like to Transfer from\n{accountChoices.SelectionPromptTitle}")
                .AddChoices(accountChoices.AccountInformationList)
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

                var transferToOwnAccount = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Transfer to Own Account or Other?")
                    .PageSize(2)
                    .AddChoices(new[] { "Own Account", "Other" })
                );

                List<BankAccount> transferAccounts;

                if (transferToOwnAccount == "Own Account")
                {
                    transferAccounts = bankAccounts;
                }
                else
                {
                    transferAccounts = bankAccounts.Where(acc => acc.GetAccountNumber() != chosenAccountNumber).ToList();
                    string destinationAccountNumber;

                    while (true)
                    {
                        destinationAccountNumber = AnsiConsole.Ask<string>("Enter the destination account number (10 letters long):");

                        if (destinationAccountNumber.Length != 10)
                        {
                            AnsiConsole.MarkupLine("[red]Destination account number must be exactly 10 letters long.[/]");
                            continue;
                        }
                        break;
                    }

                    var destinationAccount = transferAccounts.FirstOrDefault(account =>
                    {
                        if (account.GetAccountNumber() == int.Parse(destinationAccountNumber))
                        {
                            return true;
                        }
                        return false;
                    });

                    if (destinationAccount != null)
                    {
                        decimal transferAmount;

                        while (true)
                        {
                            string inputAmount = AnsiConsole.Ask<string>($"Enter the amount to transfer (Maximum: {maxTransferAmount}):");

                            if (!decimal.TryParse(inputAmount, out transferAmount) || transferAmount < 0 || transferAmount > maxTransferAmount)
                            {
                                AnsiConsole.MarkupLine($"[red]Please enter a valid transfer amount between 0 and {maxTransferAmount}.[/]");
                                continue;
                            }
                            else
                            {
                                decimal sourceCurrentBalance = decimal.Parse(sourceBalance);
                                sourceCurrentBalance -= transferAmount;
                                sourceBalance = sourceCurrentBalance.ToString();

                                AnsiConsole.MarkupLine($"[green]{transferAmount:c} {sourceCurrencyType}[/] [purple]has been transferred from {sourceAccountName} to {destinationAccount.GetAccountInformation().Name}[/]");

                                CurrencyType sourceCurrencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), sourceCurrencyType ?? CurrencyType.SEK.ToString());
                                return (selectedAccount, sourceCurrencyTypeParsed, DateTime.UtcNow, transferAmount);
                            }
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Destination account not found or unsupported for transfer.[/]");
                    }
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Selected account type is not supported for transfer.[/]");
            }

            return default;
        }
    }
}
