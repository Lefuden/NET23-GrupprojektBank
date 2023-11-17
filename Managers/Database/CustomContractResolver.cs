//using NET23_GrupprojektBank.Managers.Logs;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using System.Reflection;

//namespace NET23_GrupprojektBank.Managers.Database
//{
//    internal class CustomContractResolver : DefaultContractResolver
//    {
//        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
//        {
//            JsonProperty property = base.CreateProperty(member, memberSerialization);

//            // Conditionally ignore User property
//            if (property.DeclaringType == typeof(Log) && property.PropertyName == "User")
//            {
//                property.ShouldSerialize = instance => (instance as Log)?.User != null;
//            }

//            return property;
//        }
//    }
//}
