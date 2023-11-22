﻿using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;
using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {
        public static Email GetEmailFromUser()
        {
            WriteDivider($"{AdminColors["DividerText"]}", $"{AdminColors["DividerLine"]}", "Email Information");
            while (true)
            {
                AnsiConsole.MarkupLine($"{AdminColors["Title"]}Enter email information.[/]");
                var email = AnsiConsole.Ask<string>($"{AdminColors["Title"]}Email[/]:");
                if (email == "-1") return new Email("-1", "");
                while (true)
                {
                    if (!Email.IsEmailValid(email))
                    {
                        email = AnsiConsole.Ask<string>($"{AdminColors["Warning"]}invalid email format, try again:[/]");
                    }
                    else
                    {
                        break;
                    }
                }

                var workEmail = AnsiConsole.Ask<string>($"{AdminColors["Choice"]}Work email[/]:");
                if (workEmail == "-1") return new Email("-1", "");
                while (true)
                {
                    if (!Email.IsEmailValid(workEmail))
                    {
                        workEmail = AnsiConsole.Ask<string>($"{AdminColors["Warning"]}invalid email format, try again:[/]");
                    }
                    else
                    {
                        break;
                    }
                }

                AnsiConsole.Clear();

                var content = new Markup(
                $"{AdminColors["Choice"]}Email: [/]{AdminColors["Highlight"]}{email}[/]\n" +
                $"{AdminColors["Choice"]}Work email:[/] {AdminColors["Highlight"]}{workEmail}[/]"
                ).LeftJustified();
                var panel = new Panel(content)
                    .RoundedBorder()
                    .BorderColor(BorderColor)
                    .Header($"{AdminColors["Input"]}Email[/]")
                    .HeaderAlignment(Justify.Left);
                AnsiConsole.Write(panel);

                AddThisAmountOfNewLines(1);

                switch (AskUserYesOrNo("Is this information correct?"))
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

            WriteDivider($"{AdminColors["DividerText"]}", $"{AdminColors["DividerLine"]}", "Phone Information");
            while (true)
            {
                string? phone;
                while (true)
                {
                    phone = AnsiConsole.Ask<string>($"{AdminColors["Choice"]}Phone number (10 digits)[/]:");
                    if (phone == "-1") return new Phone("-1");
                    if (phone.Length != 10)
                    {
                        AnsiConsole.MarkupLine($"{AdminColors["Warning"]}Invalid number length! You wrote:[/] {AdminColors["Highlight"]}{phone.Length}[/].\n{AdminColors["Warning"]}Try again with 10 digits[/]");
                    }
                    if (phone.Length == 10)
                    {
                        if (ulong.TryParse(phone, out ulong i))
                        {
                            break;
                        }

                        AnsiConsole.MarkupLine($"{AdminColors["Warning"]}Invalid number format (10 digits)[/]");
                    }
                }

                var content = new Markup(
                $"{AdminColors["Choice"]}Phone number: [/]{AdminColors["Highlight"]}{phone}[/]\n")
                    .LeftJustified();
                var panel = new Panel(content)
                    .RoundedBorder()
                    .BorderColor(BorderColor)
                    .Header($"{AdminColors["Input"]}Phone[/]")
                    .HeaderAlignment(Justify.Left);
                AnsiConsole.Write(panel);

                AddThisAmountOfNewLines(1);

                switch (AskUserYesOrNo("Is this information correct?"))
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
            WriteDivider($"{AdminColors["DividerText"]}", $"{AdminColors["DividerLine"]}", "Adress Information");
            while (true)
            {
                AnsiConsole.MarkupLine($"{AdminColors["Title"]}Enter adress information.[/]");
                var country = AnsiConsole.Ask<string>($"{AdminColors["Choice"]}Country[/]:");
                if (country == "-1") return new Address("-1", "", "", "");

                var city = AnsiConsole.Ask<string>($"{AdminColors["Choice"]}City[/]:");
                if (city == "-1") return new Address("-1", "", "", "");

                var street = AnsiConsole.Ask<string>($"{AdminColors["Choice"]}Street name[/]:");
                if (street == "-1") return new Address("-1", "", "", "");

                var postalNumber = AnsiConsole.Ask<string>($"{AdminColors["Choice"]}Postal/zip code[/]:");
                if (postalNumber == "-1") return new Address("-1", "", "", "");

                AnsiConsole.Clear();
                var content = new Markup(
                $"{AdminColors["Choice"]}Country: [/]{AdminColors["Highlight"]}{country}[/]\n" +
                $"{AdminColors["Choice"]}City: [/]{AdminColors["Highlight"]}{city}[/]\n" +
                $"{AdminColors["Choice"]}Street name: [/]{AdminColors["Highlight"]}{street}[/]\n" +
                $"{AdminColors["Choice"]}Postal/zip code:[/] {AdminColors["Highlight"]}{postalNumber}[/]"
                ).LeftJustified();
                var panel = new Panel(content)
                    .RoundedBorder()
                    .BorderColor(BorderColor)
                    .Header($"{AdminColors["Input"]}Address[/]")
                    .HeaderAlignment(Justify.Left);
                AnsiConsole.Write(panel);

                AddThisAmountOfNewLines(2);

                switch (AskUserYesOrNo("Is this information correct?"))
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
            WriteDivider($"{AdminColors["DividerText"]}", $"{AdminColors["DividerLine"]}", "Birth Date Information");
            while (true)
            {
                var userInput = AnsiConsole.Ask<string>($"{AdminColors["Title"]}Date of Birth (YYYYMMDD)[/]:");
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

                    AnsiConsole.MarkupLine($"{AdminColors["Warning"]}User is below the age restriction[/]");
                    return DateTime.MinValue;
                }
                AnsiConsole.MarkupLine($"{AdminColors["Warning"]}invalid date format, try again[/]");
            }
        }

        public static (string Username, string Password, string FirstName, string LastName, DateTime DateOfBirth) GetBasicsFromUser(List<string> existingUsernames)
        {
            WriteDivider($"{AdminColors["DividerText"]}", $"{AdminColors["DividerLine"]}", "User Information");
            while (true)
            {
                AnsiConsole.MarkupLine($"{AdminColors["Title"]}Enter user information.[/]");
                string? username;
                while (true)
                {
                    username = AnsiConsole.Ask<string>($"{AdminColors["Choice"]}Username (minimum 5 characters)[/]:");
                    if (username == "-1") return ("-1", "", "", "", DateTime.Now);
                    if (existingUsernames.Contains(username))
                    {
                        AnsiConsole.MarkupLine($"{AdminColors["Warning"]}{username} already exists, enter a valid username:[/]");
                    }
                    else if (username.Length < 5)
                    {
                        AnsiConsole.MarkupLine($"{AdminColors["Choice"]}{username} is too short, enter a valid username:[/]");
                    }
                    else
                    {
                        break;
                    }
                }

                var password = AnsiConsole.Prompt(new TextPrompt<string>($"{AdminColors["Title"]}Password[/]:")
                    .PromptStyle("red")
                    .Secret());
                var firstName = AnsiConsole.Ask<string>($"{AdminColors["Title"]}First name[/]:");
                if (firstName == "-1") return ("-1", "", "", "", DateTime.Now);

                var lastName = AnsiConsole.Ask<string>($"{AdminColors["Title"]}Last name[/]:");
                if (lastName == "-1") return ("-1", "", "", "", DateTime.Now);

                var dateOfBirth = GetBirthDateFromUser();
                if (dateOfBirth == DateTime.MinValue) return ("-1", "", "", "", DateTime.Now);

                AnsiConsole.Clear();
                var content = new Markup(
                $"{AdminColors["Choice"]}User name: [/]{AdminColors["Highlight"]}{username}[/]\n" +
                $"{AdminColors["Choice"]}Password: [/]{AdminColors["Highlight"]}**********[/]\n" +
                $"{AdminColors["Choice"]}First name: [/]{AdminColors["Highlight"]}{firstName}[/]\n" +
                $"{AdminColors["Choice"]}Last Name: [/]{AdminColors["Highlight"]}{lastName}[/]\n" +
                $"{AdminColors["Choice"]}Birth date:[/] {AdminColors["Highlight"]}{dateOfBirth:d}[/]"
                ).LeftJustified();
                var panel = new Panel(content)
                    .RoundedBorder()
                    .BorderColor(BorderColor)
                    .Header($"{AdminColors["Input"]}User Information[/]")
                    .HeaderAlignment(Justify.Left);
                AnsiConsole.Write(panel);

                AddThisAmountOfNewLines(1);

                switch (AskUserYesOrNo("Is this information correct?"))
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