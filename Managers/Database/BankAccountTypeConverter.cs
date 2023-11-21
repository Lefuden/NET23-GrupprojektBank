//using NET23_GrupprojektBank.BankAccounts;
//using NET23_GrupprojektBank.Currency;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace NET23_GrupprojektBank.Managers.Database
//{
//    internal class BankAccountTypeConverter : JsonConverter<BankAccount>
//    {
//        public override bool CanWrite => false;

//        public override BankAccount? ReadJson(JsonReader reader, Type objectType, BankAccount? existingValue, bool hasExistingValue, JsonSerializer serializer)
//        {
//            JObject jsonObject = JObject.Load(reader);

//            // Extract values

//            int bankAccountNumber = jsonObject["bankAccountNumber"].Value<int>();
//            string bankAccountName = jsonObject["bankAccountName"]?.ToString();
//            BankAccountType bankAccountType = (BankAccountType)Enum.Parse(typeof(BankAccountType), jsonObject["bankAccountType"]?.ToString() ?? BankAccountType.Undeclared.ToString());
//            CurrencyType currencyType = (CurrencyType)Enum.Parse(typeof(CurrencyType), jsonObject["currencyType"]?.ToString() ?? CurrencyType.SEK.ToString());
//            decimal balance = jsonObject["balance"].Value<decimal>();
//            Guid ownerUserId = Guid.Parse(jsonObject["userId"]?.ToString());

//            // Create the appropriate bank account type based on BankAccountType
//            BankAccount bankAccount;
//            switch (bankAccountType)
//            {
//                case BankAccountType.Checking:
//                    bankAccount = new Checking(bankAccountNumber, bankAccountName, bankAccountType, currencyType, balance, ownerUserId);
//                    break;
//                case BankAccountType.Savings:
//                    double interest = jsonObject["interest"].Value<double>();
//                    bankAccount = new Savings(bankAccountNumber, bankAccountName, bankAccountType, currencyType, balance, ownerUserId, interest);
//                    break;
//                default:
//                    throw new NotSupportedException($"Unsupported BankAccountType: {bankAccountType}");
//            }

//            serializer.Populate(jsonObject.CreateReader(), bankAccount);
//            return bankAccount;
//        }

//        public override void WriteJson(JsonWriter writer, BankAccount? value, JsonSerializer serializer)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}