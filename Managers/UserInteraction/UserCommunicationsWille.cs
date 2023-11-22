using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static (BankAccount SourceBankAccount, BankAccount DestinationBankAccount, CurrencyType SourceCurrencyType, CurrencyType DestinationCurrencyType, DateTime DateAndTime, decimal Sum) MakeTransferMenu(List<BankAccount> bankAccounts, List<BankAccount> bankAccounts2)
        {
            WriteDivider("Transfer Menu");

            var accountChoices = GetBankAccountInfo(bankAccounts);
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
                string sourceAccountName = sourceAccountInfo.BankAccountName;
                string sourceBalance = sourceAccountInfo.Balance;
                string sourceCurrencyType = sourceAccountInfo.CurrencyType;
                decimal maxTransferAmount = decimal.Parse(sourceBalance);

                AnsiConsole.MarkupLine($"[purple]Account Name:[/] [green]{sourceAccountName}[/]");
                AnsiConsole.MarkupLine($"[purple]Balance:[/] [blue]{sourceBalance}[/] [gold1]{sourceCurrencyType}[/]");
                AnsiConsole.MarkupLine($"[purple]Transfer from[/]  [green]{sourceAccountName}[/]");

                var transferToOwnAccount = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Transfer to Own Account or Other?")
                    .PageSize(3)
                    .AddChoices(new[] { "Own Account", "Other" })
                );

                if (transferToOwnAccount == "Own Account")
                {
                    var transferAccountsExcludingSelected = bankAccounts.Where(acc => acc.GetAccountNumber() != chosenAccountNumber).ToList();

                    var destinationAccountChoice = GetBankAccountInfo(transferAccountsExcludingSelected);

                    var inputDestinationAccount = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Select the Destination Account")
                        .AddChoices(destinationAccountChoice.AccountInformationList)
                    );

                    int destinationAccountNumber = GetSingleMatch(pattern, inputDestinationAccount);

                    var destinationAccount = transferAccountsExcludingSelected.FirstOrDefault(account =>
                    {
                        if (account.GetAccountNumber() == destinationAccountNumber)
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

                                AnsiConsole.MarkupLine($"[green]{transferAmount:c} {sourceCurrencyType}[/] [purple]has been transferred from {sourceAccountName} to {destinationAccount.GetAccountInformation().BankAccountName}[/]");

                                CurrencyType sourceCurrencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), sourceCurrencyType ?? CurrencyType.SEK.ToString());
                                return (selectedAccount, selectedAccount, sourceCurrencyTypeParsed, sourceCurrencyTypeParsed, DateTime.UtcNow, transferAmount);
                            }
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Destination account not found or unsupported for transfer.[/]");
                    }
                }
                else if (transferToOwnAccount == "Other")
                {
                    var transferAccountsExcludingSelected = bankAccounts.Where(acc => acc.GetAccountNumber() != chosenAccountNumber).ToList();
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

                    var destinationAccount = transferAccountsExcludingSelected.FirstOrDefault(account =>
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

                                AnsiConsole.MarkupLine($"[green]{transferAmount:c} {sourceCurrencyType}[/] [purple]has been transferred from {sourceAccountName} to {destinationAccount.GetAccountInformation().BankAccountName}[/]");

                                CurrencyType sourceCurrencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), sourceCurrencyType ?? CurrencyType.SEK.ToString());
                                return (selectedAccount, selectedAccount, sourceCurrencyTypeParsed, sourceCurrencyTypeParsed, DateTime.UtcNow, transferAmount);
                            }
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Destination account not found or unsupported for transfer.[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Selected account type is not supported for transfer.[/]");
                }

                return default;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Selected account not found.[/]");
                return default;
            }
        }
    }

}
