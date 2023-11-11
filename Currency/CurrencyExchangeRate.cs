using NET23_GrupprojektBank.Users;

namespace NET23_GrupprojektBank.Currency
{
    internal static class CurrencyExchangeRate
    {
        private static Dictionary<CurrencyType, decimal> CurrentExchangeRates { get; set; }
        public static void UpdateCurrencyExchange(UserType userType)
        {
            if (userType is not UserType.Admin)
            {
                return;
            }

            // Update the Currency Exchange Rate here

        }

        public static Dictionary<CurrencyType, decimal> GetCurrentCurrencyExchangeRate() => CurrentExchangeRates;
    }
}
