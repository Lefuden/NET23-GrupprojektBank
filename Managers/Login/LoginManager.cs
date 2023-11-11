using NET23_GrupprojektBank.Users;

namespace NET23_GrupprojektBank.Managers.Login
{
    public enum LoginStatus
    {
        Success,
        Failed,
        Locked
    }
    internal class LoginManager
    {
        private List<User> Users { get; set; }
        private int LoginAttempts { get; set; } = 3;
        private bool IsLocked { get; set; } = false;

        public LoginManager()
        {
            Users = new();
        }

        public LoginStatus Login(string userName, string password)
        {
            Users.ForEach(user =>
            {
                if (user.ExistingUser(userName))
                {
                    return true;
                }
            });


            return LoginStatus.Failed;
        }
    }
}
