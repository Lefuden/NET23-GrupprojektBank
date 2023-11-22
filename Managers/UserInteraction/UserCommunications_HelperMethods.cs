using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Managers.Logic;
using NET23_GrupprojektBank.Managers.Logs;
using Spectre.Console;
using Color = Spectre.Console.Color;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {

        private static readonly string _greenColour = "palegreen3";
        private static readonly string _redWarning = "red3";
        private static readonly string _redExit = "indianred_1";
        private static readonly string _purpleTitle = "mediumpurple1";
        private static readonly string _purpleChoice = "mediumpurple2_1";
        private static readonly string _purpleHighlight = "mediumpurple2"; // inside our HHStyle
        private static readonly string _goldDividerText = "gold1";
        private static readonly string _goldDividerLine = "darkgoldenrod";
        private static readonly string _purpleBack = "#AF5F8C]";
        private static readonly string _greenAccountBalance = "springgreen3_1";
        private static readonly string _blueAccountNumber = "skyblue3";
        private static readonly string _greenAccountInfo = "darkolivegreen3_2";
        public static readonly Color BorderColor = Color.MediumPurple2_1;
        private static readonly Style HHStyle = new Style(Color.MediumPurple2, null, Decoration.Underline);
        private static string pattern { get; set; } = @$"\[{_blueAccountNumber} bold\]\s*([\d,]+)\[/\]";
        private static readonly Dictionary<string, string> UserColors = new()
        {
            {"Title",           $"[{_purpleTitle}]"},
            {"Choice",          $"[{_purpleChoice}]"},
            {"Exit",            $"[{_redExit}]"},
            {"Back",            $"[{_purpleBack}]"},
            {"DividerText",     $"[{_goldDividerText}]"},
            {"DividerLine",     $"[{_goldDividerLine}]"},
            {"Warning",         $"[{_redWarning}]"},
            {"Highlight",       $"[{_purpleHighlight}]"}
        };
        private static readonly Dictionary<string, string> AdminColors = new()
        {
            {"Title",           $"[{_purpleTitle}]"},
            {"Choice",          $"[{_purpleChoice}]"},
            {"Exit",            $"[{_redExit}]"},
            {"Back",            $"[{_purpleBack}]"},
            {"DividerText",     $"[{_goldDividerText}]"},
            {"DividerLine",     $"[{_purpleHighlight}]"},
            {"Warning",         $"[{_redWarning}]"}
        };
        private static readonly Dictionary<string, string> MenuColors = new()
        {
            {"Title",           $"[{_purpleTitle}]"},
            {"Choice",          $"[{_purpleChoice}]"},
            {"Exit",            $"[{_redExit}]"},
            {"Back",            $"[{_purpleBack}]"},
            {"DividerText",     $"[{_goldDividerText}]"},
            {"DividerLine",     $"[{_goldDividerLine}]"},
            {"Warning",         $"[{_redWarning}]" }
        };
        private static readonly Dictionary<string, string> BankAccountColors = new()
        {
            {"Title",           $"{_purpleTitle}"},
            {"Choice",          $"{_purpleChoice}"},
            {"Exit",            $"{_redExit}"},
            {"Back",            $"{_purpleBack}"},
            {"DividerText",     $"{_goldDividerText}"},
            {"DividerLine",     $"{_goldDividerLine}" },
            {"AccountName",     $"{_greenAccountInfo}"},
            {"Number",          $"{_blueAccountNumber}"},
            {"Balance",         $"{_greenAccountBalance}"},
            {"BalanceGreen",    $"{_greenColour}"},
            {"CurrencyType",    $"{_greenAccountInfo}"},
            {"AccountType",     $"{_greenAccountInfo}"},
            {"Interest",        $"{_greenAccountInfo}"},
            {"Warning",         $"{_redWarning}"},
            {"Highlight",       $"{_purpleHighlight}" }
        };
        private static readonly Dictionary<string, string> LogsColors = new()
        {
            {"Title",           $"[{_purpleTitle}]"},
            {"Choice",          $"[{_purpleChoice}]"},
            {"Exit",            $"[{_redExit}]"},
            {"Back",            $"[{_purpleBack}]"},
            {"DividerText",     $"[{_goldDividerText}]"},
            {"DividerLine",     $"[{_goldDividerLine}]"},
            {"Border",          $"[{_purpleChoice}]"},
            {"Message",         $"[{_purpleTitle}]"},
            {"DateAndTime",     $"[{_purpleTitle}]"},
            {"LogId",           $"[{_purpleHighlight}]"},
            {"Warning",         $"[{_redWarning}]" }
        };

        private static UserChoice ConvertStringToUserChoice(string input)
        {

            var cleanInput = Markup.Remove(input);
            return cleanInput switch
            {

                "Login" => UserChoice.Login,
                "Exit" => UserChoice.Exit,
                "View Account Balance" => UserChoice.ViewBalance,//inte sub
                "Transfer" => UserChoice.MakeTransfer,//inte sub
                "Deposit" => UserChoice.MakeDeposit,//inte sub
                "Withdraw" => UserChoice.MakeWithdrawal,//inte sub
                "Loan" => UserChoice.MakeLoan,//inte sub
                "Create Bank Account" => UserChoice.CreateBankAccount,//sub
                "Create Checkings Account" => UserChoice.CreateChecking,
                "Create Savings Account" => UserChoice.CreateSavings,
                "Create Customer Account" => UserChoice.CreateCustomer,//ingen sub
                "Create Admin Account" => UserChoice.CreateAdmin,//ingen sub
                "Update Currency ExchangeRate" => UserChoice.UpdateCurrencyExchange,//inte sub
                "View Logs" => UserChoice.ViewLogs,//inte sub
                "Back" => UserChoice.Back,
                "Logout" => UserChoice.Logout,
                "From File" => UserChoice.FromFile,
                "From The Intrawebbs" => UserChoice.FromInternet,
                _ => UserChoice.Invalid

            };
        }

        public static void FakeBackChoice(string text)
        {
            AnsiConsole.Cursor.Show(false);
            AnsiConsole.MarkupLine($"{MenuColors["Back"]}> {text}[/]");
            Console.ReadKey();
            AnsiConsole.Cursor.Show(true);
        }
        private static (string SelectionPromptTitle, List<string> AccountInformationList, decimal TotalSumOnAccounts) GetBankAccountInfo(List<BankAccount> bankAccounts)
        {
            var accountInfoList = new List<string>();
            int maxName = 12, maxNumber = 10, maxBalance = 7, maxCurrency = 3, maxType = 8, maxInterest = 8;
            decimal totalSumOnAccounts = 0;
            foreach (var account in bankAccounts)
            {
                totalSumOnAccounts += account.GetBalance();
                var info = account.GetAccountInformation();
                maxName = info.Name.Length > maxName ? info.Name.Length : maxName;
                maxNumber = info.Number.Length > maxNumber ? info.Number.Length : maxNumber;
                maxBalance = info.Balance.Length > maxBalance ? info.Balance.Length : maxBalance;
            }

            int accountNamePadding = ((maxName - "Account Name".Length) / 2) - ("Account Name".Length / 2);
            int accountNumberPadding = ("Number".Length / 2) - ((maxNumber - "Number".Length) / 2);
            string questionTitle = string.Format("  {0," + accountNamePadding + "}[" + BankAccountColors["AccountName"] + " bold]{1, " + -(maxName - accountNamePadding) + "}[/] - {0, " + (accountNumberPadding + accountNumberPadding % 2) + "}[" + BankAccountColors["Number"] + " bold]{2, " + -(maxNumber - accountNumberPadding - accountNumberPadding % 2) + "}[/] - [" + BankAccountColors["Balance"] + " bold]{3, " + maxBalance + "}[/] - [" + BankAccountColors["CurrencyType"] + " bold]{4, " + maxCurrency + "}[/] - [" + BankAccountColors["AccountType"] + " bold]{5, " + -maxType + "}[/] - [" + BankAccountColors["Interest"] + " bold]{6, " + maxInterest + "}[/]", "", "Account Name", "Number", "Balance", "Cur", "Type", "Interest");

            foreach (var account in bankAccounts)
            {
                var info = account.GetAccountInformation();
                string text = string.Format("[" + BankAccountColors["AccountName"] + " bold]{0, " + -maxName + "}[/] - [" + BankAccountColors["Number"] + " bold]{1, " + maxNumber + "}[/] - [" + BankAccountColors["Balance"] + " bold]{2, " + maxBalance + "}[/] - [" + BankAccountColors["CurrencyType"] + " bold]{3, " + maxCurrency + "}[/] - [" + BankAccountColors["AccountType"] + " bold]{4, " + -maxType + "}[/] - [" + BankAccountColors["Interest"] + " bold]{5, " + maxInterest + "}[/]", info.Name, info.Number, info.Balance, info.Currency, info.Type, info.Interest);
                accountInfoList.Add(text);
            }
            accountInfoList.Add("Back");
            return (questionTitle, accountInfoList, totalSumOnAccounts);
        }

        public static Table GetBankAccountsAsTable(List<BankAccount> bankAccounts)
        {
            var table = new Table();

            table.AddColumns(new TableColumn($"[{BankAccountColors["AccountName"]}]Account Name[/]").Centered(), new TableColumn($"[{BankAccountColors["Number"]}]Number[/]").Centered(), new TableColumn($"[{BankAccountColors["Balance"]}]Balance[/]").Centered(), new TableColumn($"[{BankAccountColors["CurrencyType"]}]Cur[/]").Centered(), new TableColumn($"[{BankAccountColors["AccountType"]}]Type[/]").Centered(), new TableColumn($"[{BankAccountColors["Interest"]}]Interest[/]").Centered());

            int maxNameLength = bankAccounts.Select(account => account.GetAccountInformation().Name.Length).Max();

            foreach (var account in bankAccounts)
            {
                var info = account.GetAccountInformation();
                table.AddRow(
                    $"[{BankAccountColors["AccountName"]}]{info.Name}[/]",
                    $"[{BankAccountColors["Number"]}]{info.Number}[/]",
                    $"[{BankAccountColors["Balance"]}]{info.Balance}[/]",
                    $"[{BankAccountColors["CurrencyType"]}]{info.Currency}[/]",
                    $"[{BankAccountColors["AccountType"]}]{info.Type}[/]",
                    $"[{BankAccountColors["Interest"]}]{info.Interest}[/]");
            }

            table.Border(TableBorder.Rounded);
            table.BorderColor(BorderColor);
            return table;
        }

        private static Table GetLogTables(List<Log> logs)
        {
            var table = new Table();
            table.AddColumns(new TableColumn($"{LogsColors["Message"]}Message[/]").Centered(), new TableColumn($"{LogsColors["DateAndTime"]}Date and Time[/]").Centered(), new TableColumn($"{LogsColors["LogId"]}Log Id[/]").RightAligned());

            foreach (var log in logs)
            {
                table.AddRow(
                    $"{LogsColors["Message"]}{log.Message}[/]",
                    $"{LogsColors["DateAndTime"]}{log.DateAndTime}[/]",
                    $"{LogsColors["LogId"]}{log.LogId}[/]");
            }
            table.Border(TableBorder.Rounded);
            table.BorderColor(BorderColor);
            return table;
        }
        public static double DecideInterestRate(List<BankAccount> bankAccounts, bool isMakingLoan = false)
        {
            Random rng = new();
            int highestValue = 0;
            decimal totalAmountOfMoneyOnBankAccounts = 0;
            double interest = 0;
            foreach (var bankAccount in bankAccounts)
            {
                totalAmountOfMoneyOnBankAccounts += bankAccount.GetBalance();
            }
            if (isMakingLoan)
            {
                highestValue = totalAmountOfMoneyOnBankAccounts <= 0 ? 100 : totalAmountOfMoneyOnBankAccounts <= 10000 ? 80 : totalAmountOfMoneyOnBankAccounts <= 25000 ? 60 : totalAmountOfMoneyOnBankAccounts <= 50000 ? 40 : totalAmountOfMoneyOnBankAccounts <= 100000 ? 30 : totalAmountOfMoneyOnBankAccounts <= 500000 ? 20 : 10;
                interest = rng.Next(0, highestValue + 1);
                interest *= 0.01;
            }
            else
            {
                interest = rng.NextDouble() / 100;
            }

            return interest;
        }
        private static void WriteTransactionInformation((string Type, string Name, string Number, string Balance, string Currency, string Interest) info)
        {
            AnsiConsole.MarkupLine($"{MenuColors["Highlight"]}Account Name:[/] [{BankAccountColors["AccountName"]}]{info.Name}[/]");
            AnsiConsole.MarkupLine($"{MenuColors["Highlight"]}Balance:[/] [blue]{info.Balance}[/] [{BankAccountColors["Balance"]}]{info.Currency}[/]");
            AnsiConsole.MarkupLine($"{MenuColors["Highlight"]}Withdraw from[/]  [{BankAccountColors["Highlight"]}]{info.Name}[/]");
        }

        private static void WriteDivider(string text = "")
        {
            if (text == "")
            {
                AnsiConsole.Write(new Rule().RuleStyle("grey").LeftJustified());
            }
            else
            {
                AnsiConsole.Write(new Rule($"[gold1]{text}[/]").RuleStyle("grey").LeftJustified());
            }

            AnsiConsole.WriteLine();
        }
        private static void WriteDivider(string dividerTextColor, string dividerColor, string text)
        {
            var rule = new Rule($"[{dividerTextColor}]{text}[/]").LeftJustified();
            rule.Style = Style.Parse(dividerColor);
            AnsiConsole.Write(rule);
            AnsiConsole.WriteLine();
        }

        private static Color UpdateColorBasedOnTimeRemaining(int timeRemaining)
        {
            return timeRemaining switch
            {
                > 10 => Color.Red,
                > 5 => Color.Orange1,
                > 3 => Color.Gold1,
                > 2 => Color.GreenYellow,
                <= 2 => Color.Green,
            }; ;
        }
    }
}
