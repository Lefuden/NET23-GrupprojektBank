using NET23_GrupprojektBank.Managers.Logic;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal class UserCommunications
    {
        public static UserChoice MainMenu()
        {
            DrawRuler("Hyper Hedgehogs Fundings", "navajowhite3");
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
        public static UserChoice CustomerMenu()
        {
            DrawRuler($"Bank Menu");

            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[purple]What would you like to do today?[/]")
                    .PageSize(5)
                    .AddChoices(new[]
                    {
                 "View Account Balance",
                 "Deposit",
                 "Withdraw",
                 "Logout",
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
                "View Account Balance" => UserChoice.ViewBalance,
                "Deposit" => UserChoice.ViewBalance,
                "Withdraw" => UserChoice.ViewBalance,
                "Back" => UserChoice.ViewBalance,
                "Logout" => UserChoice.ViewBalance,
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


    }
}
