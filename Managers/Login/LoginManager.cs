using NET23_GrupprojektBank.Users;

namespace NET23_GrupprojektBank.Managers.Login
{
    internal class LoginManager
    {
        private List<User> CurrentExistingUsers { get; set; }
        private int RemainingLoginAttempts { get; set; } = 3;
        private bool IsLocked { get; set; } = false;
        private const int LockoutDuration = 60;
        private DateTime LockoutTimeStart { get; set; } = DateTime.MinValue;


        public LoginManager(List<User> listOfUsers)
        {
            CurrentExistingUsers = listOfUsers;
        }


        public (User? User, EventStatus EventStatus) Login(string Username, string password)
        {
            if (IsLocked)
            {
                LockoutTimeStart = DateTime.UtcNow;
                DisplayLockoutScreen();
                return (default, EventStatus.LoginLocked);
            }

            RemainingLoginAttempts--;

            if (RemainingLoginAttempts <= 0 && !IsLocked)
            {
                IsLocked = true;
                LockoutTimeStart = DateTime.UtcNow;
                DisplayLockoutScreen();
                return (default, EventStatus.LoginLocked);
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

        private void DisplayLockoutScreen()
        {
            while (DateTime.UtcNow.Subtract(LockoutTimeStart).TotalSeconds < LockoutDuration)
            {
                int remainingTime = LockoutDuration - (int)DateTime.UtcNow.Subtract(LockoutTimeStart).TotalSeconds;
                Console.CursorVisible = false;
                Console.Clear();
                Console.WriteLine($"You are Locked. Remaining time {remainingTime} seconds.");
                Thread.Sleep(1000);
            }
            IsLocked = false;
            RemainingLoginAttempts = 3;
        }
        public bool IsUsernameAlreadyTaken(string Username)
        {
            foreach (var user in CurrentExistingUsers)
            {
                if (user.CompareUsername(Username))
                {
                    return true;
                }
            }

            return false;
        }

        //public int GetRemainingAttempts()
        //{
        //    return RemainingLoginAttempts;
        //}


        //  System Timers Timer Lockout Functionality
        //  Property of the class
        //  private System.Timers.Timer LockedOutTimer { get; set; }
        //  private System.Timers.Timer LockedOutDisplayTimer { get; set; }

        //  Constructor of the Class
        //  SetupTimer(LockedOutTimer, LockedOutDisplayTimer);

        //  In Login when we set IsLocked = true Add these
        //  LockedOutTimer.Enabled = true;
        //  LockedOutDisplayTimer.Enabled = true;
        //private void SetupTimer(System.Timers.Timer lockedTimer, System.Timers.Timer displayToUserTimer)
        //{
        //    lockedTimer = new(6000);
        //    lockedTimer.Elapsed += UnlockAfterTime;
        //    lockedTimer.AutoReset = true;
        //    lockedTimer.Enabled = false;

        //    displayToUserTimer = new(1000);
        //    displayToUserTimer.Elapsed += DisplayCountdownUntilUnlock;
        //    displayToUserTimer.AutoReset = true;
        //    displayToUserTimer.Enabled = false;
        //}
        //private void UnlockAfterTime(object? sender, ElapsedEventArgs e)
        //{
        //    LockedOutTimer.Enabled = false;
        //    LockedOutDisplayTimer.Enabled = false;
        //    IsLocked = false;
        //    RemainingLoginAttempts = 3;
        //}


    }
}
