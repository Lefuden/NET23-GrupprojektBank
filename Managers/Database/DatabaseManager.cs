using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Managers.Logs;
using NET23_GrupprojektBank.Users;
using Newtonsoft.Json;
using Spectre.Console;
using System.Text;

namespace NET23_GrupprojektBank.Managers.Database
{
    internal static class DatabaseManager
    {
        // GET DATA FROM DB

        internal static async Task<List<User>> GetAllUsersFromDB(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{uri}/User";
                var response = await client.GetAsync(url);
                await Console.Out.WriteLineAsync(response.StatusCode.ToString());
                List<User> users = new();
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    HandleResponse handleResponse = new HandleResponse(json);
                    users.AddRange(handleResponse.GetUserListFromResponse());
                    return users;
                }
                else
                {
                    Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
                    return null;
                }
            }
        }
        internal static async Task<User> GetOneUserFromDB(string uri, Guid userId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{uri}/User/{userId}";
                var response = await client.GetAsync(url);
                await Console.Out.WriteLineAsync(response.StatusCode.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    HandleResponse handleResponse = new HandleResponse(json);
                    var user = handleResponse.GetUserFromResponse();
                    return user;
                }
                else
                {
                    Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
                    return null;
                }
            }
        }

        internal static async Task<List<BankAccount>> GetAllUserBankAccountsFromDB(string uri, User user)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{uri}/BankAccount/{user.GetUserId()}";
                var response = await client.GetAsync(url);
                await Console.Out.WriteLineAsync(response.StatusCode.ToString());
                List<BankAccount> bankAccounts = new();
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    HandleResponse handleResponse = new HandleResponse(json);
                    bankAccounts.AddRange(handleResponse.GetAllUserBankAccountsAsListFromResponse());
                    return bankAccounts;
                }
                else
                {
                    Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
                    return null;
                }
            }
        }
        internal static async Task<BankAccount> GetSpecificBankAccountFromDB(string uri, int bankAccountNumber)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{uri}/BankAccount/{bankAccountNumber}";
                var response = await client.GetAsync(url);
                await Console.Out.WriteLineAsync(response.StatusCode.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    HandleResponse handleResponse = new HandleResponse(json);
                    var bankAccount = handleResponse.GetBankAccountFromResponse();
                    return bankAccount;
                }
                else
                {
                    Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
                    return null;
                }
            }
        }

        internal static async Task<List<Log>> GetAllUserLogsFromDB(string uri, User user)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{uri}/Log/owner/{user.GetUserId()}";
                var response = await client.GetAsync(url);
                await Console.Out.WriteLineAsync(response.StatusCode.ToString());
                List<Log> users = new();

                // HÄR ÄR JAG HALLPÅ!!!!












                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    HandleResponse handleResponse = new HandleResponse(json);
                    users.AddRange(handleResponse.GetUserListFromResponse());
                    return users;
                }
                else
                {
                    Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
                    return null;
                }
            }
        }
        // ADD DATA TO DB

        internal static async Task AddSpecificUserToDB(string apiUrl, User user)
        {
            AnsiConsole.Write(new FigletText($"Adding: {user.GetUsername()} to DB"));
            using (HttpClient client = new HttpClient())
            {
                string url = $"{apiUrl}/User";
                // Serialize bankAccount object to JSON
                string jsonUser = JsonConvert.SerializeObject(user, Formatting.Indented);

                // Create StringContent with JSON data
                StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                // Send POST request
                HttpResponseMessage response = await client.PostAsync(url, content);
                await Console.Out.WriteLineAsync(jsonUser);
                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"User added successfully.");
                }
                else
                {
                    Console.WriteLine($"Error adding user. Status code: {response.StatusCode}");
                }

                Console.ReadKey();
            }
        }

        internal static async Task AddLogToDB(string uri, Log log)
        {
            AnsiConsole.Write(new FigletText($"Adding log to DB"));
            using (HttpClient client = new HttpClient())
            {
                string url = $"{uri}/Log";
                // Serialize Log object to JSON
                string jsonLog = JsonConvert.SerializeObject(log, Formatting.Indented);

                // Create StringContent with JSON data
                StringContent content = new StringContent(jsonLog, Encoding.UTF8, "application/json");

                // Send POST request
                HttpResponseMessage response = await client.PostAsync(url, content);
                await Console.Out.WriteLineAsync(jsonLog);
                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"User added successfully.");
                }
                else
                {
                    Console.WriteLine($"Error adding user. Status code: {response.StatusCode}");
                }
            }
        }
        internal static async Task AddBankAccountToDB(string uri, BankAccount bankAccount)
        {
            AnsiConsole.Write(new FigletText($"Adding bank account to DB"));
            using (HttpClient client = new HttpClient())
            {
                string url = $"{uri}/BankAccount";
                // Serialize BankAccount object to JSON
                string jsonBankAccount = JsonConvert.SerializeObject(bankAccount, Formatting.Indented);

                // Create StringContent with JSON data
                StringContent content = new StringContent(jsonBankAccount, Encoding.UTF8, "application/json");

                // Send POST request
                HttpResponseMessage response = await client.PostAsync(url, content);
                await Console.Out.WriteLineAsync(jsonBankAccount);
                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"User added successfully.");
                }
                else
                {
                    Console.WriteLine($"Error adding user. Status code: {response.StatusCode}");
                }
            }

        }

        // UPDATE DATA IN DB

        internal static async Task UpdateSpecificUserInDB(string uri, User user)
        {
            AnsiConsole.Write(new FigletText($"Updating {user.GetUsername()}"));
            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Construct the URL for the specific bankAccount
                string url = $"{uri}/User/{user.GetUserId()}";


                string jsonUser = JsonConvert.SerializeObject(user, Formatting.Indented);

                // Create a StringContent with the JSON body
                StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                // Send the PUT request
                HttpResponseMessage response = await client.PutAsync(url, content);

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("User updated successfully.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        internal static async Task UpdateSpecificBankAccountInDB(string uri, BankAccount bankAccount)
        {
            AnsiConsole.Write(new FigletText($"Updating bank account in DB"));
            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Construct the URL for the specific bankAccount
                string url = $"{uri}/BankAccount/{bankAccount.OwnerUserId}";


                string jsonUser = JsonConvert.SerializeObject(bankAccount, Formatting.Indented);

                // Create a StringContent with the JSON body
                StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                // Send the PUT request
                HttpResponseMessage response = await client.PutAsync(url, content);

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("User updated successfully.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }


        internal static async Task UpdateAllUsers(string uri, List<User> users)
        {
            AnsiConsole.Write(new FigletText($"Updating all users"));
            using (HttpClient client = new HttpClient())
            {
                foreach (var user in users)
                {
                    // Construct the URL for the specific bankAccount
                    string url = $"{uri}{user.GetUserId()}";

                    string jsonUser = JsonConvert.SerializeObject(user, Formatting.Indented);

                    // Create a StringContent with the JSON body
                    StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                    // Send the PUT request
                    HttpResponseMessage response = await client.PutAsync(url, content);

                    // Check the response status
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("User updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
        }


    }
}
