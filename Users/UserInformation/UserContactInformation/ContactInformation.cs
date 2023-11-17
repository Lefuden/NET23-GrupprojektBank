using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;
using Newtonsoft.Json;

namespace NET23_GrupprojektBank.Users.UserContactInformation
{
    internal class ContactInformation
    {
        public Email Email { get; set; }
        public Phone? Phone { get; set; }
        public Address? Address { get; set; }

        [JsonConstructor]
        public ContactInformation(Email email, Phone phone, Address address)
        {
            Email = email;
            Phone = phone;
            Address = address;
        }
        public ContactInformation(Email email)
        {
            Email = email;
        }
        public ContactInformation(Email email, Phone phone)
        {
            Email = email;
            Phone = phone;
        }
        public ContactInformation(Email email, Address address)
        {
            Email = email;
            Address = address;
        }
    }
}
