using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
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
