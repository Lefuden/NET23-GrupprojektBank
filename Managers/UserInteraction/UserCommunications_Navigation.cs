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
        public static void IncorrectLoginNotification()
        {
            WriteDivider(MenuColors["DividerText"], MenuColors["DividerLine"], "Login");
            // FOIRSTÄTT HÄRRHAIWRH
        }
        public static UserChoice AdminMenu()
        {
            WriteDivider(AdminColors["DividerText"], AdminColors["DividerLine"], "Admin Menu");

            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"{AdminColors["Title"]}What would you like to do today?[/]")
                    .HighlightStyle(HHStyle)
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                      $"{AdminColors["Choice"]}Create Customer Account[/]",
                      $"{AdminColors["Choice"]}Create Admin Account[/]",
                      $"{AdminColors["Choice"]}Update Currency ExchangeRate[/]",
                      $"{AdminColors["Choice"]}View Logs[/]",
                      $"{AdminColors["Back"]}Back[/]",
                      $"{AdminColors["Exit"]}Exit[/]"
                    }
                ));

            return ConvertStringToUserChoice(stringChoice);
        }
        public static List<string> UpdateAdminUserChoiceList(List<string> choiceList, UserChoice userChoice)
        {
            string choiceToBeRemoved = userChoice switch
            {
                UserChoice.Email => $"{AdminColors["Choice"]}Email[/]",
                UserChoice.Phone => $"{AdminColors["Choice"]}Phone[/]",
                UserChoice.Address => $"{AdminColors["Choice"]}Address[/]",
            };
            choiceList.Remove(choiceToBeRemoved);
            return choiceList;
        }
        public static List<string> GetAdminCreateUserChoiceList() => new()
        {
            $"{AdminColors["Choice"]}Email[/]",
            $"{AdminColors["Choice"]}Phone[/]",
            $"{AdminColors["Choice"]}Address[/]",
            $"{AdminColors["Accept"]}Complete[/]",
            $"{AdminColors["Back"]}Back[/]",
        };
        public static UserChoice AdminCreateUserMenu(List<string> menuChoices)
        {
            WriteDivider(AdminColors["DividerText"], AdminColors["DividerLine"], "Admin Menu");
            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"{AdminColors["Title"]}What would you like to do today?[/]")
                    .HighlightStyle(HHStyle)
                    .PageSize(10)
                    .AddChoices(menuChoices));
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
                    .Title($"{UserColors["Title"]}What kind of account would you like to create?[/]")
                    .HighlightStyle(HHStyle)
                    .AddChoices(new[]
                    {
                        $"{UserColors["Choice"]}Create Checkings Account[/]",
                        $"{UserColors["Choice"]}Create Savings Account[/]",
                        $"{UserColors["Back"]}Back[/]"
                    }));


            return ConvertStringToUserChoice(stringChoice);
        }
        public static void ShowLogs(List<Log> logs, string username)
        {

            WriteDivider(MenuColors["DividerText"], MenuColors["DividerLine"], $"Display Logs | {username}");
            WriteLogTables(logs);
            FakeBackChoice("Back");
        }

        public static (string Username, string Password) GetLoginInfo()
        {
            WriteDivider(MenuColors["DividerText"], MenuColors["DividerLine"], "Login");
            var Username = AnsiConsole.Prompt(
                new TextPrompt<string>($"{MenuColors["Input"]}Username: [/]")
                    .PromptStyle(_greenColor)
                    .Validate(Username =>
                        string.IsNullOrWhiteSpace(Username)
                        ? ValidationResult.Error($"{MenuColors["Warning"]}Invalid Username[/]")
                        : Username.Length < 5
                        ? ValidationResult.Error($"{MenuColors["Warning"]}Username must be atleast 5 characters long[/]")
                        : ValidationResult.Success()
                     ));

            var password = AnsiConsole.Prompt(
                new TextPrompt<string>($"{MenuColors["Input"]}Password: [/]")
                    .PromptStyle(_greenColor)
                    .Secret()
                    .Validate(password =>
                        string.IsNullOrEmpty(password)
                        ? ValidationResult.Error($"{MenuColors["Warning"]}Invalid password[/")
                        : password.Length < 2 ? ValidationResult.Error($"{MenuColors["Warning"]}Password must be atleast 2 characters long[/]")
                        : ValidationResult.Success()
                    ));

            return (Username, password);
        }
        public static UserChoice AdminUpdateCurrencyOption()
        {
            AnsiConsole.Clear();
            WriteDivider(MenuColors["DividerText"], MenuColors["DividerLine"], "Currency Exchange Update");
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(5)
                    .HighlightStyle(HHStyle)
                    .Title($"{AdminColors["Title"]}Select a conversion[/]")
                    .AddChoices(
                        $"{AdminColors["Choice"]}From File[/]",
                        $"{AdminColors["Choice"]}From The Intrawebbs[/]",
                        $"{AdminColors["Back"]}Back[/]"
                ));


            return ConvertStringToUserChoice(choice);
        }

        public static EventStatus DisplayLockoutScreenASCII(DateTime lockoutTimeStart, int lockoutDuration)
        {
            AnsiConsole.Cursor.Hide();
            while (DateTime.UtcNow.Subtract(lockoutTimeStart).TotalSeconds < lockoutDuration)
            {
                int remainingTime = lockoutDuration - (int)DateTime.UtcNow.Subtract(lockoutTimeStart).TotalSeconds;


                AnsiConsole.Clear();
                Color timeRemainingColor = UpdateColorBasedOnTimeRemaining(remainingTime);

                AnsiConsole.Write(new FigletText("Locked for:").Centered().Color(timeRemainingColor));
                AnsiConsole.Write(new FigletText(remainingTime.ToString()).Centered().Color(timeRemainingColor));

                Thread.Sleep(1000);
            }
            AnsiConsole.Cursor.Show();
            return EventStatus.LoginUnlocked;
        }
    }
}
