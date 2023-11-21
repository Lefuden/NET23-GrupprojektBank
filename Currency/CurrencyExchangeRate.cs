using NET23_GrupprojektBank.Currency.DTO;
using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Managers.Logic;
using NET23_GrupprojektBank.Managers.UserInteraction;
using NET23_GrupprojektBank.Users;
using Newtonsoft.Json;
using Spectre.Console;
using System.Reflection;

namespace NET23_GrupprojektBank.Currency
{
    internal static class CurrencyExchangeRate
    {
        private static Dictionary<CurrencyType, double> CurrentExchangeRatesSEK { get; set; } = new();
        private static Dictionary<CurrencyType, double> CurrentExchangeRatesUSD { get; set; } = new();
        private static Dictionary<CurrencyType, double> CurrentExchangeRatesEUR { get; set; } = new();
        private const string SupportedCurrencyTypes = "SEK,USD,EUR";
        private const string SupportedCurrencyFilePaths = "SEKResponse.txt,USDResponse.txt,EURResponse.txt";
        private static bool _isUpdatedFromFile = false;
        private static string SEKResponse { get; set; }
        private static string USDResponse { get; set; }
        private static string EURResponse { get; set; }
        public static DTOForCurrencyExchangeRateAPI SEKCurrencyRate { get; set; } = new();
        public static DTOForCurrencyExchangeRateAPI USDCurrencyRate { get; set; } = new();
        public static DTOForCurrencyExchangeRateAPI EURCurrencyRate { get; set; } = new();
        public static async Task<EventStatus> UpdateCurrencyExchangeRateAsync(UserType userType, bool startup = false)
        {
            if (userType is not UserType.Admin)
            {
                return EventStatus.NonAdminUser;
            }
            SEKCurrencyRate ??= new DTOForCurrencyExchangeRateAPI();
            USDCurrencyRate ??= new DTOForCurrencyExchangeRateAPI();
            EURCurrencyRate ??= new DTOForCurrencyExchangeRateAPI();
            if (startup)
            {
                UpdateFromFile();
                UpdateDictionaries();
                return EventStatus.AdminUpdatedCurrencyFromFile;
            }
            UserChoice choice = UserCommunications.AdminUpdateCurrencyOption();
            switch (choice)
            {
                case UserChoice.Back:
                    return EventStatus.CurrencyExchangeRateUpdateFailed;

                case UserChoice.FromFile:
                    UpdateFromFile();
                    UpdateDictionaries();
                    return EventStatus.AdminUpdatedCurrencyFromFile;
                case UserChoice.FromInternet:
                    await UpdateFromTheWeb();
                    UpdateDictionaries();
                    return EventStatus.AdminUpdatedCurrencyFromWebApi;
                default:
                    return EventStatus.AdminInvalidInput;
            }
        }
        private static async Task UpdateFromTheWeb()
        {
            string apiKey = AnsiConsole.Ask<string>("Enter your api key: ");
            string baseApiUrl = "https://v6.exchangerate-api.com/v6";


            string[] currencyTypes = SupportedCurrencyTypes.Split(',');
            string[] filePaths = SupportedCurrencyFilePaths.Split(',');
            for (int i = 0; i < currencyTypes.Length; i++)
            {

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync($"{baseApiUrl}/{apiKey}/latest/{currencyTypes[i]}");
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        File.WriteAllText(filePaths[i], responseBody);
                    }
                    catch (HttpRequestException ex)
                    {
                        AnsiConsole.WriteException(ex);
                    }
                }
            }
        }
        private static void UpdateFromFile()
        {
            SEKCurrencyRate ??= new DTOForCurrencyExchangeRateAPI();
            USDCurrencyRate ??= new DTOForCurrencyExchangeRateAPI();
            EURCurrencyRate ??= new DTOForCurrencyExchangeRateAPI();

            UpdateDictionaries();
        }
        private static void UpdateDictionaries()
        {
            bool SEKUpdatedFromFile = false, USDUpdatedFromFile = false, EURUpdatedFromFile = false;
            string[] filePaths = SupportedCurrencyFilePaths.Split(',');

            for (int i = 0; i < filePaths.Length; i++)
            {
                Type type = null;
                if (File.Exists(filePaths[i]))
                {

                    switch (i)
                    {
                        case 0:
                            SEKResponse = File.ReadAllText(filePaths[i]);
                            SEKCurrencyRate = JsonConvert.DeserializeObject<DTOForCurrencyExchangeRateAPI>(SEKResponse);
                            SEKUpdatedFromFile = true;
                            type = SEKCurrencyRate.conversion_rates.GetType();

                            break;

                        case 1:
                            USDResponse = File.ReadAllText(filePaths[i]);
                            USDCurrencyRate = JsonConvert.DeserializeObject<DTOForCurrencyExchangeRateAPI>(USDResponse);
                            USDUpdatedFromFile = true;
                            type = USDCurrencyRate.conversion_rates.GetType();

                            break;

                        case 2:
                            EURResponse = File.ReadAllText(filePaths[i]);
                            EURCurrencyRate = JsonConvert.DeserializeObject<DTOForCurrencyExchangeRateAPI>(EURResponse);
                            EURUpdatedFromFile = true;
                            type = EURCurrencyRate.conversion_rates.GetType();

                            break;

                        default:
                            return;
                    }
                    PropertyInfo[] properties = type.GetProperties();
                    foreach (var property in properties)
                    {
                        if (property.PropertyType == typeof(double))
                        {
                            string propertyName = property.Name;
                            double propertyValue = (double)property.GetValue(SEKCurrencyRate.conversion_rates);
                            CurrencyTypeFull currencyTypeFull = Enum.Parse<CurrencyTypeFull>(propertyName, false);

                            CurrencyType currencyType = currencyTypeFull switch
                            {
                                CurrencyTypeFull.SEK => CurrencyType.SEK,
                                CurrencyTypeFull.EUR => CurrencyType.EUR,
                                CurrencyTypeFull.USD => CurrencyType.USD,
                                _ => CurrencyType.INVALID
                            };

                            switch (i)
                            {
                                case 0:
                                    CurrentExchangeRatesSEK[currencyType] = propertyValue;
                                    break;

                                case 1:
                                    CurrentExchangeRatesUSD[currencyType] = propertyValue;
                                    break;

                                case 2:
                                    CurrentExchangeRatesEUR[currencyType] = propertyValue;
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
            }


            _isUpdatedFromFile = SEKUpdatedFromFile == USDUpdatedFromFile == EURUpdatedFromFile;

            if (SEKUpdatedFromFile is not true)
            {
                InitializeBaseValuesToADictionary(CurrentExchangeRatesSEK);
            }
            if (USDUpdatedFromFile is not true)
            {
                InitializeBaseValuesToADictionary(CurrentExchangeRatesUSD);
            }
            if (EURUpdatedFromFile is not true)
            {
                InitializeBaseValuesToADictionary(CurrentExchangeRatesEUR);
            }
        }


        private static void InitializeBaseValuesToADictionary(Dictionary<CurrencyType, double> exchangeRateDictionary)
        {
            foreach (CurrencyType currencyType in Enum.GetValues(typeof(CurrencyType)))
            {
                if (exchangeRateDictionary.TryGetValue(currencyType, out double exchangeValue) is not true)
                {
                    exchangeRateDictionary.TryAdd(currencyType, 1);
                }
                else
                {
                    if (exchangeValue != 1)
                    {
                        exchangeRateDictionary[currencyType] = 1;
                    }
                }
            }

        }

        public static Dictionary<CurrencyType, double> GetCurrentCurrencyExchangeRate(CurrencyType dictionaryCurrencyType)
        {
            if (CurrentExchangeRatesSEK.Count <= 0 || CurrentExchangeRatesUSD.Count <= 0 || CurrentExchangeRatesEUR.Count <= 0)
            {
                if (_isUpdatedFromFile is not true)
                {
                    UpdateFromTheWeb().Wait();
                    UpdateDictionaries();
                }
            }

            return dictionaryCurrencyType switch
            {
                CurrencyType.SEK => new(CurrentExchangeRatesSEK),
                CurrencyType.USD => new(CurrentExchangeRatesUSD),
                CurrencyType.EUR => new(CurrentExchangeRatesEUR),
                _ => new(CurrentExchangeRatesSEK)
            };
        }
    }
}



//if (File.Exists(USDPath))
//{
//    USDResponse = File.ReadAllText(USDPath);
//    USDCurrencyRate = JsonConvert.DeserializeObject<DTOForCurrencyExchangeRateAPI>(USDResponse);
//    USDUpdatedFromFile = true;

//    Type type = USDCurrencyRate.conversion_rates.GetType();
//    PropertyInfo[] properties = type.GetProperties();
//    foreach (var property in properties)
//    {
//        if (property.PropertyType == typeof(double))
//        {
//            string propertyName = property.Name;
//            double propertyValue = (double)property.GetValue(USDCurrencyRate.conversion_rates);

//            CurrencyTypeFull currencyTypeFull = Enum.Parse<CurrencyTypeFull>(propertyName, false);

//            CurrencyType currencyType = currencyTypeFull switch
//            {
//                CurrencyTypeFull.SEK => CurrencyType.SEK,
//                CurrencyTypeFull.EUR => CurrencyType.EUR,
//                CurrencyTypeFull.USD => CurrencyType.USD,
//                _ => CurrencyType.INVALID
//            };

//            CurrentExchangeRatesUSD[currencyType] = propertyValue;
//        }
//    }
//}
//if (File.Exists(EURPath))
//{
//    EURResponse = File.ReadAllText(EURPath);
//    EURCurrencyRate = JsonConvert.DeserializeObject<DTOForCurrencyExchangeRateAPI>(EURResponse);
//    EURUpdatedFromFile = true;

//    Type type = EURCurrencyRate.conversion_rates.GetType();
//    PropertyInfo[] properties = type.GetProperties();
//    foreach (var property in properties)
//    {
//        if (property.PropertyType == typeof(double))
//        {
//            string propertyName = property.Name;
//            double propertyValue = (double)property.GetValue(EURCurrencyRate.conversion_rates);
//            CurrencyTypeFull currencyTypeFull = Enum.Parse<CurrencyTypeFull>(propertyName, false);

//            CurrencyType currencyType = currencyTypeFull switch
//            {
//                CurrencyTypeFull.SEK => CurrencyType.SEK,
//                CurrencyTypeFull.EUR => CurrencyType.EUR,
//                CurrencyTypeFull.USD => CurrencyType.USD,
//                _ => CurrencyType.INVALID
//            };
//            CurrentExchangeRatesEUR[currencyType] = propertyValue;
//        }
//    }
//}