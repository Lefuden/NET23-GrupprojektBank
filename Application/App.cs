using NET23_GrupprojektBank.Managers.Logic;
using NET23_GrupprojektBank.Managers.Login;
using NET23_GrupprojektBank.Managers.Transactions;
using NET23_GrupprojektBank.Managers.UserInteraction;

namespace NET23_GrupprojektBank.Application
{
    internal class App
    {
        private LoginManager LoginManager { get; set; }
        private TransactionsManager TransactionsManager { get; set; }
        private LogicManager LogicManager { get; set; }
        private UserCommunications UserCommunication { get; set; }

        public App()
        {
            LoginManager = new();
            TransactionsManager = new();
            LogicManager = new();
            UserCommunication = new();
        }

        public void Run()
        {

        }
    }
}
