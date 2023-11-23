using NET23_GrupprojektBank.Managers.UserInteraction;
using NET23_GrupprojektBank.Users;

namespace NET23_GrupprojektBank.Managers.Login
{
    internal class LoginManager
    {
        private List<User> CurrentExistingUsers { get; set; }
        private int MaxLoginAttempts { get; set; } = 3;
        private bool IsLocked { get; set; } = false;
        private const int LockoutDuration = 6;
        private DateTime LockoutTimeStart { get; set; } = DateTime.MinValue;


        public LoginManager(List<User> listOfUsers)
        {
            CurrentExistingUsers = listOfUsers;
        }

        public (DateTime LockOutTimeStarted, int LockOutDuration) GetLockOutInformation() => IsLocked ? (LockoutTimeStart, LockoutDuration) : (DateTime.MaxValue, 60000000);

        public (User? User, EventStatus EventStatus) Login(string username, string password, int attempts)
        {
            if (attempts == 1 && IsLocked)
            {
                IsLocked = false;
            }
            if (MaxLoginAttempts == attempts && !IsLocked)
            {
                IsLocked = true;
            }

            User? userLogin = CurrentExistingUsers.Find(user => user.CompareUsername(username) && user.CompareUserPassword(password));


            if (userLogin is not null)
            {
                IsLocked = false;
                return (userLogin, EventStatus.LoginSuccess);
            }
            else if (IsLocked)
            {
                LockoutTimeStart = DateTime.UtcNow;
                return (default, UserCommunications.DisplayLockoutScreenASCII(LockoutTimeStart, LockoutDuration));
            }

            return (default, EventStatus.LoginFailed);
        }
    }
}
