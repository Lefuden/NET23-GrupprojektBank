using NET23_GrupprojektBank.Users.UserInformation;
using NET23_GrupprojektBank.Managers.Logs;

namespace NET23_GrupprojektBank.Users
{
    internal abstract class User
    {
        protected string UserName { get; set; }
        protected Guid UserId { get; set; }
        protected string Salt { get; set; }
        protected string HashedPassword { get; set; }
        protected PersonInformation Person { get; set; }
        protected Enum UserType { get; set; }
        protected List<Log> Logs { get; set; }
        protected enum Type
        {
            Customer,
            Admin,
            Undeclared
        }
        
        protected User(string userName, string password)
        {
            UserName = userName;
            Person = new PersonInformation();
            UserId = Guid.NewGuid();
            Salt = BCrypt.Net.BCrypt.GenerateSalt();
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(password + Salt);
            Logs = new List<Log>();
            UserType = Type.Undeclared;
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