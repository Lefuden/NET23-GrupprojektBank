using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Users;
using Newtonsoft.Json;

namespace NET23_GrupprojektBank.Managers.Database
{
    internal class HandleResponse
    {
        public string JsonResponse { get; set; }
        public JsonSerializerSettings JsonSettings { get; set; }

        internal HandleResponse(string jsonResponse, JsonSerializerSettings jsonSettings)
        {
            JsonResponse = jsonResponse;
            JsonSettings = jsonSettings;
        }
        internal HandleResponse(string jsonResponse)
        {
            JsonResponse = jsonResponse;

        }

        internal List<User> GetUserListFromResponse()
        {
            JsonSettings = new JsonSerializerSettings
            {
                Converters = { new UserTypeConverter() }
            };

            var convertedUserList = JsonConvert.DeserializeObject<List<User>>(JsonResponse, JsonSettings);

            return convertedUserList;
        }
        internal User GetUserFromResponse()
        {
            JsonSettings = new JsonSerializerSettings
            {
                Converters = { new UserTypeConverter() }
            };

            var convertedUser = JsonConvert.DeserializeObject<User>(JsonResponse, JsonSettings);

            return convertedUser;
        }

        internal List<BankAccount> GetAllUserBankAccountsAsListFromResponse()
        {
            JsonSettings = new JsonSerializerSettings
            {
                Converters = { new BankAccountTypeConverter() }
            };
            var convertedBankAcountList = JsonConvert.DeserializeObject<List<BankAccount>>(JsonResponse, JsonSettings);

            return convertedBankAcountList;
        }
        internal BankAccount GetBankAccountFromResponse()
        {
            JsonSettings = new JsonSerializerSettings
            {
                Converters = { new BankAccountTypeConverter() }
            };
            var convertedBankAcount = JsonConvert.DeserializeObject<BankAccount>(JsonResponse, JsonSettings);

            return convertedBankAcount;
        }
    }
}
