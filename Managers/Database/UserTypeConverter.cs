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
            string password = jsonObject["hashedPassword"]?.ToString();  // Assuming you want the hashed password
            JObject personInformation = jsonObject["personInformation"] as JObject;

            // Deserialize personInformation if it's not null
            PersonInformation person = personInformation != null ? personInformation.ToObject<PersonInformation>() : null;

            // Determine user type based on UserType enum
            UserType userType = (UserType)Enum.Parse(typeof(UserType), jsonObject["userType"]?.ToString() ?? UserType.Undeclared.ToString());

            // Create the appropriate user type based on UserType
            User user;
            switch (userType)
            {
                case UserType.Customer:
                    user = new Customer(userName, password, person);
                    break;
                case UserType.Admin:
                    user = new Admin(userName, password, person);
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
