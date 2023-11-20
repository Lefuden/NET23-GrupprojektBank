//using NET23_GrupprojektBank.Managers.Logs;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace NET23_GrupprojektBank.Managers.Database
//{
//    internal class LogConverter : JsonConverter<Log>
//    {
//        public override bool CanWrite => false;


//        public override Log ReadJson(JsonReader reader, Type objectType, Log existingValue, bool hasExistingValue, JsonSerializer serializer)
//        {

//            JObject jsonObject = JObject.Load(reader);

//            // Extract values

//            LogType logType = jsonObject["logType"].ToObject<LogType>();
//            switch (logType)
//            {
//                case LogType.Admin:
//                    return jsonObject.ToObject<AdminLog>(serializer);

//                case LogType.Customer:
//                    return jsonObject.ToObject<CustomerLog>(serializer);

//                default:
//                    throw new JsonSerializationException($"Unknown LogType: {logType}");
//            }
//        }

//        public override void WriteJson(JsonWriter writer, Log? value, JsonSerializer serializer)
//        {
//            throw new NotImplementedException();
//        }

//    }
//}
