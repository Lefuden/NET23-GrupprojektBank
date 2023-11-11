using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Users.UserInformation;

namespace NET23_GrupprojektBank.Users
{
    internal class Admin : User
    {
        public Admin(string userName, string password) : base(userName, password)
        {
            UserType = UserType.Admin;
        }

        public static Customer CreateUserAccount()
        {
            //console input/output to enter username/firstname/lastname/password etc
            //call method, or add here?

            return new Customer("", "", new PersonInformation());
        }

        public static CurrencyType UpdateCurrencyExchangeRate()
        {
            //call menu, choose currency type, take input, return value
            return CurrencyType.SEK;
        }
    }
}
