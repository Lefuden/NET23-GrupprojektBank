namespace NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics
{
    internal class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalNumber { get; set; }
        public Address(string country, string city, string street, string postalNumber)
        {
            Country = country;
            City = city;
            Street = street;
            PostalNumber = postalNumber;
        }
    }
}
