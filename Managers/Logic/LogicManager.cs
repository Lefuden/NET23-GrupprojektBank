using NET23_GrupprojektBank.Managers.Login;
using NET23_GrupprojektBank.Managers.Transactions;
using NET23_GrupprojektBank.Managers.UserInteraction;

namespace NET23_GrupprojektBank.Managers.Logic
{
    internal class LogicManager
    {
        private LoginManager LoginManager { get; set; }
        private TransactionsManager TransactionsManager { get; set; }
        private UserCommunications UserCommunication { get; set; }

        public LogicManager()
        {
            LoginManager = new();
            TransactionsManager = new();
            UserCommunication = new();
        }

        public void GetUserChoice()
        {

        }
    }
}
