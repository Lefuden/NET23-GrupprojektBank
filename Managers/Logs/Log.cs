namespace NET23_GrupprojektBank.Managers.Logs
{
    //[JsonConverter(typeof(LogConverter))]
    internal class Log
    {
        public int LogId { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Message { get; set; }
        private static int LogIdReference { get; set; } = 0;

        public Log(DateTime dateAndTime, string message)
        {
            LogId = LogIdReference++;
            DateAndTime = dateAndTime;
            Message = message;
        }
        public override string ToString()
        {
            return $"{LogId} - {Message} - {DateAndTime}";
        }
    }
}
