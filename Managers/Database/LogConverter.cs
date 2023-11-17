//using NET23_GrupprojektBank.Managers.Logs;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace NET23_GrupprojektBank.Managers.Database
//{
//    internal class LogConverter : JsonConverter<Log>
//    {
//        public override void WriteJson(JsonWriter writer, Log value, JsonSerializer serializer)
//        {
//            try
//            {
//                // Add debugging output
//                Console.WriteLine("Serializing Log...");

//                var jsonObject = JObject.FromObject(value);
//                jsonObject.WriteTo(writer);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Serialization error: {ex.Message}");
//                throw;
//            }
//            //var jsonObject = JObject.FromObject(value);
//            //if (value.User != null)
//            //{

//            //    jsonObject.Add("User", JToken.FromObject(value.User));
//            //}
//            //jsonObject.WriteTo(writer);
//        }

//        public override Log ReadJson(JsonReader reader, Type objectType, Log existingValue, bool hasExistingValue, JsonSerializer serializer)
//        {
//            throw new NotImplementedException(); // Implement this method if needed for deserialization
//        }

//        public override bool CanRead => false; // Adjust based on your requirements

//    }
//}
