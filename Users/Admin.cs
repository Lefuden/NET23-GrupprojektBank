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

        public Admin CreateAdminAccount(List<string> existingUserNames)
        {
            //call method from usercommunication
            //return new Admin();
            throw new NotImplementedException();
        }

        public Customer CreateCustomerAccount(List<string> existingUserNames)
        {
            //return new Customer();
            //give the choice to cancel everything at some point. eg "-1"
            //use input list to check if username already exists
            //after finishing user creation method, move it to usercommunication and then call method here

            while (true)
            {
                Console.WriteLine("Enter user details.");
                var userName = AnsiConsole.Ask<string>("[green]User name[/]:");
                //call method IsUserNameAlreadyTaken(username) from logic manager
                //       var exisingusernames = GetAllUsernames();
                //       var username = AnsiConsole.Ask<string>("Enter username: ");
                //       if(exisingusernames.Contains(username))
                //       {
                //           // Användarnamnet finns, försök igen... (while loop kanske?
                //       }
                ////Användarnmnet är godkänt, fortsätt...

                var password = AnsiConsole.Prompt(new TextPrompt<string>("[green]Password[/]:")
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
                                var areaCode = AnsiConsole.Ask<string>("First name: ");
                                var phone = AnsiConsole.Ask<string>("First name: ");
                                var country = AnsiConsole.Ask<string>("First name: ");
                                var city = AnsiConsole.Ask<string>("First name: ");
                                var street = AnsiConsole.Ask<string>("First name: ");
                                var postalNumber = AnsiConsole.Ask<string>("First name: ");

                                Console.Clear();
                                Console.WriteLine($"Phone area code: {areaCode}\nPhone number: {phone}\nCountry: {country} \nCity: {city}" +
                                                  $"\nStreet name: {street}\nPostal code: {postalNumber}\\n\\nis this information correct?\\n1. yes\\n2. no");
                                answer = Console.ReadLine().ToLower();
                                switch (answer)
                                {
                                    case "yes":
                                    case "1":
                                        Console.WriteLine($"User {userName} has been created");
                                        Addlog(EventStatus.AccountCreationSuccess);
                                        Customer hej1 = new Customer(userName, password, new PersonInformation(firstName, lastName, dateofBirth, new ContactInformation(new Email(email), new Phone(phone, areaCode), new Address(country, city, street, postalNumber))));
                                        Console.WriteLine(hej1);
                                        return hej1;

                                    case "no":
                                    case "2":
                                        //loop again
                                        break;
                                }
                                break;
                            case "no":
                            case "2":
                                Console.WriteLine($"User {userName} has been created");
                                Customer hej2 = new Customer(userName, password, new PersonInformation(firstName, lastName, dateofBirth, new ContactInformation(new Email(email), new Phone("", ""), new Address("", "", "", ""))));
                                Addlog(EventStatus.AccountCreationSuccess);
                                hej2.Addlog(EventStatus.AccountCreationSuccess);
                                Console.WriteLine(hej2);
                                return hej2;
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
            Task<EventStatus> taskUpdateStatus = CurrencyExchangeRate.UpdateCurrencyExchangeRateAsync(UserType);
            //EventStatus eventStatus = taskUpdateStatus.Result;
            Addlog(taskUpdateStatus.Result);

            //var updateStatus = CurrencyExchangeRate.UpdateCurrencyExchangeRateAsync(UserType);
            //while (updateStatus.IsCompleted is not true)
            //{
            // fake async waiting inside a console application...
            //}
        }
    }
}
