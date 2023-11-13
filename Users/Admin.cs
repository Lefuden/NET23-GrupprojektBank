using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Users.UserInformation;

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
            //Console.WriteLine("userName, password, firstName, lastName, socialSecurityNumber, dateOfBirth");
            //string userName = Console.ReadLine();
            //string password = Console.ReadLine();
            //string firstName = Console.ReadLine();
            //string lastName = Console.ReadLine();
            //string socialSecurityNumber = Console.ReadLine();
            //DateTime dateOfBirth = DateTime.Now;

            //return new Customer(userName, password, new PersonInformation(firstName, lastName, socialSecurityNumber, dateOfBirth));
        }
        public CurrencyType UpdateCurrencyExchangeRate(CurrencyType updateCurrencyType)
        {
            //UpdateCurrencyExchangeRateAsync();
            //call menu, choose currency type, take input, return value
            //CurrencyType currency = UpdateCurrencyExchange(updateCurrencyType);
            return CurrencyType.SEK;
        }
    }
}
