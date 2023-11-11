using NET23_GrupprojektBank.Users.UserInformation;
using NET23_GrupprojektBank.Managers.Logs;

namespace NET23_GrupprojektBank.Users
{
    public enum UserType
    {
        Customer,
        Admin,
        Undeclared
    }

    internal abstract class User
    {
        protected string UserName { get; set; }
        protected Guid UserId { get; set; }
        protected string Salt { get; set; }
        protected string HashedPassword { get; set; }
        protected PersonInformation Person { get; set; }
        protected UserType UserType { get; set; }
        protected List<Log> Logs { get; set; }
        
        public User(string userName, string password, PersonInformation person)
        {
            UserName = userName;
            Person = new PersonInformation();
            UserId = Guid.NewGuid();
            Salt = BCrypt.Net.BCrypt.GenerateSalt();
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(password + Salt);
            Logs = new List<Log>();
            UserType = UserType.Undeclared;
        }

        internal bool CompareUserName(string userName)
        {
            return userName == UserName;
        }

        internal bool CompareUserPassword(string userPassword)
        {
            return BCrypt.Net.BCrypt.Verify(userPassword + Salt, HashedPassword);
        }

        //work in progress - direction/help might be wanted
        //internal void Addlog(string userName, Log log)
        //{
        //    if (!Logs.Contains(userName))
        //    {
        //        //create new log?
        //    }
        //    else
        //    {
        //        Logs.Add(userName);
        //    }
        //}
    }
}