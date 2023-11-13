using Spectre.Console;
using System.Net.Http.Headers;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal class UserCommunications
    {
        public static void MakeMenu()
        {
            while (true)
            {
                AnsiConsole.MarkupLine("[yellow]Welcome to Hyper HedgHoges Bank[/]");

                var choices = new[]
                {
                new SelectionItem("LogIn", DoLogin),
                new SelectionItem("Exit", () =>
                {
                    AnsiConsole.MarkupLine("[yellow]Exiting the application:Press any key to close window[/]");
                    Environment.Exit(0);
                }),
            };

                var selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<SelectionItem>()
                        .Title("Starting Menu")
                        .PageSize(3)

                        .AddChoices(choices)
                );

                selectedOption.Action.Invoke();
            }
        }

        static void DoLogin()
        {
            var userName = Prompt("Username: ");
            var passWord = PromptSecret("Password: ");


            if (Authenticate(userName, passWord))
            {
                AnsiConsole.MarkupLine("[green]Login successful![/]");

            }
            else
            {
                AnsiConsole.MarkupLine("[red]Login failed. Please check your credentials.[/]");
            }

            Console.ReadLine();
        }


        static bool Authenticate(string userName, string passWord)
        {

            return userName == "user" && passWord == "pass";
        }


        static string Prompt(string prompt)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>(prompt)
                    .PromptStyle("green")
                    .Validate(textUser => ValidateUsername(textUser))

                    );
        }
        static ValidationResult ValidateUsername(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length < 3 || input.Contains(" "))
            {
                return ValidationResult.Error("[red]Invalid input! Please enter at least 3 letters and make sure to not use spaces.[/]");
            }

            return ValidationResult.Success();
        }


        static string PromptSecret(string prompt)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>(prompt)
                    .PromptStyle("green")
                    .Secret()
                    .Validate(textPassword => ValidatePassword(textPassword))

            );
        }
        static ValidationResult ValidatePassword(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length < 5 || input.Contains(" "))
            {
                return ValidationResult.Error("[red]Invalid input! Please enter at least 5 letters and make sure to not use spaces.[/]");
            }

            return ValidationResult.Success();
        }


        class SelectionItem
        {
            public string Text { get; }
            public Action Action { get; }

            public SelectionItem(string text, Action action)
            {
                Text = text;
                Action = action;
            }

            public override string ToString() => Text;
        }


    }
}
