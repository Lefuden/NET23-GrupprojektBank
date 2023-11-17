using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics
{
    internal class Email
    {
        public string EmailAddress { get; set; }
        public string? WorkEmailAddress { get; set; }
        public Email(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        [JsonConstructor]
        public Email(string emailAddress, string workEmailAddress)
        {
            EmailAddress = emailAddress;
            WorkEmailAddress = workEmailAddress;
        }
        public static bool IsEmailValid(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}
