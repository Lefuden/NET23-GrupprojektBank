using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;

namespace NET23_GrupprojektBank.Users.UserContactInformation
{
    internal class ContactInformation
    {
        public Email Email { get; set; }
        public Phone? Phone { get; set; }
        public Address? Address { get; set; }
        public ContactInformation(Email email, Phone phone = null, Address address = null)
        {
            Email = email;
            if (phone is not null)
            {
                Phone = phone;
            }
            if (address is not null)
            {
                Address = address;
            }
        }
    }
}
