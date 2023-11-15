using NET23_GrupprojektBank.Users;

namespace NET23_GrupprojektBank.Managers.Logs
{
    internal class Log
    {
        public DateTime DateAndTime { get; set; }
        public User? User { get; set; }
        public string Message { get; set; }

        public Log(DateTime dateAndTime, User user, string message)
        {
            DateAndTime = dateAndTime;
            User = user;
            Message = message;
        }
        public Log(User user, string message)
        {
            DateAndTime = DateTime.UtcNow;
            User = user;
            Message = message;
        }
        public Log(DateTime dateAndTime, string message)
        {
            DateAndTime = dateAndTime;
            User = default;
            Message = message;
        }
        public Log(string message)
        {
            DateAndTime = DateTime.UtcNow;
            User = default;
            Message = message;
        }
    }
}
