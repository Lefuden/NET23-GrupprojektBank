using NET23_GrupprojektBank.Users;
using Newtonsoft.Json;
using System.Text;

namespace NET23_GrupprojektBank.Managers.Database
{
    internal static class DatabaseManager
    {
        internal static async Task<List<User>> GetAllUsersFromDB(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{uri}");
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
                var response = await client.GetAsync($"{uri}/{userId}");
                await Console.Out.WriteLineAsync(response.StatusCode.ToString());
                List<User> users = new();
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

        internal static async Task PostSpecificUser(string apiUrl, User user)
        {
            using (HttpClient client = new HttpClient())
            {
                // Serialize user object to JSON
                string jsonUser = JsonConvert.SerializeObject(user);
                Console.WriteLine(jsonUser);
                // Create StringContent with JSON data
                StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                // Send POST request
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

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

        internal static async Task UpdateSpecificUser(string uri, User userToUpdate)
        {
            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Construct the URL for the specific user
                string url = $"{uri}{userToUpdate.GetUserId()}";

                // Convert the User object to JSON
                string jsonBody = JsonConvert.SerializeObject(userToUpdate);

                // Create a StringContent with the JSON body
                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

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
