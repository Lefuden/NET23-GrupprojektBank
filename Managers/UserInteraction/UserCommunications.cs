using NET23_GrupprojektBank.Managers.Logic;
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
                new SelectionItem("[purple]LogIn[/]", DoLogin),
                new SelectionItem("[red]Exit[/]", () =>
                {
                    AnsiConsole.MarkupLine("[yellow]Exiting the application:Press any key to close window[/]");
                    Environment.Exit(0);
                }),
            };

                var selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<SelectionItem>()
                        .Title("[purple]Starting Menu")
                        .PageSize(3)
                        .AddChoices(choices)
                );

                selectedOption.Action.Invoke();
            }
        }

        static void DoLogin()
        {
            var userName = Prompt("[orange]Username: [/]");
            var password = PromptSecret("[orange]Password: [/]");


            if (Authenticate(userName, password))
            {
                AnsiConsole.MarkupLine("[green]Login successful![/]");
                Thread.Sleep(1000);
                Console.Clear();
                //implement a logo method
                MainMenu();
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Login failed. Please check your credentials.[/]");
            }

            Console.ReadLine();
        }


        static bool Authenticate(string userName, string password)
        {

            return userName == "läggtillfunktion" && password == "läggtillfuktion";
        }
        static void MainMenu()
        {
            while (true)
            {
                AnsiConsole.Render(new Spectre.Console.Rule("Bank Menu"));

                var choices = new[]
                {
                new SelectionItem("View Account Balance", () => ViewAccountBalance()),
                new SelectionItem("Deposit", () => Deposit()),
                new SelectionItem("Withdraw", () => Withdraw()),
                new SelectionItem("Logout", () => { Console.Clear(); AnsiConsole.MarkupLine("[lime]Logged out successfully![/]");Thread.Sleep(3000); Console.Clear(); MakeMenu(); }),
            };

                var selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<SelectionItem>()
                        .Title("[purple]Welcome Select an option:[/]")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .AddChoices(choices)
                );

                selectedOption.Action.Invoke();
            }
        }
        static void ViewAccountBalance()
        {
            AnsiConsole.Render(new Spectre.Console.Rule("[gold1]Account Balance[/]"));

        }

        static void Deposit()
        {
            AnsiConsole.Render(new Spectre.Console.Rule("[gold1]Deposit[/]"));

        }

        static void Withdraw()
        {
            AnsiConsole.Render(new Spectre.Console.Rule("[gold1]Withdraw[/]"));

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
