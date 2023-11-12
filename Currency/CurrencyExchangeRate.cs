using NET23_GrupprojektBank.Currency.DTO;
using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Managers.UserInteraction;
using NET23_GrupprojektBank.Users;
using Newtonsoft.Json;
using System.Reflection;

namespace NET23_GrupprojektBank.Currency
{
    internal static class CurrencyExchangeRate
    {
        private static Dictionary<CurrencyType, double> CurrentExchangeRatesSEK { get; set; } = new();
        private static Dictionary<CurrencyType, double> CurrentExchangeRatesUSD { get; set; } = new();
        private static Dictionary<CurrencyType, double> CurrentExchangeRatesEUR { get; set; } = new();
        private const int AmountOfSupportedCurrencyTypes = 3;
        private const string SupportedCurrencyTypes = "SEK,USD,EUR";
        private const string SupportedCurrencyFilePaths = "SEKResponse.txt,USDResponse.txt,EURResponse.txt";

        private static string SEKResponse { get; set; }
        private static string USDResponse { get; set; }
        private static string EURResponse { get; set; }
        public static DTOForCurrencyExchangeRateAPI SEKCurrencyRate { get; set; } = new();
        public static DTOForCurrencyExchangeRateAPI USDCurrencyRate { get; set; } = new();
        public static DTOForCurrencyExchangeRateAPI EURCurrencyRate { get; set; } = new();
        public static async Task<EventStatus> UpdateCurrencyExchangeRateAsync(UserType userType)
        {
            if (userType is not UserType.Admin)
            {
                return EventStatus.NonAdminUser;
            }
            SEKCurrencyRate ??= new DTOForCurrencyExchangeRateAPI();
            USDCurrencyRate ??= new DTOForCurrencyExchangeRateAPI();
            EURCurrencyRate ??= new DTOForCurrencyExchangeRateAPI();


            int choice = HowToUpdateCurrency.AdminUpdateCurrencyOption();
            switch (choice)
            {
                case 0:
                    return await UpdateCurrencyExchangeRateAsync(userType);
                case 1:
                    UpdateFromFile();
                    UpdateDictionaries();
                    return EventStatus.AdminUpdatedCurrencyFromFile;
                case 2:
                    await UpdateFromTheWeb();
                    UpdateDictionaries();
                    return EventStatus.AdminUpdatedCurrencyFromWebApi;
                default:
                    return EventStatus.AdminInvalidInput;
            }



        }
        private static async Task UpdateFromTheWeb()
        {
            Console.Write("Enter your api key: ");
            string apiKey = Console.ReadLine();
            string baseApiUrl = "https://v6.exchangerate-api.com/v6";

            // Option for the admin to choose what the main currency exchange rate is based on our CurrencyType Enum > 
            // Converting Enum Value ToString().ToUpper() since the API request requires all caps
            string[] currencyTypes = SupportedCurrencyTypes.Split(',');
            string[] filePaths = SupportedCurrencyFilePaths.Split(',');
            for (int i = 0; i < AmountOfSupportedCurrencyTypes; i++)
            {

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync($"{baseApiUrl}/{apiKey}/latest/{currencyTypes[i]}");
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        File.WriteAllText(filePaths[i], responseBody);
                        Console.WriteLine($"({i + 1}/{filePaths.Length}) Got the latest currency exchange rates from the API and saved it to file: {filePaths[i]}");

                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Error accessing the API: {ex.Message}");
                    }
                }
            }
        }
        private static void UpdateFromFile()
        {
            Console.WriteLine($"{SEKCurrencyRate is null} - {USDCurrencyRate is null} - {EURCurrencyRate is null}");
            SEKCurrencyRate ??= new DTOForCurrencyExchangeRateAPI();
            USDCurrencyRate ??= new DTOForCurrencyExchangeRateAPI();
            EURCurrencyRate ??= new DTOForCurrencyExchangeRateAPI();

            UpdateDictionaries();
        }
        private static void UpdateDictionaries()
        {
            Console.WriteLine("Updating the dictionaries...");
            bool SEKUpdatedFromFile = false, USDUpdatedFromFile = false, EURUpdatedFromFile = false;
            string[] filePaths = SupportedCurrencyFilePaths.Split(',');
            string SEKPath = filePaths[0];
            string USDPath = filePaths[1];
            string EURPath = filePaths[2];

            if (File.Exists(SEKPath))
            {
                Console.WriteLine(SEKPath + " Existing File, creating object");
                SEKResponse = File.ReadAllText(SEKPath);
                SEKCurrencyRate = JsonConvert.DeserializeObject<DTOForCurrencyExchangeRateAPI>(SEKResponse);
                SEKUpdatedFromFile = true;

                Type type = SEKCurrencyRate.conversion_rates.GetType();
                PropertyInfo[] properties = type.GetProperties();
                Console.WriteLine("SEK Dictionary: ");
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(double))
                    {
                        // Get the property name and value
                        string propertyName = property.Name;
                        double propertyValue = (double)property.GetValue(SEKCurrencyRate.conversion_rates);
                        CurrencyType currencyType = Enum.Parse<CurrencyType>(propertyName, false);
                        CurrentExchangeRatesSEK[currencyType] = propertyValue;
                        // Print the property name and value
                        Console.WriteLine($"\t{propertyName}: {propertyValue}");
                    }
                }
            }
            if (File.Exists(USDPath))
            {
                Console.WriteLine(USDPath + " Existing File, creating object");
                USDResponse = File.ReadAllText(USDPath);
                USDCurrencyRate = JsonConvert.DeserializeObject<DTOForCurrencyExchangeRateAPI>(USDResponse);
                USDUpdatedFromFile = true;

                Type type = USDCurrencyRate.conversion_rates.GetType();
                PropertyInfo[] properties = type.GetProperties();
                Console.WriteLine("USD Dictionary: ");
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(double))
                    {
                        // Get the property name and value
                        string propertyName = property.Name;
                        double propertyValue = (double)property.GetValue(USDCurrencyRate.conversion_rates);
                        CurrencyType currencyType = Enum.Parse<CurrencyType>(propertyName, false);
                        CurrentExchangeRatesUSD[currencyType] = propertyValue;
                        // Print the property name and value
                        Console.WriteLine($"\t{propertyName}: {propertyValue}");
                    }
                }
            }
            if (File.Exists(EURPath))
            {
                Console.WriteLine(EURPath + " Existing File, creating object");
                EURResponse = File.ReadAllText(EURPath);
                EURCurrencyRate = JsonConvert.DeserializeObject<DTOForCurrencyExchangeRateAPI>(EURResponse);
                EURUpdatedFromFile = true;

                Type type = EURCurrencyRate.conversion_rates.GetType();
                PropertyInfo[] properties = type.GetProperties();
                Console.WriteLine("EUR Dictionary: ");
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(double))
                    {
                        // Get the property name and value
                        string propertyName = property.Name;
                        double propertyValue = (double)property.GetValue(EURCurrencyRate.conversion_rates);
                        CurrencyType currencyType = Enum.Parse<CurrencyType>(propertyName, false);
                        CurrentExchangeRatesEUR[currencyType] = propertyValue;
                        // Print the property name and value
                        Console.WriteLine($"\t{propertyName}: {propertyValue}");
                    }
                }
            }


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

        public static Dictionary<CurrencyType, double> GetCurrentCurrencyExchangeRate(CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.SEK:
                    return CurrentExchangeRatesSEK;
                case CurrencyType.USD:
                    return CurrentExchangeRatesUSD;
                case CurrencyType.EUR:
                    return CurrentExchangeRatesEUR;
                default:
                    return CurrentExchangeRatesSEK;
            }
        }
    }
}
