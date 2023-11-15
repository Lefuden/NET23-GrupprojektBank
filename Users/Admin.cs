using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Users.UserContactInformation;
using NET23_GrupprojektBank.Users.UserInformation;
using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;

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
            //console input/output to enter username/firstname/lastname/password etc
            //call method, or add here?
            
            //date of birth and social security number is the same thing though..
            //DateTime dateOfBirth = DateTime.Now;
            //Console.WriteLine("Enter date of birth: ");

            while (true)
            {
                Console.WriteLine("Enter a new user name: ");
                string userName = Console.ReadLine(); //make method to check if user already exists
                Console.WriteLine("Enter password: ");
                string password = Console.ReadLine();
                Console.WriteLine("Enter first name: ");
                string firstName = Console.ReadLine();
                Console.WriteLine("Enter last name: ");
                string lastName = Console.ReadLine();
                Console.WriteLine("Enter social security number: ");
                string socialSecurityNumber = Console.ReadLine(); //make method to check if at least 6 or 8 digits (yy/mm/dd or yyyy/mm/dd)
                Console.WriteLine("Enter email: ");
                string email = Console.ReadLine(); //use IsEmailValid(string email) from email.cs to validate?

                Console.Clear();
                Console.WriteLine($"User name: {userName}\nPassword not shown\nFirst name: {firstName}\nLast Name: {lastName}" +
                                  $"\nSocial security number: {socialSecurityNumber}\nemail: {email}\n\nis this information correct?\n1. yes\n2. no");
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

                                break;
                            case "no":
                            case "2":
                                Console.WriteLine($"User {userName} has been created");
                                Addlog(EventStatus.AccountCreationSuccess);
                                return new Customer(userName, password, new PersonInformation(firstName, lastName, socialSecurityNumber, DateTime.Now, new ContactInformation(new Email(email), new Phone("", ""), new Address("", "", "", ""))));
                        }

                        break;
                    case "no":
                    case "2":
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
