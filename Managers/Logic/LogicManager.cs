using NET23_GrupprojektBank.Managers.Login;
using NET23_GrupprojektBank.Managers.Transactions;

namespace NET23_GrupprojektBank.Managers.Logic
{
    internal class LogicManager
    {
        private LoginManager LoginManager { get; set; }
        private TransactionsManager TransactionsManager { get; set; }

        public LogicManager()
        {
            LoginManager = new();
            TransactionsManager = new();
        }

        public void GetUserChoice()
        {

        }
    }
}
