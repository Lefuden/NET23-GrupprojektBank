using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {

        public static (BankAccount SourceBankAccount, BankAccount DestinationBankAccount, CurrencyType SourceCurrencyType, CurrencyType DestinationCurrencyType, DateTime DateAndTime, decimal Sum) MakeTransferMenu(List<BankAccount> bankAccounts, List<BankAccount> allBankAccounts)
        {
            WriteDivider("Transfer Menu");

            var transferDirection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Choose were to Transfer")
                .PageSize(3)
                .AddChoices(new[] { "Transfer between personal accounts", "Transfer to other account" })
            );

            if (transferDirection == "Transfer between personal accounts")
            {
                var accountChoices = GetBankAccountInfo(bankAccounts);
                var selectedAccountChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Select account to transfer from")
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
                        while (true)
                        {
                            decimal transferAmount = AnsiConsole.Ask<decimal>($"Enter the amount to transfer (Maximum: {maxTransferAmount}):");

                            if (transferAmount < 0 || transferAmount > maxTransferAmount)
                            {
                                AnsiConsole.MarkupLine($"[red]Please enter a valid transfer amount between 0 and {maxTransferAmount}.[/]");
                                continue;
                            }
                            else
                            {
                                var destinationAccountInfo = destinationAccount.GetAccountInformation();
                                decimal sourceCurrentBalance = decimal.Parse(sourceBalance);
                                sourceCurrentBalance -= transferAmount;
                                sourceBalance = sourceCurrentBalance.ToString();

                                CurrencyType sourceCurrencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), sourceCurrencyType ?? CurrencyType.SEK.ToString());
                                CurrencyType destinationCurrencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), destinationAccountInfo.Currency ?? CurrencyType.SEK.ToString());

                                var confirmation = AskUserYesOrNo($"Do you want to deposit this {transferAmount:c} to {destinationAccount.GetAccountInformation().Name}?");

                                if (confirmation)
                                {
                                    AnsiConsole.MarkupLine($"[green]{transferAmount:c} {sourceCurrencyType}[/] [purple]has been transferred from {sourceAccountName} to {destinationAccount.GetAccountInformation().Name}[/]");
                                    return (selectedAccount, destinationAccount, sourceCurrencyTypeParsed, destinationCurrencyTypeParsed, DateTime.UtcNow, transferAmount);
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine("[yellow]Transfer canceled. Please re-enter the amount to transfer.[/]");
                                    continue;
                                }
                            }
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Destination account not found or unsupported for transfer.[/]");
                        return default;
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Selected account not found.[/]");
                    return default;
                }
            }
            else if (transferDirection == "Transfer to other account")
            {
                var accountChoices = GetBankAccountInfo(bankAccounts);
                var selectedAccountChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Select Account to Transfer From")
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

                   
                    var transferAccountsExcludingSelected = allBankAccounts.Where(acc => acc.GetAccountNumber() != chosenAccountNumber).ToList();
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
                        while (true)
                        {
                            decimal transferAmount = AnsiConsole.Ask<decimal>($"Enter the amount to transfer (Maximum: {maxTransferAmount}):");

                            if (transferAmount < 0 || transferAmount > maxTransferAmount)
                            {
                                AnsiConsole.MarkupLine($"[red]Please enter a valid transfer amount between 0 and {maxTransferAmount}.[/]");
                                continue;
                            }
                            else
                            {
                                var destinationAccountInfo = destinationAccount.GetAccountInformation();
                                decimal sourceCurrentBalance = decimal.Parse(sourceBalance);
                                sourceCurrentBalance -= transferAmount;
                                sourceBalance = sourceCurrentBalance.ToString();
                                CurrencyType sourceCurrencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), sourceCurrencyType ?? CurrencyType.SEK.ToString());
                                CurrencyType destinationCurrencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), destinationAccountInfo.Currency ?? CurrencyType.SEK.ToString());

                                var confirmation = AskUserYesOrNo($"Do you want to deposit this {transferAmount:c} to {destinationAccount.GetAccountInformation().Name}? (Yes/No): ");

                                if (confirmation)
                                {
                                    AnsiConsole.MarkupLine($"[green]{transferAmount:c} {sourceCurrencyType}[/] [purple]has been transferred from {sourceAccountName} to {destinationAccount.GetAccountInformation().Name}[/]");
                                    return (selectedAccount, destinationAccount, sourceCurrencyTypeParsed, destinationCurrencyTypeParsed, DateTime.UtcNow, transferAmount);
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine("[yellow]Transfer canceled. Please re-enter the amount to transfer.[/]");
                                }
                            }
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Destination account not found or unsupported for transfer.[/]");
                        return default;
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Selected account not found.[/]");
                    return default;
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid transfer direction selected.[/]");
                return default;
            }

            return default;
        }
       
    }

}
