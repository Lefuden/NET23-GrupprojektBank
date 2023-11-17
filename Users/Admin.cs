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

        public Customer CreateCustomerAccount(List<string> existingUsernames)
        {
            //return new Customer();
            //give the choice to cancel everything at some point. eg "-1"
            //use input list to check if username already exists
            //after finishing user creation method, move it to usercommunication and then call method here

            while (true)
            {
                Console.WriteLine("Enter user details.");
                var username = AnsiConsole.Ask<string>("[green]Username[/]:");
                while (true)
                {
                    if (username == "-1") { return null; }

                    if (existingUsernames.Contains(username))
                    {
                        Console.WriteLine($"{username} already exists, enter a valid [green]username[/]: ");
                    }
                    else
                    {
                        break;
                    }
                }
                
                var password = AnsiConsole.Prompt(new TextPrompt<string>("[green]Password[/]:")
                        .PromptStyle("red")
                        .Secret());
                if (password == "-1") { return null; }
                
                var firstName = AnsiConsole.Ask<string>("First name: ");
                if (firstName == "-1") { return null; }
                
                var lastName = AnsiConsole.Ask<string>("Last name: ");
                if (lastName == "-1") { return null; }
                
                var dateofBirth = AnsiConsole.Ask<DateTime>("Date of birth format YY,MM,DD or YYYY,MM,DD: "); 
                //make method? to use string as input and then to check if at least 6 or 8 digits and correct order, then add correct format to parse to datetime (yy/mm/dd or yyyy/mm/dd)
                //to get "cancel" input to work. will tinker.
                //if (dateofBirth.ToString() == "-1") { return null; }

                var email = AnsiConsole.Ask<string>("Email: "); //use IsEmailValid(string email) from email.cs to validate?

                Console.Clear();
                Console.WriteLine($"User name: {username}\nPassword not shown\nFirst name: {firstName}\nLast Name: {lastName}" +
                                  $"\nBirth date: {dateofBirth}\nemail: {email}\n\n");
                var answer = AnsiConsole.Ask<string>("is this information correct?\n1. yes\n2. no");
                switch (answer)
                {
                    case "yes":
                    case "1":
                        Console.WriteLine("Would you like to add additional user information?\n1. yes\n2. no");
                        answer = Console.ReadLine().ToLower();

                        switch (answer)
                        {
                            case "yes":
                            case "1":
                                var areaCode = AnsiConsole.Ask<string>("Area code: ");
                                var phone = AnsiConsole.Ask<string>("Phone number: ");
                                var country = AnsiConsole.Ask<string>("Country: ");
                                var city = AnsiConsole.Ask<string>("City: ");
                                var street = AnsiConsole.Ask<string>("Street name: ");
                                var postalNumber = AnsiConsole.Ask<string>("Postal/zip code: ");

                                Console.Clear();
                                Console.WriteLine($"Phone area code: {areaCode}\nPhone number: {phone}\nCountry: {country} \nCity: {city}" +
                                                  $"\nStreet name: {street}\nPostal/zip code: {postalNumber}\\n\\nis this information correct?\\n1. yes\\n2. no");
                                answer = Console.ReadLine().ToLower();
                                switch (answer)
                                {
                                    case "yes":
                                    case "1":
                                        Console.WriteLine($"User {username} has been created");
                                        AddLog(EventStatus.AccountCreationSuccess);
                                        return new Customer(username, password, new PersonInformation(firstName, lastName, dateofBirth, new ContactInformation(new Email(email), new Phone(phone, areaCode), new Address(country, city, street, postalNumber))));

                                    case "no":
                                    case "2":
                                        //loop again
                                        break;
                                }
                                break;
                            case "no":
                            case "2":
                                Console.WriteLine($"User {username} has been created");
                                AddLog(EventStatus.AccountCreationSuccess);
                                return new Customer(username, password, new PersonInformation(firstName, lastName, dateofBirth, new ContactInformation(new Email(email), new Phone("", ""), new Address("", "", "", ""))));
                        }

                        break;
                    case "no":
                    case "2":
                        //loop again
                        break;
                }
            }
        }

        public void UpdateCurrencyExchangeRate()
        {
            EventStatus taskUpdateStatus = CurrencyExchangeRate.UpdateCurrencyExchangeRateAsync(UserType).Result;
            AddLog(taskUpdateStatus);
        }
    }
}
