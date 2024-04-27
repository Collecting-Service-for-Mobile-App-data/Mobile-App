using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CordelUTE;
using System.IO;
using System.Net.Http.Headers;

namespace MauiApp1;

public class ApiService
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string _baseUrl = "http://129.241.153.179:8080";

    public ApiService()
    {

    }

    public async Task<(bool isLogIn, string errorMessage)> LoginAsync(LoginRequest request)
    {
        var jsonRequest = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/user/authenticate", content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var parsedJson = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse);
            var token = parsedJson["jwt"];
            await SecureStorage.Default.SetAsync("jwt", token);
            return (true, null);
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return (false, errorMessage);
        }
    }

    public async Task StoreUserId() {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.Default.GetAsync("jwt"));
        var response = await httpClient.GetAsync($"{_baseUrl}/api/user/sessionuser");
        if (response.IsSuccessStatusCode) {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var parsedJson = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);
            if (parsedJson.ContainsKey("id")) {
                var userId = parsedJson["id"].ToString();
                await SecureStorage.Default.SetAsync("userId", userId);
            }
        }
    }

 public async Task<bool> UploadFileAsync(string filePath)
    {
        HttpClient httpClient = new HttpClient();
        string jwtToken = await SecureStorage.Default.GetAsync("jwt");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        // Get user information (assuming `getUser` is implemented as you've shown)
        User user = await getUser();

        // Prepare the multipart content
        MultipartFormDataContent multiContent = new MultipartFormDataContent();

        // Set the date to the current timestamp
        string date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

        // Read the file and add it to the multipart content
        byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
        ByteArrayContent fileContent = new ByteArrayContent(fileBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        multiContent.Add(new StringContent(date), "date");
        multiContent.Add(new StringContent(user.id.ToString()), "userId");
        multiContent.Add(new StringContent(user.companyId.ToString()), "companyId");
        multiContent.Add(new StringContent("false"), "isChecked");
        multiContent.Add(fileContent, "file", Path.GetFileName(filePath));

        // Send the POST request
        HttpResponseMessage response = await httpClient.PostAsync($"{_baseUrl}/api/sqlite-files/upload", multiContent);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("File uploaded successfully.");
            return true;
        }
        else
        {
            Console.WriteLine("File upload failed with status: " + response.StatusCode);
            return false;
        }
    }


    private async Task<User> getUser() {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.Default.GetAsync("jwt"));
        var id = await SecureStorage.Default.GetAsync("userId");
        var response = await httpClient.GetAsync($"{_baseUrl}/api/user/getuser/{id}");
        if(response.IsSuccessStatusCode) {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonDocument.Parse(jsonResponse).RootElement;
            User user = new User
            {
                id = jsonObject.GetProperty("id").GetInt64(),
                email = jsonObject.GetProperty("email").GetString(),
                companyId = jsonObject.GetProperty("company").GetProperty("id").GetInt64()  // Extract the company ID
            };
            return user;
        }
        return null;
    }

    

    public async Task<bool> SignupAsync(SignupRequest request)
    {
        try
        {
            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/user", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            // Log or handle the exception as needed.
            Console.WriteLine($"An exception occurred during signup: {ex.Message}");
            return false;
        }
    }

    public async Task<List<Company>> GetCompaniesAsync() {
        List<Company> companies = new List<Company>();
        try {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/company/companies");
            if(response.StatusCode == HttpStatusCode.OK) {
                var responseContent = await response.Content.ReadAsStringAsync();
                companies = JsonSerializer.Deserialize<List<Company>>(responseContent);
            }
            else {
                Console.WriteLine(response.StatusCode.ToString());
            }
        }
        catch(Exception e) {
            Console.WriteLine(e);
        }
        return companies;
    }


}
