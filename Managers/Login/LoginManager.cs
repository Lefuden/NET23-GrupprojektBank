using NET23_GrupprojektBank.Users;

namespace NET23_GrupprojektBank.Managers.Login
{
    internal class LoginManager
    {
        private List<User> Users { get; set; }
        private int LoginAttempts { get; set; } = 3;
        private bool IsLocked { get; set; } = false;
        private EventStatus EventStatus { get; set; }

        public LoginManager()
        {
            Users = new();
        }

        public (User User, EventStatus EventStatus) Login(string userName, string password)
        {
            var userLogin = (default, EventStatus.Failed);

            if (LoginAttempts <= 0 && !IsLocked)
            {
                IsLocked = true;
                userLogin = (default, EventStatus.Locked);
                return userLogin;
            }

            LoginAttempts--;

            Users.ForEach(user =>
            {
                if (user.CompareUserName(userName))
                {
                    if (user.ComparePassword(password))
                    {
                        userLogin = (user, EventStatus.Success);
                    }
                }
            });

            return userLogin;
        }
    }
}
