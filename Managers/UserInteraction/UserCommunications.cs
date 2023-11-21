using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers.Logic;
using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        private static readonly string pattern = @"\[red bold\](\d+)\[/\]";
        public static CurrencyType ChooseCurrencyType()
        {
            WriteDivider("Choose Currency Type");
            var currencyChoices = new SelectionPrompt<string>()
                .Title("Choose currency type")
                .PageSize(3);

            foreach (CurrencyType type in CurrencyType.GetValuesAsUnderlyingType<CurrencyType>())
            {
                currencyChoices.AddChoice($"{type}");
            }

            var choice = AnsiConsole.Prompt(currencyChoices);
            Console.Clear();
            return Enum.TryParse(choice, out CurrencyType selectedType) ? selectedType : CurrencyType.SEK;
        }
        public static Email GetEmailFromUser()
        {
            WriteDivider("Email Information");
            while (true)
            {
                Console.WriteLine("Enter email information.");
                var email = AnsiConsole.Ask<string>("[green]Email[/]:");
                if (email == "-1") return new Email("-1", "");
                while (true)
                {
                    if (!Email.IsEmailValid(email))
                    {
                        email = AnsiConsole.Ask<string>("[red]invalid email format, try again:[/]");
                    }
                    else
                    {
                        break;
                    }
                }

                var workEmail = AnsiConsole.Ask<string>("[green]Work email[/]:");
                if (workEmail == "-1") return new Email("-1", "");
                while (true)
                {
                    if (!Email.IsEmailValid(workEmail))
                    {
                        workEmail = AnsiConsole.Ask<string>("[red]invalid email format, try again:[/]");
                    }
                    else
                    {
                        break;
                    }
                }

                Console.Clear();
                Console.WriteLine($"Email: {email}\nWork email: {workEmail}\n\n");
                switch (AskUserYesOrNo("is this information correct?"))
                {
                    case true:
                        Console.Clear();
                        return new Email(email, workEmail);
                    case false:
                        Console.Clear();
                        break;
                }
            }
        }

        public static Phone GetPhoneFromUser()
        {
            WriteDivider("Phone Information");
            while (true)
            {
                string? phone;
                while (true)
                {
                    phone = AnsiConsole.Ask<string>("[green]Phone number (10 digits)[/]:");
                    if (phone == "-1") return new Phone("-1");
                    if (phone.Length != 10)
                    {
                        AnsiConsole.MarkupLine("[red]Invalid number length (10 digits)[/]");
                    }
                    if (phone.Length == 10)
                    {
                        if (ulong.TryParse(phone, out ulong i))
                        {
                            break;
                        }

                        AnsiConsole.MarkupLine("[red]Invalid number format (10 digits)[/]");
                    }
                }
                Console.WriteLine($"Phone number: {phone}\n\n");
                switch (AskUserYesOrNo("is this information correct?"))
                {
                    case true:
                        Console.Clear();
                        return new Phone(phone);
                    case false:
                        Console.Clear();
                        break;
                }
            }
        }

        public static Address GetAdressFromUser()
        {
            WriteDivider("Adress Information");
            while (true)
            {
                Console.WriteLine("Enter adress information.");
                var country = AnsiConsole.Ask<string>("[green]Country[/]:");
                if (country == "-1") return new Address("-1", "", "", "");

                var city = AnsiConsole.Ask<string>("[green]City[/]:");
                if (city == "-1") return new Address("-1", "", "", "");

                var street = AnsiConsole.Ask<string>("[green]Street name[/]:");
                if (street == "-1") return new Address("-1", "", "", "");

                var postalNumber = AnsiConsole.Ask<string>("[green]Postal/zip code[/]:");
                if (postalNumber == "-1") return new Address("-1", "", "", "");

                Console.Clear();
                Console.WriteLine($"Country: {country}\nCity: {city}\nStreet name: {street}\nPostal/zip code: {postalNumber}\n\n");
                switch (AskUserYesOrNo("is this information correct?"))
                {
                    case true:
                        Console.Clear();
                        return new Address(country, city, street, postalNumber);
                    case false:
                        Console.Clear();
                        break;
                }
            }
        }

        public static bool AskUserYesOrNo(string message)
        {
            string stringChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"[purple]{message}[/]")
                .PageSize(3)
                .AddChoices(new[]
                    {
                        "Yes",
                        "No"
                    }
                ));
            return stringChoice == "Yes";
        }

        public static DateTime GetBirthDateFromUser()
        {
            WriteDivider("Birth Date Information");
            while (true)
            {
                var userInput = AnsiConsole.Ask<string>("[green]Date of Birth (YYYYMMDD)[/]:");
                if (userInput.Length == 8)
                {
                    userInput = userInput.Insert(6, ",");
                    userInput = userInput.Insert(4, ",");
                }

                if (DateTime.TryParse(userInput, out DateTime validDateFormat))
                {
                    if (UserAgeRestriction(validDateFormat))
                    {
                        return validDateFormat;
                    }

                    AnsiConsole.MarkupLine("[red]User is below the age restriction[/]");
                    return DateTime.MinValue;
                }
                AnsiConsole.MarkupLine("[red]invalid date format, try again[/]");
            }
        }

        public static bool UserAgeRestriction(DateTime userInput)
        {
            int userAge = DateTime.Now.Year - userInput.Year;
            return userAge >= 18;


        }
        public static (string Username, string Password, string FirstName, string LastName, DateTime DateOfBirth) GetBasicsFromUser(List<string> existingUsernames)
        {
            WriteDivider("User Information");
            while (true)
            {
                Console.WriteLine("Enter user information.");
                string? username;
                while (true)
                {
                    username = AnsiConsole.Ask<string>("[green]Username (minimum 5 characters)[/]:");
                    if (username == "-1") return ("-1", "", "", "", DateTime.Now);
                    if (existingUsernames.Contains(username))
                    {
                        Console.WriteLine($"{username} already exists, enter a valid username:");
                    }else if (username.Length < 5)
                    {
                        Console.WriteLine($"{username} is too short, enter a valid username:");
                    }
                    else
                    {
                        break;
                    }
                }

                var password = AnsiConsole.Prompt(new TextPrompt<string>("[green]Password[/]:")
                    .PromptStyle("red")
                    .Secret());
                var firstName = AnsiConsole.Ask<string>("[green]First name[/]:");
                if (firstName == "-1") return ("-1", "", "", "", DateTime.Now);

                var lastName = AnsiConsole.Ask<string>("[green]Last name[/]:");
                if (lastName == "-1") return ("-1", "", "", "", DateTime.Now);

                var dateOfBirth = GetBirthDateFromUser();
                if (dateOfBirth == DateTime.MinValue) return ("-1", "", "", "", DateTime.Now);

                Console.WriteLine($"User name: {username}\nPassword not shown\nFirst name: {firstName}\nLast Name: {lastName}\nBirth date: {dateOfBirth}\n\n");
                switch (AskUserYesOrNo("is this information correct?"))
                {
                    case true:
                        Console.Clear();
                        return (username, password, firstName, lastName, dateOfBirth);
                    case false:
                        Console.Clear();
                        break;
                }
            }
        }

        public static UserChoice MainMenu()
        {
            WriteDivider("Hyper Hedgehogs Fundings");
            string stringChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[purple]Welcome Menu[/]")
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
            WriteDivider("Admin Menu");

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
            WriteDivider("Customer Menu");

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
            WriteDivider($"Create Bank Account");

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
        public static void ViewBankAccounts(List<BankAccount> bankAccounts)
        {
            if (bankAccounts.Count == 0)
            {
                AnsiConsole.MarkupLine("[bold red] You have no accounts in the bank![/]");
                FakeBackChoice("Back");
                return;
            }
            var table = GetBankAccountsAsTable(bankAccounts);
            table.Title("[Purple]Bank Accounts[/]");
            table.BorderColor(Color.Purple);

            AnsiConsole.Write(table);
            FakeBackChoice("Back");
        }

        private static int GetSingleMatch(string pattern, string input)
        {
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);

            if (match.Success)
            {
                // The captured number is in the first capturing group (index 1).
                if (int.TryParse(match.Groups[1].Value, out int convertedValue))
                {
                    return convertedValue;
                }

                return -1;
            }

            return -1;
        }
        public static (BankAccount SourceBankAccount, CurrencyType SourceCurrencyType, DateTime DateAndTime, decimal Sum) MakeWithdrawalMenu(List<BankAccount> bankAccounts)
        {
            WriteDivider($"Withdrawal Menu");

            var accountChoices = GetBankAccountInfo(bankAccounts);

            accountChoices.AccountInformationList.Add("Back");

            var selectedAccountChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(20)
                    .Title("Select an Account to Withdraw from\n" + accountChoices.SelectionPromptTitle)
                    .AddChoices(accountChoices.AccountInformationList)
                );

            int chosenAccountNumber = GetSingleMatch(pattern, selectedAccountChoice);

            var selectedAccount = bankAccounts.FirstOrDefault(account => account.GetAccountNumber() == chosenAccountNumber);

            if (selectedAccount == null)
            {
                return default;
            }

            var info = selectedAccount.GetAccountInformation();
            WriteTransactionInformation(info);

            if (decimal.TryParse(info.Balance, out decimal accountBalance) is not true)
            {
                return default;
            }


            decimal withdrawalAmount = AnsiConsole.Ask<decimal>($"[purple]How Much Would You Like To Withdraw?[/] [gold1](Maximum: {accountBalance})[/]");

            while (withdrawalAmount < 0 || withdrawalAmount > accountBalance)
            {
                AnsiConsole.MarkupLine($"[red]Invalid withdrawal amount. Please enter a valid amount between 0 and {accountBalance}.[/]");
                withdrawalAmount = AnsiConsole.Ask<decimal>($"[purple]How Much Would You Like To Withdraw?[/] [gold1](Maximum: {accountBalance})[/]");
            }

            AnsiConsole.MarkupLine($"[green]{withdrawalAmount:c} {info.Currency}[/] [purple]has been withdrawn from {info.Name}[/]\n");
            FakeBackChoice("Ok");
            CurrencyType currencyTypeParsed = (CurrencyType)Enum.Parse(typeof(CurrencyType), info.Currency ?? CurrencyType.SEK.ToString());
            return (selectedAccount, currencyTypeParsed, DateTime.UtcNow, withdrawalAmount);
        }

        private static void WriteTransactionInformation((string Type, string Name, string Number, string Balance, string Currency, string Interest) info)
        {
            AnsiConsole.MarkupLine($"[purple]Account Name:[/] [green]{info.Name}[/]");
            AnsiConsole.MarkupLine($"[purple]Balance:[/] [blue]{info.Balance}[/] [gold1]{info.Currency}[/]");
            AnsiConsole.MarkupLine($"[purple]Withdraw from[/]  [green]{info.Name}[/]");
        }
        private static void DrawRuler(string content, string colorName)
        {
            AnsiConsole.Write(new Rule($"[{colorName}]{content}[/]"));
        }
        private static void DrawRuler(string content)
        {
            AnsiConsole.Write(new Rule(content));
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
                "From File" => UserChoice.FromFile,
                "From The Intrawebbs" => UserChoice.FromInternet,
                _ => UserChoice.Invalid

            };
        }
        public static (string Username, string Password) GetLoginInfo()
        {
            WriteDivider("Login");
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
                > 10 => Color.Red,
                > 5 => Color.Orange1,
                > 3 => Color.Gold1,
                > 2 => Color.GreenYellow,
                <= 2 => Color.Green,
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
