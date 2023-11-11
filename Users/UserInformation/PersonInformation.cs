using NET23_GrupprojektBank.Users.UserContactInformation;
namespace NET23_GrupprojektBank.Users.UserInformation
{
    internal class PersonInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SocialSecurityNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ContactInformation ContactInformation { get; set; }

        public PersonInformation(string firstName, string lastName, string socialSecurityNumber, DateTime dateOfBirth, ContactInformation contactInformation)
        {
            FirstName = firstName;
            LastName = lastName;
            SocialSecurityNumber = socialSecurityNumber;
            DateOfBirth = dateOfBirth;
            ContactInformation = contactInformation;
        }
    }
}
