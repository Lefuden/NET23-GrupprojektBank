using NET23_GrupprojektBank.Managers.Logic;
using NET23_GrupprojektBank.Managers.Logs;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static UserChoice MainMenu()
        {

            WriteDivider(MenuColors["DividerText"], MenuColors["DividerLine"], "Hyper Hedgehogs Fundings");
            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"{MenuColors["Title"]}Welcome Menu[/]")
                    .HighlightStyle(HHStyle)
                    .AddChoices(new[]
                    {
                        $"{MenuColors["Choice"]}Login[/]",
                        $"{MenuColors["Exit"]}Exit[/]"
                    }
                ));

            return ConvertStringToUserChoice(stringChoice);
        }
        public static UserChoice AdminMenu()
        {
            WriteDivider(AdminColors["DividerText"], AdminColors["DividerLine"], "Admin Menu");

            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[purple3]What would you like to do today?[/]")
                    .HighlightStyle(HHStyle)
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                      $"{AdminColors["Choice"]}Create Customer Account[/]",
                      $"{AdminColors["Choice"]}Create Admin Account[/]",
                      $"{AdminColors["Choice"]}Update Currency ExchangeRate[/]",
                      $"{AdminColors["Choice"]}View Logs[/]",
                      $"{AdminColors["Back"]}Logout[/]",
                      $"{AdminColors["Exit"]}Exit[/]"
                    }
                )); ;

            return ConvertStringToUserChoice(stringChoice);
        }
        public static UserChoice CustomerMenu()
        {
            WriteDivider(UserColors["DividerText"], UserColors["DividerLine"], "Customer Menu");

            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"{UserColors["Title"]}What would you like to do today?[/]")
                    .PageSize(10)
                    .HighlightStyle(HHStyle)
                    .AddChoices(new[]
                    {
                        $"{UserColors["Choice"]}View Account Balance[/]",
                        $"{UserColors["Choice"]}Create Bank Account[/]",
                        $"{UserColors["Choice"]}Transfer[/]",
                        $"{UserColors["Choice"]}Deposit[/]",
                        $"{UserColors["Choice"]}Withdraw[/]",
                        $"{UserColors["Choice"]}Loan[/]",
                        $"{UserColors["Choice"]}View Logs[/]",
                        $"{UserColors["Back"]}Logout[/]",
                        $"{UserColors["Exit"]}Exit[/]"
                    }
                ));

            return ConvertStringToUserChoice(stringChoice);
        }
        public static UserChoice CreateBankAccount()
        {
            WriteDivider(UserColors["DividerText"], UserColors["DividerLine"], "Create Bank Account");

            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[purple]What would you like to do today?[/]")
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
        public static void ShowLogs(List<Log> logs, string username)
        {

            WriteDivider(MenuColors["DividerText"], MenuColors["DividerLine"], $"Display Logs | {username}");
            var table = GetLogTables(logs);
            table.Expand();
            AnsiConsole.Write(table);
            FakeBackChoice("Back");
        }

        public static (string Username, string Password) GetLoginInfo()
        {
            WriteDivider(MenuColors["DividerText"], MenuColors["DividerLine"], "Login");
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
        public static UserChoice AdminUpdateCurrencyOption()
        {
            Console.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(3)
                    .Title("Select a conversion")
                    .MoreChoicesText("Scroll down for more options")
                    .AddChoices(
                        "From File",
                        "From The Intrawebbs",
                        "Back"
                        )
            );

            return ConvertStringToUserChoice(choice);
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
