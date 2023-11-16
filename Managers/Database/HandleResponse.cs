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
            JsonSettings = new JsonSerializerSettings
            {
                Converters = { new UserTypeConverter() }
            };
        }

        internal List<User> GetUserListFromResponse()
        {

            var convertedUserList = JsonConvert.DeserializeObject<List<User>>(JsonResponse, JsonSettings);

            return convertedUserList;
        }
        internal User GetUserFromResponse()
        {
            var convertedUser = JsonConvert.DeserializeObject<User>(JsonResponse, JsonSettings);

            return convertedUser;
        }
    }
}
