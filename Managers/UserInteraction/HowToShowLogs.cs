using NET23_GrupprojektBank.Managers.Logs;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal static class HowToShowLogs
    {
        public static void ShowLogs(List<Log> logs)
        {
            foreach (var log in logs)
            {
                Console.WriteLine(log);
            }
        }
    }
}
