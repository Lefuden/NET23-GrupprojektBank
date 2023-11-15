using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Users.UserContactInformation;
using NET23_GrupprojektBank.Users.UserInformation;
using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;
using Spectre.Console;

namespace NET23_GrupprojektBank.Users
{
    internal class Admin : User
    {
        public Admin(string userName, string password, PersonInformation person) : base(userName, password, person)
        {
            UserType = UserType.Admin;
        }
        public Customer CreateUserAccount()
        {
            //give option to create customer or admin


            while (true)
            {
                Console.WriteLine("Enter user details.");
                var userName = AnsiConsole.Ask<string>("[green]User name[/]:"); //make method to check if user already exists
                //var password = AnsiConsole.Ask<string>("Password: ");
                var password = AnsiConsole.Prompt(
                    new TextPrompt<string>("[green]Password[/]:")
                        .PromptStyle("red")
                        .Secret());

                var firstName = AnsiConsole.Ask<string>("First name: ");
                var lastName = AnsiConsole.Ask<string>("Last name: ");
                var dateofBirth = AnsiConsole.Ask<DateTime>("Date of birth: "); //make method to check if at least 6 or 8 digits and correct order (yy/mm/dd or yyyy/mm/dd)
                var email = AnsiConsole.Ask<string>("Email: "); //use IsEmailValid(string email) from email.cs to validate?
                
                Console.Clear();
                Console.WriteLine($"User name: {userName}\nPassword not shown\nFirst name: {firstName}\nLast Name: {lastName}" +
                                  $"\nBirth date: {dateofBirth}\nemail: {email}\n\nis this information correct?\n1. yes\n2. no");
                var answer = Console.ReadLine().ToLower();
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
                                Console.WriteLine("Enter phone area code: ");
                                string areacode = Console.ReadLine();
                                Console.WriteLine("Enter phone number: ");
                                string phone = Console.ReadLine();
                                Console.WriteLine("Enter country: ");
                                string country = Console.ReadLine();
                                Console.WriteLine("Enter city: ");
                                string city = Console.ReadLine();
                                Console.WriteLine("Enter street name: ");
                                string street = Console.ReadLine();
                                Console.WriteLine("Enter postal number / zip code: ");
                                string postalnumber = Console.ReadLine();

                                Console.Clear();
                                Console.WriteLine($"Phone area code: {areacode}\nPhone number: {phone}\nCountry: {country} \nCity: {city}" +
                                                  $"\nStreet name: {street}\nPostal code: {postalnumber}\\n\\nis this information correct?\\n1. yes\\n2. no");
                                answer = Console.ReadLine().ToLower();
                                switch (answer)
                                {
                                    case "yes":
                                    case "1":
                                        Console.WriteLine($"User {userName} has been created");
                                        Addlog(EventStatus.AccountCreationSuccess);
                                        return new Customer(userName, password, new PersonInformation(firstName, lastName, dateofBirth, new ContactInformation(new Email(email), new Phone(phone, areacode), new Address(country, city, street, postalnumber))));
                                        break;

                                    case "no":
                                    case "2":
                                        //loop again
                                        break;
                                }
                                break;
                            case "no":
                            case "2":
                                Console.WriteLine($"User {userName} has been created");
                                Addlog(EventStatus.AccountCreationSuccess);
                                return new Customer(userName, password, new PersonInformation(firstName, lastName, dateofBirth, new ContactInformation(new Email(email), new Phone("", ""), new Address("", "", "", ""))));
                        }

                        break;
                    case "no":
                    case "2":
                        //loop again
                        break;
                }
            }
            

            //return new Customer("", "", new PersonInformation("", "", "", DateTime.Now, 
            //    new ContactInformation(new Email(""), new Phone("",""), new Address("","","",""))));
        }
        public void UpdateCurrencyExchangeRate()
        {
            // call UpdateCurrencyExchangeRateAsync(); ?


            //call menu, choose currency type, take input, return value
            //CurrencyType currency = UpdateCurrencyExchange(updateCurrencyType);
        }
    }
}
