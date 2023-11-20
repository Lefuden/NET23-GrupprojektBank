using NET23_GrupprojektBank.Managers.UserInteraction;
using NET23_GrupprojektBank.Users;

namespace NET23_GrupprojektBank.Managers.Login
{
    internal class LoginManager
    {
        private List<User> CurrentExistingUsers { get; set; }
        private int RemainingLoginAttempts { get; set; } = 3;
        private bool IsLocked { get; set; } = false;
        private const int LockoutDuration = 6;
        private DateTime LockoutTimeStart { get; set; } = DateTime.MinValue;


        public LoginManager(List<User> listOfUsers, int maxAttempts = 3)
        {
            CurrentExistingUsers = listOfUsers;
        }

        public (DateTime LockOutTimeStarted, int LockOutDuration) GetLockOutInformation() => IsLocked ? (LockoutTimeStart, LockoutDuration) : (DateTime.MaxValue, 60000000);

        public (User? User, EventStatus EventStatus) Login(string Username, string password)
        {
            if (IsLocked)
            {
                LockoutTimeStart = DateTime.UtcNow;
                return (default, UserCommunications.DisplayLockoutScreenASCII(LockoutTimeStart, LockoutDuration));
            }

            RemainingLoginAttempts--;

            if (RemainingLoginAttempts <= 0 && !IsLocked)
            {
                IsLocked = true;
                LockoutTimeStart = DateTime.UtcNow;
                return (default, UserCommunications.DisplayLockoutScreenASCII(LockoutTimeStart, LockoutDuration));
            }

            User? userLogin = CurrentExistingUsers.Find(user =>
            {
                if (user.CompareUsername(Username))
                {
                    if (user.CompareUserPassword(password))
                    {
                        return true;
                    }
                }
                return false;
            });

            if (userLogin is not null)
            {
                RemainingLoginAttempts = 3;
                return (userLogin, EventStatus.LoginSuccess);
            }


            return (default, EventStatus.LoginFailed);
        }

        public void ResetLoginLockout()
        {
            RemainingLoginAttempts = 3;
            IsLocked = false;
        }
    }
}
