using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Managers.Logic;
using Spectre.Console;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal class UserCommunications
    {
        
        public static UserChoice MainMenu()
        {
            DrawRuler("Hyper Hedgehogs Fundings", "Gold1");
            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[purple]Welcome Menu[/]")
                    .PageSize(3)
                    .AddChoices(new[]
                    {
                 "Login",
                 "Exit"
                    }
                ));

            return ConvertStringToUserChoice(stringChoice);
        }
        public static UserChoice AdminMenu()
        {
            DrawRuler($"Admin Menu");

            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[purple]What would you like to do today?[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                 "Create Customer Account",
                 "Create Admin Account",
                 "Update Currency ExchangeRate",
                 "View Logs",
                 "Logout",
                 "Exit"
                    }
                ));

            return ConvertStringToUserChoice(stringChoice);
        }
        public static UserChoice CustomerMenu()
        {
            DrawRuler($"Bank Menu");

            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[purple]What would you like to do today?[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                 "View Account Balance",
                 "Create Bank Account",
                 "Transfer",
                 "Deposit",
                 "Withdraw",
                 "Loan",
                 "View Logs",
                 "Logout",
                 "Exit"
                    }
                ));

            return ConvertStringToUserChoice(stringChoice);
        }
        public static UserChoice CreateBankAccount()
        {
            DrawRuler($"Create Bank Account");

            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[purple]What would you like to do today?[/]")
                    .PageSize(3)
                    .AddChoices(new[]
                    {
                 "Create Checkings Account",
                 "Create Savings Account",
                 "Back",
                 "Exit"
                    }
                ));

            return ConvertStringToUserChoice(stringChoice);
        }
        public static void ViewBankAccounts(List<BankAccount> bankAccounts)
        {
            if (bankAccounts.Count != 0)
            {
                var table = new Table().Centered();


                AnsiConsole.Live(table)
                    .AutoClear(false)
                    .Overflow(VerticalOverflow.Ellipsis)
                    .Cropping(VerticalOverflowCropping.Top)
                    .Start(ctx =>
                    {
                        void Update(int delay, Action action)
                        {
                            action();
                            ctx.Refresh();
                            Thread.Sleep(delay);
                        }


                        Update(230, () => table.AddColumn("Account Type"));
                        Update(230, () => table.AddColumn("Account Name"));
                        Update(230, () => table.AddColumn("Account Number"));
                        Update(230, () => table.AddColumn("Balance"));
                        Update(230, () => table.AddColumn("Currency"));
                        Update(230, () => table.AddColumn("Intrest"));

                        Update(70, () => table.Columns[0].Header("[orange1 bold]Account Type[/]"));
                        Update(70, () => table.Columns[1].Header("[yellow bold]Account Name[/]"));
                        Update(70, () => table.Columns[2].Header("[red bold]Account Number[/]"));
                        Update(70, () => table.Columns[3].Header("[green bold]Balance[/]"));
                        Update(70, () => table.Columns[4].Header("[blue bold]Currency[/]"));
                        Update(70, () => table.Columns[5].Header("[cyan1 bold]Interest[/]"));



                        foreach (var account in bankAccounts)
                        {
                         
                            
                                if (account is Checking checking)
                                {
                                    var info = checking.GetAccountInformation();
                                Update(100, () => table.AddRow($"[orange1]{info.Type}[/]", $"[yellow]{info.Name}[/]", $"[red]{info.Number}[/]", $"[green]{info.Balance}[/]", $"[blue]{info.Currency}[/]", "[cyan1][/]"));

                                }
                                if (account is Savings savings)
                                {
                                    var info = savings.GetAccountInformation();
                                Update(100, () => table.AddRow($"[orange1]{info.Type}[/]", $"[yellow]{info.Name}[/]", $"[red]{info.Number}[/]", $"[green]{info.Balance}[/]", $"[blue]{info.Currency}[/]", $"[cyan1]{info.Interest}[/]"));
                                }
                            






                            Update(100, () => table.BorderColor(Color.Purple));

                            Update(0, () => table.Columns[0].Centered());
                            Update(0, () => table.Columns[1].Centered());
                            Update(0, () => table.Columns[2].Centered());
                            Update(0, () => table.Columns[3].Centered());
                            Update(0, () => table.Columns[4].Centered());


                        }
                        Update(500, () => table.Title("Bank Accounts"));
                        Update(400, () => table.Title("[[ [Purple]Bank Accounts[/] ]]"));


                        {
                            string stringChoice = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .PageSize(3)
                                    .AddChoices(new[]
                                    {
                                     "Back"
                                    }
                                ));

                            return ConvertStringToUserChoice(stringChoice);
                        }
                    });
            }
            else
            {
                static UserChoice Nånting()
                {
                    DrawRuler($"No Bank Account");

                    string stringChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[purple]Would you like to Create one or go back[/]")
                            .PageSize(3)
                            .AddChoices(new[]
                            {
                         "Create Bank Account",
                         "Back",
                            }
                        ));
                    return ConvertStringToUserChoice(stringChoice);
                }
            }
        }
            private static void DrawRuler(string content, string colorName)
        {
            AnsiConsole.Write(new Rule($"[{colorName}]{content}[/]"));
        }
        private static void DrawRuler(string content)
        {
            AnsiConsole.Write(new Rule(content));
        }
        private static UserChoice ConvertStringToUserChoice(string input)
        {
            return input switch
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
                _ => UserChoice.Invalid

            };
        }
        public static (string Username, string Password) GetLoginInfo()
        {
            var Username = AnsiConsole.Prompt(
         new TextPrompt<string>("[orange1]Username: [/]")
                    .PromptStyle("green")
                    .Validate(Username =>
                        string.IsNullOrWhiteSpace(Username)
                        ? ValidationResult.Error("[red]Invalid Username[/]")
                        : Username.Length < 5
                        ? ValidationResult.Error("[red]Username must be atleast 5 characters long[/]")
                        : ValidationResult.Success()
                     ));

            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("[orange1]Password: [/]")
                    .PromptStyle("green")
                    .Secret()
                    .Validate(password =>
                        string.IsNullOrEmpty(password)
                        ? ValidationResult.Error("[red]Invalid password[/")
                        : password.Length < 2 ? ValidationResult.Error("[red]Password must be atleast 2 characters long[/]")
                        : ValidationResult.Success()
                    ));

            return (Username, password);
        }
        private static Color UpdateColorBasedOnTimeRemaining(int timeRemaining)
        {
            return timeRemaining switch
            {
                > 30 => Color.Red,
                > 20 => Color.Orange1,
                > 10 => Color.Gold1,
                > 3 => Color.GreenYellow,
                <= 3 => Color.Green,
            }; ;
        }

        public static EventStatus DisplayLockoutScreenASCII(DateTime lockoutTimeStart, int lockoutDuration)
        {

            while (DateTime.UtcNow.Subtract(lockoutTimeStart).TotalSeconds < lockoutDuration)
            {
                int remainingTime = lockoutDuration - (int)DateTime.UtcNow.Subtract(lockoutTimeStart).TotalSeconds;
                Console.CursorVisible = false;

                Console.Clear();
                Color timeRemainingColor = UpdateColorBasedOnTimeRemaining(remainingTime);

                AnsiConsole.Write(new FigletText("Locked for: ").Centered().Color(timeRemainingColor));
                AnsiConsole.Write(new FigletText(remainingTime.ToString()).Centered().Color(timeRemainingColor));


                Thread.Sleep(1000);
            }
            return EventStatus.LoginUnlocked;
        }


    }
}
