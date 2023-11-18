using Newtonsoft.Json;

namespace NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics
{
    internal class Phone
    {
        public string? PhoneNumber { get; set; }
        
        [JsonConstructor]
        public Phone(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
    }
}
