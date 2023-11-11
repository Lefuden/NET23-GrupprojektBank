namespace NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics
{
    internal class Phone
    {
        public string? PhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string? WorkPhoneNumber { get; set; }
        public string AreaCode { get; set; }

        public Phone(string phoneNumber, string mobilePhoneNumber, string workPhoneNumber, string areaCode)
        {
            PhoneNumber = phoneNumber;
            MobilePhoneNumber = mobilePhoneNumber;
            WorkPhoneNumber = workPhoneNumber;
            AreaCode = areaCode;
        }
        public Phone(string phoneNumber, string mobilePhoneNumber, string areaCode)
        {
            PhoneNumber = phoneNumber;
            MobilePhoneNumber = mobilePhoneNumber;
            AreaCode = areaCode;
        }
        public Phone(string mobilePhoneNumber, string areaCode)
        {
            MobilePhoneNumber = mobilePhoneNumber;
            AreaCode = areaCode;
        }
    }
}
