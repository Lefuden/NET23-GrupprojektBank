﻿using NET23_GrupprojektBank.Users.UserContactInformation;

namespace NET23_GrupprojektBank.Users.UserInformation
{
    internal class PersonInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ContactInformation ContactInformation { get; set; }

        public PersonInformation(string firstName, string lastName, DateTime dateOfBirth, ContactInformation contactInformation)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            ContactInformation = contactInformation;
        }
    }
}
