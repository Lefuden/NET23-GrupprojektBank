using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers.Transactions;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static void ViewBankAccounts(List<BankAccount> bankAccounts)
        {
            if (bankAccounts.Count == 0)
            {
                AnsiConsole.MarkupLine($"[{BankAccountColors["Warning"]}]You have no accounts in the bank![/]");
                FakeBackChoice("Back");
                return;
            }
            var table = GetBankAccountsAsTable(bankAccounts);
            table.Title($"[{BankAccountColors["Title"]}]Bank Accounts[/]");
            table.BorderColor(BorderColor);

            AnsiConsole.Write(table);
            FakeBackChoice("Back");
        }

        public static BankAccount GetBankAccountFromUserPrompt(List<BankAccount> bankAccounts, TransactionType transactionType, bool isTransferDestinationAccount = false)
        {
            AnsiConsole.Clear();
            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Choose Account");
            var accountChoices = GetBankAccountInfo(bankAccounts);

            string titleContent = transactionType switch
            {
                TransactionType.Loan => "Select an Account to apply Loan to",
                TransactionType.Transfer => "Select an Account to Transfer from",
                TransactionType.Deposit => "Select an Account to Deposit to",
                TransactionType.Withdrawal => "Select an Account to Withdraw from",
                _ => ""
            };
            titleContent = isTransferDestinationAccount ? "Select an Account to Transfer to" : titleContent;


            var selectedAccountChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"[{BankAccountColors["Title"]}]{titleContent}[/]\n\n{accountChoices.SelectionPromptTitle}")
                .PageSize(25)
                .HighlightStyle(HHStyle)
                .AddChoices(accountChoices.AccountInformationList)
            );

            int chosenAccountNumber = GetSingleMatch(pattern, selectedAccountChoice.ToString());

            var selectedAccount = bankAccounts.FirstOrDefault(account => (account.GetAccountNumber() == chosenAccountNumber));
            AnsiConsole.Clear();
            if (selectedAccount == null)
            {
                return default;
            }
            return selectedAccount;
        }
        public enum TransferTo
        {
            Personal,
            Other,
            Invalid
        }
        public static TransferTo AskUserWhereToTransferTo()
        {
            var transferDirection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"[{BankAccountColors["Title"]}]Choose were to Transfer[/]")
                .PageSize(3)
                .AddChoices(new[] {
                    $"[{BankAccountColors["Choice"]}]Transfer between personal accounts[/]",
                    $"[{BankAccountColors["Choice"]}]Transfer to other account[/]",
                    $"[{BankAccountColors["Back"]}]Back[/]"

                }));
            string choice = Markup.Remove(transferDirection);
            AnsiConsole.Clear();
            return choice switch
            {
                "Transfer between personal accounts" => TransferTo.Personal,
                "Transfer to other account" => TransferTo.Other,
                _ => TransferTo.Invalid
            };
        }
        private static (bool FoundAccount, BankAccount? DestinationAccount) FindDestinationTransferAccountWithAccountNumber(List<BankAccount> transferAccountListExcludingSelected)
        {
            int destinationAccountNumber = AnsiConsole.Prompt(
                    new TextPrompt<int>($"{AdminColors["Title"]}Enter the destination account number (10 letters long)[/]:")
                    .PromptStyle("red")
                    .Validate(destNumber =>
                        destNumber.ToString().Length == 10
                        ? ValidationResult.Success()
                        : destNumber.ToString().Length < 10
                        ? ValidationResult.Error($"{MenuColors["Warning"]}Account number too short! An account number is always 10 numbers[/]")
                        : destNumber.ToString().Length > 10
                        ? ValidationResult.Error($"{MenuColors["Warning"]}Account number too long! An account number is always 10 numbers[/]")
                        : ValidationResult.Error($"{MenuColors["Warning"]}Unkown Error")
                    ));
            BankAccount destinationAccount = transferAccountListExcludingSelected.FirstOrDefault(account => (account.GetAccountNumber() == destinationAccountNumber));
            bool foundAccount = destinationAccount != null;
            return (foundAccount, destinationAccount);
        }



        public static CurrencyType ChooseCurrencyType()
        {
            WriteDivider($"{BankAccountColors["DividerText"]}", $"{BankAccountColors["DividerLine"]}", "Choose Currency Type");
            var currencyChoices = new SelectionPrompt<string>()
                .Title($"[{BankAccountColors["Title"]}]Choose currency type[/]")
                .PageSize(3);

            foreach (CurrencyType type in CurrencyType.GetValuesAsUnderlyingType<CurrencyType>())
            {
                if (type is not CurrencyType.INVALID)
                {
                    currencyChoices.AddChoice($"[{BankAccountColors["Title"]}]{type}[/]");
                }
            }

            var choice = AnsiConsole.Prompt(currencyChoices);
            var cleanedChoice = Markup.Remove(choice);
            Console.Clear();
            return Enum.TryParse(cleanedChoice, out CurrencyType selectedType) ? selectedType : CurrencyType.SEK;
        }


        public static decimal GetMaximumLoanAllowedWithInterestCalculated(List<BankAccount> bankAccounts)
        {

            decimal currentLoanWithInterestRateCalculated = 0;
            foreach (var account in bankAccounts)
            {
                currentLoanWithInterestRateCalculated += account.GetLoanAmount() * (decimal)account.GetLoanInterestRate() * 100;
            }

            return currentLoanWithInterestRateCalculated;
        }
    }
}
