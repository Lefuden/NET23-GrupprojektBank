using NET23_GrupprojektBank.Users;

namespace NET23_GrupprojektBank.Managers.Login
{
    internal class LoginManager
    {
        private List<User> Users { get; set; }
        private int LoginAttempts { get; set; } = 3;
        private bool IsLocked { get; set; } = false;
        public LoginManager()
        {
            Users = new();
        }
        public (User User, EventStatus EventStatus) Login(string userName, string password)
        {
            var userLogin = ((User)default, EventStatus.LoginFailed);

            if (LoginAttempts <= 0 && !IsLocked)
            {
                IsLocked = true;
                userLogin = ((User)default, EventStatus.LoginLocked);
                return userLogin;
            }

            LoginAttempts--;

            Users.ForEach(user =>
            {
                if (user.CompareUserName(userName))
                {
                    if (user.CompareUserPassword(password))
                    {
                        userLogin = (user, EventStatus.LoginSuccess);
                    }
                }
            });

            return userLogin;
        }
    }
}
