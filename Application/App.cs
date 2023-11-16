using NET23_GrupprojektBank.Managers.Logic;

namespace NET23_GrupprojektBank.Application
{
    internal class App
    {
        private LogicManager LogicManager { get; set; }

        public App()
        {
            bool isUsingDatabase = true;
            LogicManager = new(isUsingDatabase);
        }

        public void Run()
        {
            LogicManager.GetUserChoice();
        }
    }
}
