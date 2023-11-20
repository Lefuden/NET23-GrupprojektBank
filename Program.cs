using NET23_GrupprojektBank.Application;
namespace NET23_GrupprojektBank
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            App HyperHedgehogsBank = new();
            await HyperHedgehogsBank.Run();
        }
    }
}