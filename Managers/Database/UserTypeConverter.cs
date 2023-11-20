using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Managers.Logs;
using NET23_GrupprojektBank.Users;
using NET23_GrupprojektBank.Users.UserInformation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NET23_GrupprojektBank.Managers.Database
{
    internal class UserTypeConverter : JsonConverter<User>
    {
        public override bool CanWrite => false;


        public override User ReadJson(JsonReader reader, Type objectType, User existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            // Extract values
            string userName = jsonObject["userName"]?.ToString();
            //string userId = jsonObject["userId"]?.ToString();
            Guid userId = Guid.Parse(jsonObject["userId"]?.ToString());
            string salt = jsonObject["salt"]?.ToString();
            string hashedPassword = jsonObject["hashedPassword"]?.ToString();  // Assuming you want the hashed password
            JObject personInformationJson = jsonObject["personInformation"] as JObject;
            JObject? bankAccountsJson = jsonObject["bankAccounts"] as JObject;
            JObject? logsJson = jsonObject["logs"] as JObject;

            // Deserialize personInformationJson if it's not null
            PersonInformation personInformation = personInformationJson != null ? personInformationJson.ToObject<PersonInformation>() : null;
            List<BankAccount> bankAccounts = bankAccountsJson != null ? bankAccountsJson.ToObject<List<BankAccount>>() : null;
            // Determine user type based on UserType enum
            UserType userType = (UserType)Enum.Parse(typeof(UserType), jsonObject["userType"]?.ToString() ?? UserType.Undeclared.ToString());

            // Create the appropriate user type based on UserType
            User user;
            switch (userType)
            {
                case UserType.Customer:
                    user = new Customer(userName, userId, salt, hashedPassword, personInformation, userType, logs, bankAccounts);
                    break;
                case UserType.Admin:
                    user = new Admin(userName, userId, salt, hashedPassword, personInformation, userType, logs);
                    break;
                default:
                    throw new NotSupportedException($"Unsupported UserType: {userType}");
            }

            serializer.Populate(jsonObject.CreateReader(), user);
            return user;
        }

        public override void WriteJson(JsonWriter writer, User? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
