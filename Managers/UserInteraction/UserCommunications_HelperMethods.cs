using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Managers.Logic;
using NET23_GrupprojektBank.Managers.Logs;
using NET23_GrupprojektBank.Managers.Transactions;
using Spectre.Console;
using Color = Spectre.Console.Color;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        private static readonly string _greenColor = "greenyellow";
        private static readonly string _redWarning = "red3";
        private static readonly string _redExit = "indianred_1";
        private static readonly string _title = "mediumpurple1";
        private static readonly string _choice = "mediumpurple2_1";
        private static readonly string _menuInput = "orange1";
        private static readonly string _menuInfo = "orange1";
        private static readonly string _purpleHighlight = "mediumpurple2"; // inside our HHStyle
        private static readonly string _goldDividerText = "gold1";
        private static readonly string _goldDividerLine = "darkgoldenrod";
        private static readonly string _back = "#AF5F8C";
        private static readonly string _accountBalance = "springgreen3_1";
        private static readonly string _accountNumber = "skyblue3";
        private static readonly string _accountInfo = "darkolivegreen3_2";
        public static readonly Color BorderColor = Color.MediumPurple2_1;
        public static readonly Color TableBorderColor = Color.LightGoldenrod2_1;
        private static readonly Style HHStyle = new Style(Color.MediumPurple2, null, Decoration.Bold);
        private static string pattern { get; set; } = @$"\[{_accountNumber} bold\]\s*([\d,]+)\[/\]";
        private static readonly Dictionary<string, string> UserColors = new()
        {
            {"Title",           $"[{_title}]"},
            {"Choice",          $"[{_choice}]"},
            {"Input",           $"[{_menuInput}]"},
            {"Info",            $"[{_menuInfo}]"},
            {"Exit",            $"[{_redExit}]"},
            {"Back",            $"[{_back}]"},
            {"DividerText",     $"{_goldDividerText}"},
            {"DividerLine",     $"{_goldDividerLine}"},
            {"Warning",         $"[{_redWarning}]"},
            {"Highlight",       $"[{_purpleHighlight}]"}
        };
        private static readonly Dictionary<string, string> AdminColors = new()
        {
            {"Title",           $"[{_title}]"},
            {"Choice",          $"[{_choice}]"},
            {"Input",           $"[{_goldDividerLine}]"},
            {"Info",            $"[{_menuInfo}]"},
            {"Exit",            $"[{_redExit}]"},
            {"Back",            $"[{_back}]"},
            {"DividerText",     $"{_goldDividerText}"},
            {"DividerLine",     $"{_goldDividerLine}"},
            {"Warning",         $"[{_redWarning}]"},
            {"Highlight",       $"[{_purpleHighlight}]"}
        };

        private static readonly Dictionary<string, string> MenuColors = new()
        {
            {"Title",           $"[{_title}]"},
            {"Choice",          $"[{_choice}]"},
            {"Input",           $"[{_menuInput}]"},
            {"Info",            $"[{_menuInfo}]"},
            {"Exit",            $"[{_redExit}]"},
            {"Back",            $"[{_back}]"},
            {"DividerText",     $"{_goldDividerText}"},
            {"DividerLine",     $"{_goldDividerLine}"},
            {"Warning",         $"[{_redWarning}]" },
            {"Highlight",       $"[{_purpleHighlight}]"}
        };
        private static readonly Dictionary<string, string> BankAccountColors = new()
        {
            {"Title",           $"{_title}"},
            {"Choice",          $"{_choice}"},
            {"Info",            $"{_menuInfo}"},
            {"Exit",            $"{_redExit}"},
            {"Back",            $"{_back}"},
            {"DividerText",     $"{_goldDividerText}"},
            {"DividerLine",     $"{_goldDividerLine}" },
            {"AccountName",     $"{_accountInfo}"},
            {"Number",          $"{_accountNumber}"},
            {"Balance",         $"{_accountBalance}"},
            {"Success",         $"{_greenColor}"},
            {"CurrencyType",    $"{_accountInfo}"},
            {"AccountType",     $"{_accountInfo}"},
            {"Interest",        $"{_accountInfo}"},
            {"Warning",         $"{_redWarning}"},
            {"Highlight",       $"{_purpleHighlight}" }
        };
        private static readonly Dictionary<string, string> LogsColors = new()
        {
            {"Title",           $"[{_title}]"},
            {"Choice",          $"[{_choice}]"},
            {"Exit",            $"[{_redExit}]"},
            {"Back",            $"[{_back}]"},
            {"DividerText",     $"{_goldDividerText}"},
            {"DividerLine",     $"{_goldDividerLine}"},
            {"Message",         $"[{_title}]"},
            {"DateAndTime",     $"[{_title}]"},
            {"LogId",           $"[{_purpleHighlight}]"},
            {"Warning",         $"[{_redWarning}]"}
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
            AnsiConsole.MarkupLine($"{MenuColors["Highlight"]}> {text}[/]");
            Console.ReadKey();
            AnsiConsole.Cursor.Show(true);
        }

        private static (string SelectionPromptTitle, List<string> AccountInformationList, decimal TotalSumOnAccounts) GetBankAccountInfo(List<BankAccount> bankAccounts)
        {
            var accountInfoList = new List<string>();

            int maxCurrency = 3, maxType = 8, maxInterest = 8;
            decimal totalSumOnAccounts = bankAccounts.Sum(account => account.GetBalance());
            int maxName = bankAccounts.Select(account => account.GetAccountInformation().BankAccountName.Length).Max();
            int maxNumber = bankAccounts.Select(account => account.GetAccountInformation().BankAccountNumber.Length).Max();
            int maxBalance = bankAccounts.Select(account => account.GetAccountInformation().Balance.Length).Max();
            maxName = maxName < 12 ? 12 : maxName;
            maxNumber = maxNumber < 10 ? 10 : maxNumber;
            maxBalance = maxBalance < 7 ? 7 : maxBalance;

            int accountNamePadding = ((maxName - "Account Name".Length) / 2) - ("Account Name".Length / 2);
            int accountNumberPadding = ("Number".Length / 2) - ((maxNumber - "Number".Length) / 2);
            string questionTitle = string.Format("  {0," + accountNamePadding + "}[" + BankAccountColors["AccountName"] + " bold]{1, " + -(maxName - accountNamePadding) + "}[/] - {0, " + (accountNumberPadding + accountNumberPadding % 2) + "}[" + BankAccountColors["Number"] + " bold]{2, " + -(maxNumber - accountNumberPadding - accountNumberPadding % 2) + "}[/] - [" + BankAccountColors["Balance"] + " bold]{3, " + maxBalance + "}[/] - [" + BankAccountColors["CurrencyType"] + " bold]{4, " + maxCurrency + "}[/] - [" + BankAccountColors["AccountType"] + " bold]{5, " + -maxType + "}[/] - [" + BankAccountColors["Interest"] + " bold]{6, " + maxInterest + "}[/]", "", "Account Name", "Number", "Balance", "Cur", "Type", "Interest");

            foreach (var account in bankAccounts)
            {
                var info = account.GetAccountInformation();
                string text = string.Format("[" + BankAccountColors["AccountName"] + " bold]{0, " + -maxName + "}[/] - [" + BankAccountColors["Number"] + " bold]{1, " + maxNumber + "}[/] - [" + BankAccountColors["Balance"] + " bold]{2, " + maxBalance + "}[/] - [" + BankAccountColors["CurrencyType"] + " bold]{3, " + maxCurrency + "}[/] - [" + BankAccountColors["AccountType"] + " bold]{4, " + -maxType + "}[/] - [" + BankAccountColors["Interest"] + " bold]{5, " + maxInterest + "}[/]", info.BankAccountName, info.BankAccountNumber, info.Balance, info.CurrencyType, info.BankAccountType, info.Interest);
                accountInfoList.Add(text);
            }
            accountInfoList.Add($"[{BankAccountColors["Back"]}]Back[/]");
            return (questionTitle, accountInfoList, totalSumOnAccounts);
        }

        public static Table GetBankAccountsAsTable(List<BankAccount> bankAccounts)
        {
            var table = new Table();
            int maxName = bankAccounts.Select(account => account.GetAccountInformation().BankAccountName.Length).Max();
            int accountNamePadding = (((maxName < 12 ? 12 : maxName) - "Account Name".Length) / 2);
            string nameTextColumn = string.Format("{0," + accountNamePadding + "}[" + BankAccountColors["AccountName"] + " bold]{1, " + -(maxName - accountNamePadding) + "}[/]", "", "Account Name");
            table.AddColumns(new TableColumn(nameTextColumn).LeftAligned(), new TableColumn($"[{BankAccountColors["Number"]}]Number[/]").Centered(), new TableColumn($"[{BankAccountColors["Balance"]}]Balance[/]").Centered(), new TableColumn($"[{BankAccountColors["CurrencyType"]}]Cur[/]").Centered(), new TableColumn($"[{BankAccountColors["AccountType"]}]Type[/]").Centered(), new TableColumn($"[{BankAccountColors["Interest"]}]Interest[/]").Centered());
            foreach (var account in bankAccounts)
            {
                var info = account.GetAccountInformation();
                table.AddRow(
                    new Markup($"[{BankAccountColors["AccountName"]}]{info.BankAccountName}[/]").LeftJustified(),
                    new Markup($"[{BankAccountColors["Number"]}]{info.BankAccountNumber}[/]").RightJustified(),
                    new Markup($"[{BankAccountColors["Balance"]}]{info.Balance}[/]").RightJustified(),
                    new Markup($"[{BankAccountColors["CurrencyType"]}]{info.CurrencyType}[/]").LeftJustified(),
                    new Markup($"[{BankAccountColors["AccountType"]}]{info.BankAccountType}[/]").LeftJustified(),
                    new Markup($"[{BankAccountColors["Interest"]}]{info.Interest}[/]").RightJustified()
                    ).LeftAligned();
            }
            table.Border(TableBorder.Rounded);
            table.BorderColor(TableBorderColor);
            return table;
        }

        private static void WriteLogTables(List<Log> logs)
        {
            var table = new Table();
            table.AddColumns(new TableColumn($"{LogsColors["Title"]}Message[/]").LeftAligned(), new TableColumn($"{LogsColors["Title"]}Date and Time[/]").Centered(), new TableColumn($"{LogsColors["Title"]}Log Id[/]").Centered());
            foreach (var log in logs)
            {
                table.AddRow(
                    $"{LogsColors["Choice"]}{log.Message}[/]",
                    $"{LogsColors["Choice"]}{log.DateAndTime}[/]",
                    $"{LogsColors["LogId"]}{log.LogId}[/]");
            }
            table.Border(TableBorder.Rounded);
            table.BorderColor(TableBorderColor);
            AnsiConsole.Write(table);

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
                highestValue = totalAmountOfMoneyOnBankAccounts <= 0 ? 100 : totalAmountOfMoneyOnBankAccounts <= 10000 ? 45 : totalAmountOfMoneyOnBankAccounts <= 25000 ? 35 : totalAmountOfMoneyOnBankAccounts <= 50000 ? 25 : totalAmountOfMoneyOnBankAccounts <= 100000 ? 15 : totalAmountOfMoneyOnBankAccounts <= 500000 ? 10 : 5;
                interest = rng.Next(0, highestValue + 1);
                interest *= 0.01;
            }
            else
            {
                interest = rng.NextDouble() / 100;
            }

            return interest;
        }
        private static void WriteTransactionInformation((string Type, string Name, string Number, string Balance, string Currency, string Interest) info, TransactionType transactionType, decimal transactionAmount, decimal balance)
        {
            string transactionText = transactionType switch
            {
                TransactionType.Loan => "Loan to:",
                TransactionType.Transfer => "Transfer to:",
                TransactionType.Deposit => "Deposit to:",
                TransactionType.Withdrawal => "Withdraw from:",
                _ => ""
            };
            string descriptionText = transactionType switch
            {
                TransactionType.Loan => "Loaned amount:",
                TransactionType.Transfer => "Transferred amount:",
                TransactionType.Deposit => "Deposited amount:",
                TransactionType.Withdrawal => "Withdrawal amount:",
                _ => ""
            };
            decimal inversionMultiplier = transactionType == TransactionType.Transfer ? -1 * transactionAmount : transactionType == TransactionType.Withdrawal ? -1 * transactionAmount : transactionAmount;

            var content = new Markup(
                $"[{BankAccountColors["Highlight"]}]Account Name: [/][{BankAccountColors["Choice"]}]{info.Name}[/]\n" +
                $"[{BankAccountColors["Highlight"]}]{descriptionText}[/] [{BankAccountColors["Balance"]}]{transactionAmount:0.##} {info.Currency}[/]\n" +
                $"[{BankAccountColors["Highlight"]}]New Balance:[/] [{(balance - inversionMultiplier >= 0 ? BankAccountColors["Balance"] : BankAccountColors["Warning"])}]{balance + (inversionMultiplier < 0 ? inversionMultiplier : transactionAmount):0.##} {info.Currency}[/]"
                ).LeftJustified();
            var panel = new Panel(content)
                .RoundedBorder()
                .BorderColor(TableBorderColor)
                .Header($"[{BankAccountColors["Info"]}]{transactionText} {info.Name}[/]")
                .HeaderAlignment(Justify.Left);
            AnsiConsole.Write(panel);

        }

        private static void AddThisAmountOfNewLines(int amountOfNewLines)
        {
            for (int i = 1; i <= amountOfNewLines; i++)
            {
                AnsiConsole.WriteLine();
            }
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
        private static void WriteDivider(string dividerTextColor, string dividerLineColor, string text)
        {
            var rule = new Rule($"[{dividerTextColor}]{text}[/]").LeftJustified();
            rule.Style = Style.Parse(dividerLineColor);
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
