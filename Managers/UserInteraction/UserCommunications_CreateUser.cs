using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
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
                    }
                    else if (username.Length < 5)
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
    }
}
