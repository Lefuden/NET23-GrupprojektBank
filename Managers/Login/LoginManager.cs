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
                //return (default, UserCommunications.DisplayLockoutScreenASCII(LockoutTimeStart, LockoutDuration));
            }

            RemainingLoginAttempts--;

            if (RemainingLoginAttempts <= 0 && !IsLocked)
            {
                IsLocked = true;
                LockoutTimeStart = DateTime.UtcNow;
                //return (default, UserCommunications.DisplayLockoutScreenASCII(LockoutTimeStart, LockoutDuration));
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

        //private static Color UpdateColorBasedOnTimeRemaining(int timeRemaining)
        //{
        //    return timeRemaining switch
        //    {
        //        > 30 => Color.Red,
        //        > 20 => Color.Orange1,
        //        > 10 => Color.Gold1,
        //        > 3 => Color.GreenYellow,
        //        <= 3 => Color.Green,
        //    }; ;
        //}

        //public static EventStatus DisplayLockoutScreenASCII(DateTime lockoutTimeStart, int lockoutDuration)
        //{

        //    while (DateTime.UtcNow.Subtract(lockoutTimeStart).TotalSeconds < lockoutDuration)
        //    {
        //        int remainingTime = lockoutDuration - (int)DateTime.UtcNow.Subtract(lockoutTimeStart).TotalSeconds;
        //        Console.CursorVisible = false;

        //        Console.Clear();
        //        Color timeRemainingColor = UpdateColorBasedOnTimeRemaining(remainingTime);

        //        AnsiConsole.Write(new FigletText("Locked for: ").Centered().Color(timeRemainingColor));
        //        AnsiConsole.Write(new FigletText(remainingTime.ToString()).Centered().Color(timeRemainingColor));


        //        Thread.Sleep(1000);
        //    }
        //    return EventStatus.LoginUnlocked;
        //}




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
