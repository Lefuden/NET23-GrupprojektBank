using NET23_GrupprojektBank.Application;
namespace NET23_GrupprojektBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            App HyperHedgehogsBank = new();
            HyperHedgehogsBank.Run();
        }
    }
}