using NET23_GrupprojektBank.Managers.Logic;
using NET23_GrupprojektBank.Managers.UserInteraction;

namespace NET23_GrupprojektBank.Application
{
    internal class App
    {
        private LogicManager LogicManager { get; set; }
        public App()
        {
            LogicManager = new();
        }

        public void Run()
        {
            BankLoggo.StartUpAppLoadingScreen();
            LogicManager.GetUserChoice();
        }
    }
}
