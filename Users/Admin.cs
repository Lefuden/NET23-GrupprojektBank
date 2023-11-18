using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Managers.Logs;
using NET23_GrupprojektBank.Users.UserContactInformation;
using NET23_GrupprojektBank.Users.UserInformation;
using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;
using Newtonsoft.Json;
using Spectre.Console;

namespace NET23_GrupprojektBank.Users
{
    internal class Admin : User
    {
        public Admin(string username, string password, PersonInformation person) : base(username, password, person)
        {
            UserType = UserType.Admin;
        }

        [JsonConstructor]
        public Admin(string username, Guid userId, string salt, string hashedPassword, PersonInformation personInformation, UserType userType, List<Log> logs) : base(username, userId, salt, hashedPassword, personInformation, userType, logs)
        {

        }

        public Admin CreateAdminAccount(List<string> existingUsernames)
        {
            //call method from usercommunication
            //return new Admin();
            throw new NotImplementedException();
        }

        public static Customer CreateCustomerAccount(List<string> existingUsernames)
        {
            var (username, password, firstName, lastName, dateOfBirth) = GetBasicsFromUser(existingUsernames);
            if (username == "-1") return null;

            if (!AskUserYesOrNo("Add more information?"))
            {
                return new Customer(username, password, 
                       new PersonInformation(firstName, lastName, dateOfBirth, 
                       new ContactInformation(new Email(""))));
            }

            Email email = GetEmailFromUser();
            if (email.EmailAddress == "-1") return null;
            
            if (!AskUserYesOrNo("Add more information?"))
            {
                return new Customer(username, password, 
                       new PersonInformation(firstName, lastName, dateOfBirth, 
                       new ContactInformation(email)));
            }


            Phone phone = GetPhoneFromUser();
            if (phone.PhoneNumber == "-1") return null;
            
            if (!AskUserYesOrNo("Add more information?"))
            {
                return new Customer(username, password, 
                       new PersonInformation(firstName, lastName, dateOfBirth, 
                       new ContactInformation(email, phone)));
            }

            Address adress = GetAdressFromUser();
            if (adress.Country == "-1") return null;

            return new Customer(username, password, 
                   new PersonInformation(firstName, lastName, dateOfBirth,
                   new ContactInformation(email, phone, adress)));
        }

        public static (string Username, string Password, string FirstName, string LastName, DateTime DateOfBirth) GetBasicsFromUser(List<string> existingUsernames)
        {
            while (true)
            {
                Console.WriteLine("Enter user information.");
                string? username;
                while (true)
                {
                    username = AnsiConsole.Ask<string>("[green]Username[/]:");
                    if (username == "-1") return ("-1", "", "", "", DateTime.Now);
                    if (existingUsernames.Contains(username))
                    {
                        Console.WriteLine($"{username} already exists, enter a valid username:");
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

                DateTime dateOfBirth;
                while (true)
                {
                    var userInput = AnsiConsole.Ask<string>("[green]Date of birth (YYYYMMDD)[/]:");
                    if (userInput == "-1") return ("-1", "", "", "", DateTime.Now);
                    if (userInput.Length == 8)
                    {
                        userInput = userInput.Insert(6, ",");
                        userInput = userInput.Insert(4, ",");
                        dateOfBirth = DateTime.Parse(userInput); //if length == 8, check if DD is within correct range, MM correct range.
                        break;
                    }

                    if (userInput.Length is > 8 or < 8)
                    {
                        dateOfBirth = DateTime.Now;
                        break;
                    }
                }
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
        
        public static Email GetEmailFromUser()
        {
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

        public static Phone GetPhoneFromUser() //requires set amount of digits on phone number?
        {
            while (true)
            {
                Console.WriteLine("Enter phone information.");
                var mobilePhone = AnsiConsole.Ask<string>("[green]Mobilephone number[/]:");
                if (mobilePhone == "-1") return new Phone("-1");
                
                //error handling on number format
                
                Console.Clear();
                Console.WriteLine($"Phone number: {mobilePhone}\n\n");
                switch (AskUserYesOrNo("is this information correct?"))
                {
                    case true:
                        Console.Clear();
                        return new Phone(mobilePhone);
                    case false:
                        Console.Clear();
                        break;
                }
            }
        }

        public static Address GetAdressFromUser()
        {
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

        public void UpdateCurrencyExchangeRate()
        {
            EventStatus taskUpdateStatus = CurrencyExchangeRate.UpdateCurrencyExchangeRateAsync(UserType).Result;
            AddLog(taskUpdateStatus);
        }
    }
}
