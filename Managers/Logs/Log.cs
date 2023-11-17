using NET23_GrupprojektBank.Users;
using Newtonsoft.Json;

namespace NET23_GrupprojektBank.Managers.Logs
{
    //[JsonConverter(typeof(LogConverter))]
    internal class Log
    {
        [JsonIgnore]
        public int LogId { get; set; }
        [JsonProperty]
        public DateTime DateAndTime { get; set; }
        [JsonProperty]
        public Guid OwnerUserId { get; set; }
        [JsonProperty]
        public string Message { get; set; }

        [JsonConstructor]
        public Log(int logId, DateTime dateAndTime, Guid ownerUserId, string message)
        {
            LogId = logId;
            DateAndTime = dateAndTime;
            OwnerUserId = ownerUserId;
            Message = message;
        }
        public Log(DateTime dateAndTime, Guid ownerUserId, string message)
        {
            DateAndTime = dateAndTime;
            OwnerUserId = ownerUserId;
            Message = message;
        }
        public Log(DateTime dateAndTime, User user, string message)
        {
            DateAndTime = dateAndTime;
            OwnerUserId = user.GetUserId();
            Message = message;
        }
    }
}
