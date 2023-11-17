using NET23_GrupprojektBank.Managers.Logic;
using Spectre.Console;
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
